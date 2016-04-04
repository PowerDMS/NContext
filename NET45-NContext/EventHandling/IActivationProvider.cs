namespace NContext.EventHandling
{
    using System;

    /// <summary>
    /// Defines an extensibility point that allows you to implement 
    /// your own factory for resolving event handlers at runtime.
    /// </summary>
    public interface IActivationProvider
    {
        /// <summary>
        /// Creates the handler instance.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler to create.</param>
        /// <returns>IHandleEvent{TEvent}.</returns>
        IHandleEvent<TEvent> CreateInstance<TEvent>(Type handler);
    }
}