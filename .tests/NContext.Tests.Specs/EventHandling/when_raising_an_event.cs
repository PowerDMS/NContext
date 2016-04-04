namespace NContext.Tests.Specs.EventHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Configuration;
    using NContext.EventHandling;
    
    public abstract class when_raising_an_event
    {
        Establish context = () =>
            {
                HandledEvents = new ConcurrentBag<object>();
                ActivationProvider = new DefaultActivationProvider();
                EventManager = new EventManager(ActivationProvider);

                var appConfig = A.Fake<ApplicationConfigurationBase>();
                A.CallTo(() => appConfig.CompositionContainer)
                    .Returns(
                        new CompositionContainer(
                            new AggregateCatalog(
                                new AssemblyCatalog(Assembly.Load("NContext")), new AssemblyCatalog(Assembly.GetExecutingAssembly()))));

                EventManager.Configure(appConfig);
            };

        Because of = async () => await EventManager.Raise(Event).Await().AsTask;

        public static ConcurrentBag<object> HandledEvents;

        protected static IManageEvents EventManager;

        protected static IActivationProvider ActivationProvider;

        protected static object Event;
    }
}