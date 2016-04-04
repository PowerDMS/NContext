namespace NContext.Tests.Specs.EventHandling.Serial
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;

    using Machine.Specifications;

    public class with_serial_handlers : when_raising_an_event
    {
        Establish context = () => Event = new SerialEvent();

        It should_handle_event_serially = 
            () => EventHandlerThreadIds.All(id => id.Equals(Thread.CurrentThread.ManagedThreadId)).ShouldBeTrue();

        public static ConcurrentBag<int> EventHandlerThreadIds = new ConcurrentBag<int>();
    }
}