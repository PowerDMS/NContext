namespace NContext.Tests.Specs.EventHandling
{
    using System.Threading.Tasks;

    using NContext.EventHandling;

    public class AsynchronousEventHandler : IHandleEventAsync<AsynchronousEvent>
    {
        public async Task Handle(AsynchronousEvent @event)
        {
            await Task.Delay(2000);

            when_raising_an_event.HandledEvents.Add(@event);
        }
    }
}