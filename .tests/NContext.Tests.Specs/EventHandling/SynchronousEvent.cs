namespace NContext.Tests.Specs.EventHandling
{
    using System;

    public class SynchronousEvent : DummyEvent
    {
        public SynchronousEvent(String eventParameter) 
            : base(eventParameter)
        {
        }
    }
}