namespace NContext.Extensions.AspNetWebApi.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using System.Web.Http.SelfHost;

    using NContext.Configuration;
    using NContext.Extensions.AspNetWebApi.Routing;

    /// <summary>
    /// Defines an application component manager for configuring ASP.NET Web API.
    /// </summary>
    public class WebApiManager : IManageWebApi, IManageHttpRouting
    {
        private readonly WebApiConfiguration _WebApiConfiguration;

        private readonly Lazy<HttpSelfHostServer> _SelfHostServer;

        private readonly Lazy<IList<Route>> _HttpRoutes;

        private CompositionContainer _CompositionContainer;

        private Boolean _IsConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiManager" /> class.
        /// </summary>
        /// <param name="webApiConfiguration">The Web API configuration.</param>
        /// <exception cref="System.ArgumentNullException">webApiConfiguration</exception>
        public WebApiManager(WebApiConfiguration webApiConfiguration)
        {
            if (webApiConfiguration == null)
            {
                throw new ArgumentNullException("webApiConfiguration");
            }

            _HttpRoutes = new Lazy<IList<Route>>(() => new List<Route>());
            _WebApiConfiguration = webApiConfiguration;
            
            if (webApiConfiguration.IsSelfHosted)
            {
                _SelfHostServer = new Lazy<HttpSelfHostServer>(() => new HttpSelfHostServer(WebApiConfiguration.HttpSelfHostConfiguration));
            }
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
            get
            {
                return HttpConfiguration.Routes;
            }
        }

        /// <summary>
        /// Gets the HTTP configuration.
        /// </summary>
        /// <value>The HTTP configuration.</value>
        public HttpConfiguration HttpConfiguration
        {
            get
            {
                return CreateOrGetHttpConfiguration();
            }
        }

        /// <summary>
        /// Gets the HTTP server.
        /// </summary>
        /// <value>The HTTP server.</value>
        public HttpServer HttpServer
        {
            get
            {
                return _SelfHostServer == null ? null : _SelfHostServer.Value;
            }
        }

        /// <summary>
        /// Gets or sets the composition container.
        /// </summary>
        /// <value>The composition container.</value>
        /// <remarks></remarks>
        protected CompositionContainer CompositionContainer
        {
            get
            {
                return _CompositionContainer;
            }

            set
            {
                _CompositionContainer = value;
            }
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
            _HttpRoutes.Value.Add(new Route(routeName, routeTemplate, defaults, constraints));
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
            CompositionContainer = applicationConfiguration.CompositionContainer;

            if (!WebApiConfiguration.IsSelfHosted && WebApiConfiguration.AspNetHttpConfigurationDelegate != null)
            {
                WebApiConfiguration.AspNetHttpConfigurationDelegate.Invoke(HttpConfiguration);
            }

            var webApiConfigurations = _CompositionContainer.GetExportedValues<IConfigureWebApi>();
            foreach (var webApiConfiguration in webApiConfigurations)
            {
                webApiConfiguration.Configure(HttpConfiguration);
            }

            var routingConfigurations = _CompositionContainer.GetExportedValues<IConfigureHttpRouting>().OrderBy(c => c.Priority);
            foreach (var routingConfiguration in routingConfigurations)
            {
                routingConfiguration.Configure(this);
            }

            CreateRoutes();
            
            IsConfigured = true;
        }

        protected virtual HttpConfiguration CreateOrGetHttpConfiguration()
        {
            return _WebApiConfiguration.IsSelfHosted
                ? _WebApiConfiguration.HttpSelfHostConfiguration
                : GlobalConfiguration.Configuration;
        }

        /// <summary>
        /// Registers the routes in the routing collection.
        /// </summary>
        protected virtual void CreateRoutes()
        {
            if (WebApiConfiguration.IsSelfHosted)
            {
                _HttpRoutes.Value.ForEach(
                    route =>
                    {
                        HttpConfiguration
                            .Routes
                            .MapHttpRoute(route.RouteName, route.RouteTemplate, route.Defaults, route.Constraints);
                        });

                ((HttpSelfHostServer)HttpServer).OpenAsync().Wait();
            }
            else
            {
                _HttpRoutes.Value.ForEach(
                    route =>
                    {
                        HttpConfiguration
                            .Routes
                            .MapHttpRoute(route.RouteName, route.RouteTemplate, route.Defaults, route.Constraints);
                    });
            }
        }
    }
}