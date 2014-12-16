namespace NContext.Tests.Specs.EventHandling
{
    using System;
    using System.Threading;

    using NContext.EventHandling;

    public class GracefulEventHandler : IGracefullyHandleEvent<GracefulEvent>
    {
        public void Handle(GracefulEvent @event)
        {
            Thread.Sleep(300);

            throw new Exception("Error!");
        }

        public void HandleException(GracefulEvent @event, Exception exception)
        {
            when_raising_an_event.HandledEvents.Add(@event);

            throw exception;
        }
    }
}