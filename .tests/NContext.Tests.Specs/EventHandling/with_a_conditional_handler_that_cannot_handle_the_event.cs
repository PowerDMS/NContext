namespace NContext.Tests.Specs.EventHandling
{
    using System;

    using FakeItEasy;

    using Machine.Specifications;

    public class with_a_conditional_handler_that_cannot_handle_the_event : when_raising_an_event
    {
        Establish context = () =>
        {
            A.CallTo(() => ActivationProvider.CreateInstance<ConditionalEvent>(A<Type>._))
                .ReturnsLazily((Type handlerType) => HandlerFactory(handlerType));

            _Event = new ConditionalEvent(false);
        };

        Because of = () => EventManager.Raise(_Event);

        It should_not_handle_event = () => HandledEvents.ShouldNotContain(_Event);

        private static ConditionalEvent _Event;
    }
}