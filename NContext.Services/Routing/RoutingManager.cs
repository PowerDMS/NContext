// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingManager.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>

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

using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;

using NContext.Application.Configuration;
using NContext.Application.Services.Formatters;

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// Defines an application-level manager for configuring WCF service routes.
    /// </summary>
    public class RoutingManager : IRoutingManager
    {
        #region Fields

        private static Boolean _IsConfigured;

        private static readonly Lazy<IList<Route>> _ServiceRoutes = 
            new Lazy<IList<Route>>(() => new List<Route>());

        private static CompositionContainer _CompositionContainer;

        private static Lazy<HttpServiceHostFactory> _RestFactory;

        private static Lazy<ServiceHostFactory> _SoapFactory;

        private readonly RoutingConfiguration _RoutingConfiguration;

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

            _RoutingConfiguration = routingConfiguration;
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
            if ((_RoutingConfiguration.EndpointBinding & EndpointBinding.Rest) == EndpointBinding.Rest)
            {
                _ServiceRoutes.Value.Add(new Route(String.Format("{0}{1}", routePrefix, String.Empty), 
                                                          typeof(TServiceContract), 
                                                          typeof(TService), 
                                                          EndpointBinding.Rest));
            }

            if ((_RoutingConfiguration.EndpointBinding & EndpointBinding.Soap) == EndpointBinding.Soap)
            {
                // TODO: (DG) Add support for customizing soap endpoint postfix address. (/soap)
                _ServiceRoutes.Value.Add(new Route(String.Format("{0}{1}", routePrefix, "/soap"), 
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

                var httpConfiguration = new WebApiConfiguration();
                httpConfiguration.Formatters.AddRange(new JsonNetMediaTypeFormatter(), new XmlDataContractMediaTypeFormatter());
                httpConfiguration.CreateInstance = _RoutingConfiguration.ResourceFactory;
                httpConfiguration.MessageHandlerFactory = _RoutingConfiguration.MessageHandlerFactory;

                _RestFactory = new Lazy<HttpServiceHostFactory>(() => new HttpServiceHostFactory { Configuration = httpConfiguration });
                _SoapFactory = new Lazy<ServiceHostFactory>(() => new ServiceHostFactory());

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