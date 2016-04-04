namespace NContext.EventHandling
{
    using System;
    using System.Threading.Tasks;

    using NContext.Configuration;

    /// <summary>
    /// Defines an application component for event handling.
    /// </summary>
    public interface IManageEvents : IApplicationComponent
    {
        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <exception cref="System.AggregateException">Thrown if the <see cref="IActivationProvider"/> 
        /// cannot create an instance of the handler.</exception>
        /// <returns>Task.</returns>
        Task Raise(Object @event);
    }
}