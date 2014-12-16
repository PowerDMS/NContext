namespace NContext.Tests.Specs.EventHandling
{
    using System.Threading;

    using NContext.EventHandling;

    public class SynchronousEventHandler : IHandleEvent<SynchronousEvent>
    {
        public void Handle(SynchronousEvent @event)
        {
            Thread.Sleep(300);

            when_raising_an_event.HandledEvents.Add(@event);
        }
    }
}