namespace NContext.Tests.Specs.EventHandling.Serial
{
    using System.Collections.Concurrent;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Common;

    using Configuration;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Extensions;
    using NContext.EventHandling;

    public class with_exception_serial_handlers
    {
        Establish context = () =>
        {
            Event = new ExceptionThrowingEvent();
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

        Because of = () =>
        {
            var serviceResponse = Task.FromResult<IServiceResponse<int>>(new DataResponse<int>(0));
            for (int i = 0; i < 10000; i++)
            {
                serviceResponse = serviceResponse.AwaitRaiseAsync(EventManager, Event);
            }
        };
        
        It should_handle_event_serially =
            () => EventHandlerThreadIds.All(id => id.Equals(Thread.CurrentThread.ManagedThreadId)).ShouldBeTrue();

        public static ConcurrentBag<object> HandledEvents;

        protected static IManageEvents EventManager;

        protected static IActivationProvider ActivationProvider;

        protected static object Event;

        public static ConcurrentBag<int> EventHandlerThreadIds = new ConcurrentBag<int>();
    }
}