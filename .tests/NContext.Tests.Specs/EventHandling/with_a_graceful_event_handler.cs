namespace NContext.Tests.Specs.EventHandling
{
    using System;

    using Machine.Specifications;

    using Telerik.JustMock;

    public class with_a_graceful_event_handler : when_raising_an_event
    {
        Establish context = () =>
        {
            Mock.Arrange(() => ActivationProvider.CreateInstance<GracefulEvent>(Arg.IsAny<Type>()))
                .Returns((Type handlerType) => HandlerFactory(handlerType));

            _Event = new GracefulEvent("graceful");
        };

        Because of = () => _Exception = (AggregateException)Catch.Exception(() => EventManager.Raise(_Event).Await());

        It should_handle_event = () => HandledEvents.ShouldContain(_Event);

        It should_contain_the_exception = () => _Exception.InnerException.Message.ShouldEqual("Error!");

        private static GracefulEvent _Event;

        private static AggregateException _Exception;
    }
}