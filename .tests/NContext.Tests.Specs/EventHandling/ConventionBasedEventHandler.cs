namespace NContext.Tests.Specs.EventHandling
{
    using NContext.EventHandling;

    public class ConventionBasedEventHandler : IHandleEvents
    {
        public void Handle<TEvent>(TEvent @event)
        {
            
        }
    }
}