namespace NContext.Extensions.AspNet.WebApi.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Routing;

    using NContext.Configuration;

    using Routing;

    /// <summary>
    /// Defines an application component manager for configuring ASP.NET Web API.
    /// </summary>
    public class WebApiManager : IManageWebApi, IManageHttpRouting
    {
        private readonly WebApiConfiguration _WebApiConfiguration;

        private readonly IList<Route> _HttpRoutes;

        private HttpConfiguration _HttpConfiguration;

        private Boolean _IsConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiManager" /> class.
        /// </summary>
        /// <param name="webApiConfiguration">The Web API configuration.</param>
        /// <exception cref="System.ArgumentNullException">webApiConfiguration</exception>
        public WebApiManager(WebApiConfiguration webApiConfiguration)
        {
            if (webApiConfiguration == null)
                throw new ArgumentNullException("webApiConfiguration");

            _HttpRoutes = new List<Route>();
            _WebApiConfiguration = webApiConfiguration;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
            protected set
            {
                _IsConfigured = value;
            }
        }

        /// <summary>
        /// Gets the Web API HTTP service routes registered.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<IHttpRoute> Routes
        {
            get { return HttpConfiguration.Routes; }
        }

        /// <summary>
        /// Gets the <see cref="HttpConfiguration" /> instance used to configure Web API.
        /// </summary>
        /// <value>The HTTP configuration.</value>
        public HttpConfiguration HttpConfiguration
        {
            get { return _HttpConfiguration; }
            private set { _HttpConfiguration = value; }
        }
        
        /// <summary>
        /// Gets the web API configuration.
        /// </summary>
        /// <value>The web API configuration.</value>
        protected WebApiConfiguration WebApiConfiguration
        {
            get
            {
                return _WebApiConfiguration;
            }
        }
        
        /// <summary>
        /// Registers the HTTP service route.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeTemplate">The route URI template.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <remarks></remarks>
        public virtual void RegisterHttpRoute(String routeName, String routeTemplate, Object defaults = null, Object constraints = null)
        {
            _HttpRoutes.Add(new Route(routeName, routeTemplate, defaults, constraints));
        }

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (IsConfigured) return;

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageWebApi>(this);

            HttpConfiguration = _WebApiConfiguration.HttpConfigurationFactory();
            
            var webApiConfigurations = applicationConfiguration.CompositionContainer.GetExportedValues<IConfigureWebApi>();
            foreach (var webApiConfiguration in webApiConfigurations)
            {
                webApiConfiguration.Configure(HttpConfiguration);
            }

            var routingConfigurations = applicationConfiguration.CompositionContainer.GetExportedValues<IConfigureHttpRouting>().OrderBy(c => c.Priority);
            foreach (var routingConfiguration in routingConfigurations)
            {
                routingConfiguration.Configure(this);
            }

            _HttpRoutes.ForEach(
                route =>
                {
                    HttpConfiguration
                        .Routes
                        .MapHttpRoute(route.RouteName, route.RouteTemplate, route.Defaults, route.Constraints);
                });

            _WebApiConfiguration.HostConfigurationAction();

            IsConfigured = true;
        }
    }
}