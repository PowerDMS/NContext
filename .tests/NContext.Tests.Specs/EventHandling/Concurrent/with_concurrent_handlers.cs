namespace NContext.Tests.Specs.EventHandling.Concurrent
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;

    using Machine.Specifications;

    public class with_concurrent_handlers : when_raising_an_event
    {
        Establish context = () => Event = new ConcurrentEvent();

        It should_handle_event_concurrently = 
            () => EventHandlerThreadIds.All(id => id.Equals(Thread.CurrentThread.ManagedThreadId)).ShouldBeFalse();

        public static ConcurrentBag<int> EventHandlerThreadIds = new ConcurrentBag<int>();
    }
}