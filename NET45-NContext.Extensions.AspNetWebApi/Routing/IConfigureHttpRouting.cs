namespace NContext.Extensions.AspNetWebApi.Routing
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Defines an HTTP routing configuration extensibility point for ASP.NET Web API. Allows 
    /// for routing configuration to be done outside the hosting application.
    /// </summary>
    [InheritedExport]
    public interface IConfigureHttpRouting
    {
        /// <summary>
        /// Gets the priority in which to configure the routing. Implementations will be run 
        /// in ascending order based on priority, so a lower priority value will execute first.
        /// </summary>
        /// <remarks></remarks>
        Int32 Priority { get; }

        /// <summary>
        /// Configures the specified HTTP routing manager.
        /// </summary>
        /// <param name="routingManager">The HTTP routing manager.</param>
        void Configure(IManageHttpRouting routingManager);
    }
}