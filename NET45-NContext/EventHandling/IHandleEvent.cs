namespace NContext.EventHandling
{
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an event handler for a specific type of event.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    [InheritedExport(typeof(IHandleEvent<>))]
    public interface IHandleEvent<in TEvent>
    {
        /// <summary>
        /// Handles the specified event. This may be invoked on a different thread then the thread which raised the event.
        /// </summary>
        /// <param name="event">The event.</param>
        Task HandleAsync(TEvent @event);
    }
}