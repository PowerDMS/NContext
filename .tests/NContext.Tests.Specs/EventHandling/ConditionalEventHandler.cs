namespace NContext.Tests.Specs.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using NContext.EventHandling;

    public class ConditionalEventHandler : IConditionallyHandleEvents
    {
        public Boolean CanHandle<TEvent>(TEvent @event)
        {
            var e = (@event as ConditionalEvent);

            return e != null && e.CanHandle;
        }

        public async Task Handle<TEvent>(TEvent @event)
        {
            await Task.Delay(300);

            when_raising_an_event.HandledEvents.Add(@event as DummyEvent);
        }
    }
}