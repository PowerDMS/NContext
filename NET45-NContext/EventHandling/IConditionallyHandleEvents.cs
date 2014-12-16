namespace NContext.EventHandling
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a conditional event-handler abstraction.
    /// </summary>
    [InheritedExport]
    public interface IConditionallyHandleEvents : IHandleEvents
    {
        /// <summary>
        /// Determines whether this instance can handle the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        Boolean CanHandle<TEvent>(TEvent @event);

        /// <summary>
        /// Handles the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        Task Handle<TEvent>(TEvent @event);
    }
}