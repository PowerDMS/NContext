namespace NContext.EventHandling
{
    using System;

    /// <summary>
    /// Defines an event handler for a specific type of event.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IGracefullyHandleEvent<in TEvent> : IHandleEvent<TEvent>
    {
        /// <summary>
        /// Invoked when an exception is thrown within execution of <see cref="IHandleEvent{T}.Handle"/>.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="exception">The exception.</param>
        void HandleException(TEvent @event, Exception exception); 
    }
}