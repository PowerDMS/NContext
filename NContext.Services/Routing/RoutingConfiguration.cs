// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingConfiguration.cs">
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
//
// <summary>
//   Defines a component configuration class for service routing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.ApplicationServer.Http;

using NContext.Application.Configuration;

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// Defines a component configuration class for service routing.
    /// </summary>
    public class RoutingConfiguration : ApplicationComponentConfigurationBase
    {
        #region Fields
        
        private EndpointBinding _EndpointBinding;

        private String _RestEndpointPostfix;

        private String _SoapEndpointPostfix;

        private Lazy<HttpConfiguration> _HttpConfiguration;

        private readonly Lazy<RoutingConfigurationBuilder> _RoutingConfigurationBuilder;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public RoutingConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
            _RoutingConfigurationBuilder = new Lazy<RoutingConfigurationBuilder>(() => new RoutingConfigurationBuilder(this));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the endpoint bindings.
        /// </summary>
        /// <remarks></remarks>
        public EndpointBinding EndpointBinding
        {
            get
            {
                return _EndpointBinding;
            }
        }

        /// <summary>
        /// Gets the rest endpoint postfix.
        /// </summary>
        /// <remarks></remarks>
        public String RestEndpointPostfix
        {
            get
            {
                return String.IsNullOrWhiteSpace(_RestEndpointPostfix)
                           ? String.Empty
                           : String.Format("/{0}", _RestEndpointPostfix);
            }
        }

        /// <summary>
        /// Gets the SOAP endpoint postfix.
        /// </summary>
        /// <remarks></remarks>
        public String SoapEndpointPostfix
        {
            get
            {
                if (_SoapEndpointPostfix == null)
                {
                    return "/soap";
                }

                return String.IsNullOrWhiteSpace(_SoapEndpointPostfix)
                           ? String.Empty
                           : String.Format("/{0}", _SoapEndpointPostfix);
            }
        }

        /// <summary>
        /// Gets the default WCF WebApi service route <see cref="HttpConfiguration"/>.
        /// </summary>
        /// <remarks></remarks>
        public HttpConfiguration HttpConfiguration
        {
            get
            {
                return _HttpConfiguration.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Configure WCF routing using the specified <typeparamref name="TWcfRoutingConfiguration"/>.
        /// </summary>
        /// <typeparam name="TWcfRoutingConfiguration">The type of <see cref="RoutingConfigurationBase"/> to use.</typeparam>
        /// <returns>Instance of <typeparamref name="TWcfRoutingConfiguration"/>.</returns>
        /// <remarks></remarks>
        public TWcfRoutingConfiguration ConfigureRouting<TWcfRoutingConfiguration>()
            where TWcfRoutingConfiguration : RoutingConfigurationBase
        {
            return (TWcfRoutingConfiguration)Activator.CreateInstance(typeof(TWcfRoutingConfiguration), Builder, _RoutingConfigurationBuilder.Value);
        }

        /// <summary>
        /// Sets the WCF supported endpoint bindings. You 
        /// may use the | operator to allow multiple bindings.
        /// </summary>
        /// <param name="endpointBindings">The endpoint bindings.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public RoutingConfiguration SetEndpointBindings(EndpointBinding endpointBindings)
        {
            _EndpointBinding = endpointBindings;
            return this;
        }

        /// <summary>
        /// Sets the endpoint postfix. Use this to differentiate your WCF WebApi & WCF Soap endpoints.
        /// </summary>
        /// <param name="restPostfix">The REST postfix.</param>
        /// <param name="soapPostfix">The SOAP postfix.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public RoutingConfiguration SetEndpointPostfix(String restPostfix = "", String soapPostfix = "soap")
        {
            _RestEndpointPostfix = restPostfix;
            _SoapEndpointPostfix = soapPostfix;
            
            return this;
        }

        /// <summary>
        /// Sets the <see cref="HttpConfiguration"/> factory used for default 
        /// WCF WebApi service route registration.
        /// </summary>
        /// <param name="httpConfigurationFactory">The HTTP configuration factory.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public RoutingConfiguration SetHttpConfigurationFactory(Func<HttpConfiguration> httpConfigurationFactory)
        {
            _HttpConfiguration = new Lazy<HttpConfiguration>(httpConfigurationFactory);
            return this;
        }

        /// <summary>
        /// Configures the <see cref="RoutingConfiguration"/> instance.
        /// </summary>
        /// <remarks></remarks>
        protected internal void ConfigureInstance()
        {
            Setup();
        }

        /// <summary>
        /// Applies the component configuration with the <see cref="IApplicationConfiguration"/>.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<IRoutingManager>(() => new RoutingManager(this));
        }

        #endregion
    }
}