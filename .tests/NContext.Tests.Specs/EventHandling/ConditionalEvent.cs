namespace NContext.Tests.Specs.EventHandling
{
    using System;

    public class ConditionalEvent : DummyEvent
    {
        private readonly Boolean _CanHandle;

        public ConditionalEvent(Boolean canHandle)
            : base("conditional")
        {
            _CanHandle = canHandle;
        }

        public Boolean CanHandle
        {
            get { return _CanHandle; }
        }
    }
}