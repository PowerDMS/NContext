namespace NContext.Tests.Specs.EventHandling
{
    public class DummyEvent
    {
        private readonly int _EventParameter;

        public DummyEvent(int EventParameter)
        {
            _EventParameter = EventParameter;
        }

        public int EventParameter
        {
            get { return _EventParameter; }
        }
    }
}