namespace NContext.Tests.Specs.EventHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;

    using Machine.Specifications;

    using NContext.Configuration;
    using NContext.EventHandling;

    using Telerik.JustMock;

    public abstract class when_raising_an_event
    {
        private static readonly ConcurrentBag<DummyEvent> _HandledEvents = new ConcurrentBag<DummyEvent>();

        private static IManageEvents _EventManager;

        private static IActivationProvider _ActivationProvider;

        Establish context = () =>
            {
                var appConfig = Mock.Create<ApplicationConfigurationBase>();
                _ActivationProvider = Mock.Create<IActivationProvider>();

                Mock.Arrange(() => appConfig.CompositionContainer)
                    .Returns(new CompositionContainer(new AggregateCatalog(new AssemblyCatalog(Assembly.Load("NContext")), new AssemblyCatalog(Assembly.GetExecutingAssembly()))));
                
                _EventManager = new EventManager(ActivationProvider);
                _EventManager.Configure(appConfig);
            };

        public static ConcurrentBag<DummyEvent> HandledEvents
        {
            get { return _HandledEvents; }
        }

        protected static IManageEvents EventManager
        {
            get { return _EventManager; }
        }

        protected static IActivationProvider ActivationProvider
        {
            get { return _ActivationProvider; }
        }

        protected static IHandleEvents HandlerFactory(Type handlerType)
        {
            return (IHandleEvents) Activator.CreateInstance(handlerType);
        }
    }
}