namespace NContext.Extensions.AspNetWebApi.Configuration
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using System.Web.Http.SelfHost;

    using NContext.Configuration;

    /// <summary>
    /// Defines an application component manager for configuring ASP.NET Web API.
    /// </summary>
    public interface IManageWebApi : IApplicationComponent
    {
        /// <summary>
        /// Gets the Web API HTTP service routes registered.
        /// </summary>
        /// <remarks></remarks>
        IEnumerable<IHttpRoute> Routes { get; }

        /// <summary>
        /// Gets the <see cref="HttpConfiguration"/> instance used to configure Web API.
        /// </summary>
        HttpConfiguration HttpConfiguration { get; }

        /// <summary>
        /// Gets the <see cref="HttpSelfHostServer"/> instance if used with <see cref="WebApiManagerBuilder.ConfigureForSelfHosting"/>
        /// </summary>
        HttpServer HttpServer { get; }
    }
}