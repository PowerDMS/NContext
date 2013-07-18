namespace NContext.Tests.Specs.EventHandling
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;

    using Machine.Specifications;

    using NContext.EventHandling;

    using Telerik.JustMock;

    public class with_concurrent_handlers : when_raising_an_event
    {
        static Int32 _Result;

        static Int32 _RaisedThreadId;

        static ICollection<Int32> _HandledThreadIds;

        Establish context = () =>
            {
                _HandledThreadIds = new Collection<Int32>();

                Func<IHandleEvent<DummyEvent>> factory = () =>
                    {
                        var handler = Mock.Create<IHandleEvent<DummyEvent>>(c => c.CallConstructor(() => new DummyEventHandler()));

                        Mock.Arrange(() => handler.Handle(Arg.IsAny<DummyEvent>()))
                            .DoInstead((DummyEvent e) =>
                                {
                                    _Result = e.EventParameter;
                                    _HandledThreadIds.Add(Thread.CurrentThread.ManagedThreadId);
                                });

                        return handler;
                    };

                Mock.Arrange(() => ActivationProvider.CreateInstance<DummyEvent>(Arg.IsAny<Type>()))
                    .Returns(factory);
            };

        Because of = () =>
            {
                _RaisedThreadId = Thread.CurrentThread.ManagedThreadId;

                EventManager.Raise(new DummyEvent(12)).Wait();
            };

        It should_handle_the_event = () => _Result.ShouldEqual(12);

        It should_handle_the_event_on_different_threads = () =>
            {
                Console.WriteLine(String.Join(", ", _HandledThreadIds));
                _HandledThreadIds.ShouldNotOnlyContain(_RaisedThreadId);
            };
    }
}