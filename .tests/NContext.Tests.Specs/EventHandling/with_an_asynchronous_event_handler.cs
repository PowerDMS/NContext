namespace NContext.Tests.Specs.EventHandling
{
    using System;

    using Machine.Specifications;

    using Telerik.JustMock;

    public class with_an_asynchronous_event_handler : when_raising_an_event
    {
        Establish context = () =>
        {
            Mock.Arrange(() => ActivationProvider.CreateInstance<AsynchronousEvent>(Arg.IsAny<Type>()))
                .Returns((Type handlerType) => HandlerFactory(handlerType));

            _Event = new AsynchronousEvent("asynchronous");
        };

        Because of = async () => await EventManager.Raise(_Event).Await().AsTask;

        It should_handle_event = () => HandledEvents.ShouldContain(_Event);

        private static AsynchronousEvent _Event;
    }
}