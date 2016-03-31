namespace NContext.EventHandling
{
    using System;

    /// <summary>
    /// <see cref="Activator"/>-based implementation.
    /// </summary>
    public class DefaultActivationProvider : IActivationProvider
    {
        /// <summary>
        /// Creates the handler instance.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler to create.</param>
        /// <returns>IHandleEvent{TEvent}.</returns>
        public IHandleEvents CreateInstance<TEvent>(Type handler)
        {
            return Activator.CreateInstance(handler) as IHandleEvents;
        }
    }
}