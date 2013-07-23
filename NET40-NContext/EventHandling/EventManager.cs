// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventManager.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.EventHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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
        private static readonly IDictionary<Type, IEnumerable<Type>> _EventHandlerCache;

        private static IActivationProvider _ActivationProvider;

        private static CompositionContainer _CompositionContainer;

        private static IEnumerable<Type> _ConditionalEventHandlers;

        private Boolean _IsConfigured;

        static EventManager()
        {
            _EventHandlerCache = new ConcurrentDictionary<Type, IEnumerable<Type>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManager"/> class.
        /// </summary>
        /// <param name="activationProvider">The activation provider.</param>
        public EventManager(IActivationProvider activationProvider)
        {
            _ActivationProvider = activationProvider;
        }

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public Task Raise<TEvent>(TEvent @event)
        {
            return RaiseEvent(@event);
        }

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public static Task RaiseEvent<TEvent>(TEvent @event)
        {
#if NET45_OR_GREATER
            return Task.Run(() => RaiseEventInternal(@event));
#else
            return Task.Factory.StartNew(() => RaiseEventInternal(@event));
#endif
        }

        private static Task RaiseEventInternal<TEvent>(TEvent @event)
        {
            IEnumerable<Type> handlerTypes;
            if (_EventHandlerCache.ContainsKey(typeof(TEvent)))
            {
                handlerTypes = _EventHandlerCache[typeof(TEvent)];
            }
            else
            {
                _EventHandlerCache[typeof(TEvent)] = handlerTypes = _CompositionContainer.GetExportTypesThatImplement<IHandleEvent<TEvent>>().ToList();
            }

            var tcs = new TaskCompletionSource<Object>();
            var exceptions = new ConcurrentQueue<Exception>();

            handlerTypes
                .Concat(_ConditionalEventHandlers)
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .ForAll(handlerType =>
                    {
                        try
                        {
                            var handler = _ActivationProvider.CreateInstance<TEvent>(handlerType);
                            if (handler == null)
                            {
                                throw new Exception(String.Format("Activation provider could not create an instance of event handler: {0}.", handlerType.Name));
                            }

                            try
                            {
                                if (handlerType.Implements<IConditionallyHandleEvents>())
                                {
                                    var conditionalHandler = (IConditionallyHandleEvents)handler;
                                    if (conditionalHandler.CanHandle(@event))
                                    {
                                        conditionalHandler.Handle(@event);
                                    }
                                }
                                else
                                {
                                    ((IHandleEvent<TEvent>)handler).Handle(@event);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (handler.GetType().Implements<IGracefullyHandleEvent<TEvent>>())
                                {
                                    ((IGracefullyHandleEvent<TEvent>)handler).HandleException(@event, ex);

                                    return;
                                }

                                exceptions.Enqueue(ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptions.Enqueue(ex);
                        }
                    });

            if (exceptions.Any())
            {
                tcs.SetException(new AggregateException(exceptions));
            }
            else
            {
                tcs.SetResult(null);
            }

            return tcs.Task;
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
            _ConditionalEventHandlers = _CompositionContainer.GetExportTypesThatImplement<IConditionallyHandleEvents>();

            _IsConfigured = true;
        }
    }
}