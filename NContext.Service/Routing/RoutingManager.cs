// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingManager.cs">
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
//
// <summary>
//   Defines an application-level manager for configuring WCF service routes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web.Routing;

using Microsoft.ApplicationServer.Http.Activation;

using NContext.Application.Configuration;

namespace NContext.Service.Routing
{
    /// <summary>
    /// Defines an application-level manager for configuring WCF service routes.
    /// </summary>
    public class RoutingManager : IRoutingManager
    {
        #region Fields

        protected readonly RoutingConfiguration RoutingConfiguration;

        private static Boolean _IsConfigured;

        private static CompositionContainer _CompositionContainer;

        private static Lazy<HttpServiceHostFactory> _RestFactory;

        private static Lazy<ServiceHostFactory> _SoapFactory;

        private static readonly Lazy<IList<Route>> _ServiceRoutes = 
            new Lazy<IList<Route>>(() => new List<Route>());

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutingManager"/> class.
        /// Prevents a default instance of the <see cref="RoutingManager"/> class from being created.
        /// </summary>
        /// <param name="routingConfiguration">The routing configuration.</param>
        /// <remarks></remarks>
        public RoutingManager(RoutingConfiguration routingConfiguration)
        {
            if (routingConfiguration == null)
            {
                throw new ArgumentNullException("routingConfiguration");
            }

            RoutingConfiguration = routingConfiguration;
        }

        protected RoutingManager()
        {
        }

        #endregion

        #region Properties

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
        /// Gets the routes.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<Route> ServiceRoutes
        {
            get
            {
                return _ServiceRoutes.Value.OrderByDescending(r => r.RoutePrefix);
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
        /// Gets or sets the REST <see cref="HttpServiceHostFactory"/>.
        /// </summary>
        /// <value>The rest factory.</value>
        /// <remarks></remarks>
        protected Lazy<HttpServiceHostFactory> RestFactory
        {
            get
            {
                return _RestFactory;
            }

            set
            {
                _RestFactory = value;
            }
        }

        /// <summary>
        /// Gets or sets the SOAP <see cref="ServiceHostFactory"/>.
        /// </summary>
        /// <value>The SOAP factory.</value>
        /// <remarks></remarks>
        protected Lazy<ServiceHostFactory> SoapFactory
        {
            get
            {
                return _SoapFactory;
            }

            set
            {
                _SoapFactory = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the service route.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <remarks></remarks>
        public virtual void RegisterServiceRoute<TServiceContract, TService>(String routePrefix)
        {
            if ((RoutingConfiguration.EndpointBinding & EndpointBinding.Rest) == EndpointBinding.Rest)
            {
                _ServiceRoutes.Value.Add(new Route(String.Format("{0}{1}", routePrefix, RoutingConfiguration.RestEndpointPostfix), 
                                                          typeof(TServiceContract), 
                                                          typeof(TService), 
                                                          EndpointBinding.Rest));
            }

            if ((RoutingConfiguration.EndpointBinding & EndpointBinding.Soap) == EndpointBinding.Soap)
            {
                _ServiceRoutes.Value.Add(new Route(String.Format("{0}{1}", routePrefix, RoutingConfiguration.SoapEndpointPostfix), 
                                                          typeof(TServiceContract), 
                                                          typeof(TService), 
                                                          EndpointBinding.Soap));
            }
        }

        /// <summary>
        /// Registers the routes in the routing table.
        /// </summary>
        protected virtual void CreateRoutes()
        {
            var serviceRouteCreatedActions = _CompositionContainer.GetExports<IRunWhenAServiceRouteIsCreated>().ToList();
            foreach (var route in _ServiceRoutes.Value.OrderByDescending(r => r.RoutePrefix))
            {
                // TODO: (DG) Add support for custom route configuration! (route.RestConfiguration ?? _RestFactory.Value)
                RouteBase serviceRoute = route.Binding == EndpointBinding.Rest
                                             ? new WebApiRoute(route.RoutePrefix, _RestFactory.Value, route.ServiceType)
                                             : new ServiceRoute(route.RoutePrefix, _SoapFactory.Value, route.ServiceType);

                RouteTable.Routes.Add(serviceRoute);
                serviceRouteCreatedActions.ForEach(createdAction => createdAction.Value.Run(route));
            }
        }

        #endregion

        #region Implementation of IApplicationComponent

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public virtual void Configure(IApplicationConfiguration applicationConfiguration)
        {
            if (!_IsConfigured)
            {
                _CompositionContainer = applicationConfiguration.CompositionContainer;
                _SoapFactory = new Lazy<ServiceHostFactory>(() => new ServiceHostFactory());
                _RestFactory = new Lazy<HttpServiceHostFactory>(() => 
                    new HttpServiceHostFactory
                        {
                            Configuration = RoutingConfiguration.HttpConfiguration
                        });

                var serviceConfigurables = _CompositionContainer.GetExports<IConfigureServiceRoutes>();
                foreach (var serviceConfigurable in serviceConfigurables)
                {
                    serviceConfigurable.Value.RegisterServiceRoutes(this);
                }

                CreateRoutes();

                _IsConfigured = true;
            }
        }

        #endregion
    }
}