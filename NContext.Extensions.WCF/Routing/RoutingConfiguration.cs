// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingConfiguration.cs">
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
//   Defines a component configuration class for service routing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.ApplicationServer.Http;

using NContext.Configuration;

namespace NContext.Extensions.WCF.Routing
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
        /// Configure WCF routing using the specified <typeparamref name="TRoutingConfiguration"/>.
        /// </summary>
        /// <typeparam name="TRoutingConfiguration">The type of <see cref="RoutingConfigurationBase"/> to use.</typeparam>
        /// <returns>Instance of <typeparamref name="TRoutingConfiguration"/>.</returns>
        /// <remarks></remarks>
        public TRoutingConfiguration ConfigureRouting<TRoutingConfiguration>()
            where TRoutingConfiguration : RoutingConfigurationBase
        {
            return (TRoutingConfiguration)Activator.CreateInstance(typeof(TRoutingConfiguration), Builder, _RoutingConfigurationBuilder.Value);
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
        protected internal virtual void ConfigureInstance()
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
                   .RegisterComponent<IManageRouting>(() => new RoutingManager(this));
        }

        #endregion
    }
}