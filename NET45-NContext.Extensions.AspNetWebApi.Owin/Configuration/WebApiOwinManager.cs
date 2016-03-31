namespace NContext.Extensions.AspNetWebApi.Owin.Configuration
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Web.Http;

    using global::Owin;

    using NContext.Configuration;
    using NContext.Extensions.AspNetWebApi.Configuration;
    using NContext.Extensions.AspNetWebApi.Routing;

    public class WebApiOwinManager : WebApiManager
    {
        private readonly IAppBuilder _AppBuilder;

        public WebApiOwinManager(IAppBuilder appBuilder)
            : base()
        {
            _AppBuilder = appBuilder;
        }

        /// <summary>
        /// Gets the OWIN application builder.
        /// </summary>
        /// <value>The application builder.</value>
        public IAppBuilder AppBuilder
        {
            get { return _AppBuilder; }
        }

        /// <summary>
        /// Configures the specified application configuration.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        public override void ConfigureHost()
        {
            AppBuilder.UseWebApi(HttpConfiguration);
        }
    }
}