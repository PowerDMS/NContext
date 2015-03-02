namespace NContext.Tests.Specs.EventHandling
{
    using System;

    using Machine.Specifications;

    using Telerik.JustMock;

    public class with_a_conditional_handler_that_can_handle_the_event : when_raising_an_event
    {
        Establish context = () =>
        {
            Mock.Arrange(() => ActivationProvider.CreateInstance<ConditionalEvent>(Arg.IsAny<Type>()))
                .Returns((Type handlerType) => HandlerFactory(handlerType));

            _Event = new ConditionalEvent(true);
        };

        Because of = async () => await EventManager.Raise(_Event).Await().AsTask;

        It should_handle_event = () => HandledEvents.ShouldContain(_Event);

        private static ConditionalEvent _Event;
    }
}