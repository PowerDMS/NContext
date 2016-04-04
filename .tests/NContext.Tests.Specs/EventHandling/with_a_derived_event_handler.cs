namespace NContext.Tests.Specs.EventHandling
{
    using Machine.Specifications;

    public class with_a_derived_event_handler : when_raising_an_event
    {
        Establish context = () => Event = new DerivedEvent();
        
        It should_handle_event = () => HandledEvents.ShouldContain(Event);
    }
}