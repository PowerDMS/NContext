namespace NContext.Tests.Specs.EventHandling
{
    using System;

    using FakeItEasy;

    using Machine.Specifications;

    public class with_a_conditional_handler_that_can_handle_the_event : when_raising_an_event
    {
        Establish context = () =>
        {
            A.CallTo(() => ActivationProvider.CreateInstance<ConditionalEvent>(A<Type>._))
                .ReturnsLazily((Type handlerType) => HandlerFactory(handlerType));

            _Event = new ConditionalEvent(true);
        };

        Because of = async () => await EventManager.Raise(_Event).Await().AsTask;

        It should_handle_event = () => HandledEvents.ShouldContain(_Event);

        private static ConditionalEvent _Event;
    }
}