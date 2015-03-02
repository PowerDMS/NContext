namespace NContext.Tests.Specs.EventHandling
{
    using System;

    public abstract class DummyEvent
    {
        private readonly String _EventParameter;

        public DummyEvent(String eventParameter)
        {
            _EventParameter = eventParameter;
        }

        public String EventParameter
        {
            get { return _EventParameter; }
        }
    }

    public class AsynchronousEvent : DummyEvent
    {
        public AsynchronousEvent(String eventParameter)
            : base(eventParameter)
        {
        }
    }
}