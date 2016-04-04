namespace NContext.EventHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using NContext.Configuration;
    using NContext.Extensions;

    /// <summary>
    /// Defines an application component for event handling.
    /// </summary>
    public class EventManager : IManageEvents
    {
        private readonly ConcurrentDictionary<Type, EventInformation> _EventHandlerCache;

        private readonly IActivationProvider _ActivationProvider;

        private readonly MethodInfo _ActivationProviderCreateInstance;

        private CompositionContainer _CompositionContainer;

        private Boolean _IsConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManager"/> class.
        /// </summary>
        /// <param name="activationProvider">The activation provider.</param>
        public EventManager(IActivationProvider activationProvider)
        {
            _EventHandlerCache = new ConcurrentDictionary<Type, EventInformation>();
            _ActivationProvider = activationProvider;
            _ActivationProviderCreateInstance = activationProvider
                .GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Single(m => m.IsGenericMethodDefinition && m.Name.Equals("CreateInstance", StringComparison.Ordinal));
        }
        
        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public Task Raise(object @event)
        {
            return RaiseEventInternal(@event);
        }

        /// <summary>
        /// Determines the number of partitions to create for concurrent execution of processing event handlers.  This will 
        /// affect the number of thread-pool threads queued to execute handlers in parallel. By default, <see cref="EventManager"/> 
        /// will only process a maximum of <see cref="Environment.ProcessorCount"/> (one thread per virtual processor) 
        /// unless the <paramref name="handlerCount"/> is less than the <see cref="Environment.ProcessorCount"/> value. 
        /// In which it will partition according to the <paramref name="handlerCount"/>.
        /// </summary>
        /// <param name="handlerCount"></param>
        /// <returns></returns>
        protected virtual Int32 GetPartitionCount(Int32 handlerCount)
        {
            return handlerCount > Environment.ProcessorCount
                ? Environment.ProcessorCount
                : handlerCount;
        }

        private async Task RaiseEventInternal(object @event)
        {
            var eventType = @event.GetType();
            var eventInformation = _EventHandlerCache.GetOrAdd(
                eventType,
                _ =>
                {
                    var eventHandlerInterfaceType = typeof (IHandleEvent<>).MakeGenericType(eventType);
                    var eventHandlers = _CompositionContainer.GetExportTypesThatImplement(eventHandlerInterfaceType)
                        .Select(handlerType =>
                        {
                            ParameterInfo[] parameters;
                            var handleMethod = handlerType
                                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                .Single(m => 
                                    m.Name.Equals("HandleAsync", StringComparison.Ordinal) &&
                                    (parameters = m.GetParameters()).Length == 1 &&
                                    parameters[0].ParameterType == eventType);

                            return new EventHandlerInformation(handlerType, handleMethod);
                        })
                        .ToList();
                    var parallelizeEvent = TypeDescriptor.GetAttributes(eventType).Contains(new HandleConcurrently());
                    var createInstanceMethod = _ActivationProviderCreateInstance.MakeGenericMethod(eventType);

                    if (eventHandlers.Count == 0)
                        throw new InvalidOperationException(
                            String.Format(
                                "There is no event handler for event type '{0}'.  You must create a class which implements IHandleEvent<{0}>.", 
                                eventType.Name));

                    return new EventInformation(
                        eventHandlers, 
                        parallelizeEvent,
                        createInstanceMethod);
                });

            if (eventInformation.Parallelize)
            {
                await Task.WhenAll(
                    from partition in Partitioner.Create(eventInformation.Handlers)
                        .GetPartitions(GetPartitionCount(eventInformation.Handlers.Count))
                    select Task.Run(async () =>
                    {
                        using (partition)
                            while (partition.MoveNext())
                                await InvokeEventHandler(partition.Current, eventInformation.CreateInstanceMethod, @event);
                    }));

                return;
            }

            foreach (var handlerInformation in eventInformation.Handlers)
            {
                await InvokeEventHandler(handlerInformation, eventInformation.CreateInstanceMethod, @event);
            }
        }

        private Task InvokeEventHandler(EventHandlerInformation handlerInformation, MethodInfo createInstanceMethod, object @event)
        {
            var handler = createInstanceMethod.Invoke(_ActivationProvider, new object[] { handlerInformation.HandlerType });
            if (handler == null)
            {
                throw new Exception(
                    String.Format(
                        "Activation provider could not create an instance of event handler: {0}.",
                        handlerInformation.HandlerType.Name));
            }

            return (Task)handlerInformation.HandleMethod.Invoke(handler, new[] { @event });
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>The is configured.</value>
        public Boolean IsConfigured
        {
            get { return _IsConfigured; }
        }

        /// <summary>
        /// Configures the component instance. This method should set <see cref="IsConfigured" />.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        public void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageEvents>(this);
            _CompositionContainer = applicationConfiguration.CompositionContainer;

            _IsConfigured = true;
        }

        private class EventInformation
        {
            public EventInformation(
                IReadOnlyList<EventHandlerInformation> handlers, 
                Boolean parallelize,
                MethodInfo createInstanceMethod)
            {
                Handlers = handlers;
                Parallelize = parallelize;
                CreateInstanceMethod = createInstanceMethod;
            }

            public IReadOnlyList<EventHandlerInformation> Handlers { get; private set; }

            public Boolean Parallelize { get; private set; }

            public MethodInfo CreateInstanceMethod { get; private set; }
        }

        private class EventHandlerInformation
        {
            public EventHandlerInformation(Type handlerType, MethodInfo handleMethod)
            {
                HandlerType = handlerType;
                HandleMethod = handleMethod;
            }

            public Type HandlerType { get; private set; }

            public MethodInfo HandleMethod { get; private set; }
        }
    }
}