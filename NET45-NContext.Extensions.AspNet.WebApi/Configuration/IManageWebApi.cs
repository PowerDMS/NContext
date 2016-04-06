namespace NContext.Extensions.AspNet.WebApi.Configuration
{
    using System.Web.Http;

    using NContext.Configuration;

    /// <summary>
    /// Defines an application component manager for configuring ASP.NET Web API.
    /// </summary>
    public interface IManageWebApi : IApplicationComponent
    {
        /// <summary>
        /// Gets the <see cref="HttpConfiguration"/> instance used to configure Web API.
        /// </summary>
        HttpConfiguration HttpConfiguration { get; }
    }
}