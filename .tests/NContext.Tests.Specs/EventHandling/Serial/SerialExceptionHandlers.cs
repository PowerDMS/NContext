namespace NContext.Tests.Specs.EventHandling.Serial
{
    using System.Threading;
    using System.Threading.Tasks;

    using NContext.EventHandling;


    public class SerialExceptionHandler1 : IHandleEvent<ExceptionThrowingEvent>
    {
        public Task HandleAsync(ExceptionThrowingEvent @event)
        {
            with_exception_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);
            return Task.FromResult(0);
        }
    }

    public class SerialExceptionHandler2 : IHandleEvent<ExceptionThrowingEvent>
    {
        public Task HandleAsync(ExceptionThrowingEvent @event)
        {
            throw new System.InvalidOperationException();
        }
    }

    public class SerialExceptionHandler3 : IHandleEvent<ExceptionThrowingEvent>
    {
        public Task HandleAsync(ExceptionThrowingEvent @event)
        {
            with_exception_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);
            return Task.FromResult(0);
        }
    }

    public class SerialExceptionHandler4 : IHandleEvent<ExceptionThrowingEvent>
    {
        public Task HandleAsync(ExceptionThrowingEvent @event)
        {
            with_exception_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);
            return Task.FromResult(0);
        }
    }
}