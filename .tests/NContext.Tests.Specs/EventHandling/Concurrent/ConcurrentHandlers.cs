namespace NContext.Tests.Specs.EventHandling.Concurrent
{
    using System.Threading;
    using System.Threading.Tasks;

    using NContext.EventHandling;

    public class ConcurrentHandler1 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler2 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler3 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler4 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler5 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler6 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler7 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler8 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler9 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }

    public class ConcurrentHandler10 : IHandleEvent<ConcurrentEvent>
    {
        public Task HandleAsync(ConcurrentEvent @event)
        {
            with_concurrent_handlers.EventHandlerThreadIds.Add(Thread.CurrentThread.ManagedThreadId);

            return Task.FromResult(0);
        }
    }
}