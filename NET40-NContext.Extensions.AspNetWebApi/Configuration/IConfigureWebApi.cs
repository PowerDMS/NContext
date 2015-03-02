namespace NContext.Extensions.AspNetWebApi.Configuration
{
    using System.ComponentModel.Composition;
    using System.Web.Http;

    /// <summary>
    /// Defines an extensibility point for configuring <see cref="HttpConfiguration"/>.
    /// </summary>
    [InheritedExport]
    public interface IConfigureWebApi
    {
        /// <summary>
        /// Called by NContext to support Web API configuration.
        /// </summary>
        /// <param name="configuration"></param>
        void Configure(HttpConfiguration configuration);
    }
}