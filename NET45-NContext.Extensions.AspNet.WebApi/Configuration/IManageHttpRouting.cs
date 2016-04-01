namespace NContext.Extensions.AspNetWebApi.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Routing;

    /// <summary>
    /// Defines HTTP routing management for ASP.NET Web API.
    /// </summary>
    public interface IManageHttpRouting
    {
        /// <summary>
        /// Gets the Web API HTTP route collection.
        /// </summary>
        /// <remarks></remarks>
        IEnumerable<IHttpRoute> Routes { get; }
        
        /// <summary>
        /// Registers the HTTP service route.
        /// </summary>
        /// <param name="routeName">Unique route name.</param>
        /// <param name="routeTemplate">The route URI template.</param>
        /// <param name="defaults">The default values for parameter binding.</param>
        /// <param name="constraints">The constraints to be used in route.</param>
        /// <remarks></remarks>
        void RegisterHttpRoute(String routeName, String routeTemplate, Object defaults = null, Object constraints = null);
    }
}