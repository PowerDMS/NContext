namespace NContext.Tests.Specs.EventHandling
{
    using System.Threading.Tasks;

    using NContext.EventHandling;

    public class DerivedEventHandler : IHandleEvent<DerivedEvent>
    {
        public async Task HandleAsync(DerivedEvent @event)
        {
            await Task.Delay(2000);

            when_raising_an_event.HandledEvents.Add(@event);
        }
    }
}