namespace NContext.Tests.Specs.EventHandling
{
    using System;

    public class GracefulEvent : DummyEvent
    {
        public GracefulEvent(String eventParameter) 
            : base(eventParameter)
        {
        }
    }
}