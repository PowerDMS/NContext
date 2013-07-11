// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiManager.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
                return WebApiConfiguration.IsSelfHosted
                           ? SelfHostServer.Configuration.Routes.AsEnumerable()
                           : GlobalConfiguration.Configuration.Routes.AsEnumerable();
            }
        }

        public HttpConfiguration HttpConfiguration
        {
            get
            {
                return _WebApiConfiguration.IsSelfHosted
                           ? _WebApiConfiguration.HttpSelfHostConfiguration
                           : GlobalConfiguration.Configuration;
            }
        }

        public HttpSelfHostServer SelfHostServer
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
            
            var webApiConfigurations = _CompositionContainer.GetExportedValues<IConfigureWebApi>();
            if (!WebApiConfiguration.IsSelfHosted && WebApiConfiguration.AspNetHttpConfigurationDelegate != null)
            {
                WebApiConfiguration.AspNetHttpConfigurationDelegate.Invoke(HttpConfiguration);
            }

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

        /// <summary>
        /// Registers the routes in the routing collection.
        /// </summary>
        protected virtual void CreateRoutes()
        {
            var serviceRouteCreatedActions = CompositionContainer.GetExports<IRunWhenAWebApiRouteIsMapped>();
            if (WebApiConfiguration.IsSelfHosted)
            {
                _HttpRoutes.Value.ForEach(
                    route =>
                        {
                            SelfHostServer.Configuration
                                          .Routes
                                          .MapHttpRoute(route.RouteName, route.RouteTemplate, route.Defaults, route.Constraints);

                            serviceRouteCreatedActions.ForEach(createdAction => createdAction.Value.Run(route));
                        });

                SelfHostServer.OpenAsync().Wait();
            }
            else
            {
                _HttpRoutes.Value.ForEach(
                    route =>
                    {
                        GlobalConfiguration
                            .Configuration
                            .Routes
                            .MapHttpRoute(route.RouteName, route.RouteTemplate, route.Defaults, route.Constraints);

                        serviceRouteCreatedActions.ForEach(createdAction => createdAction.Value.Run(route));
                    });
            }
        }
    }
}