namespace NContext.Tests.Specs.EventHandling.Serial
{
    using System.Threading;
    using System.Threading.Tasks;

    using NContext.EventHandling;

    public class SerialHandler1 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler2 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler3 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler4 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler5 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler6 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler7 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler8 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler9 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class SerialHandler10 : IHandleEvent<SerialEvent>
    {
        public Task HandleAsync(SerialEvent @event)
        {
            with_serial_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }
}