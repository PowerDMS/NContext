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
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Xml.Serialization;

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

        private Boolean _ClearDefaultFormatters;

        private IEnumerable<MediaTypeFormatter> _MediaTypeFormatters; 

        private Func<IEnumerable<DelegatingHandler>> _MessageHandlerFactory;

        private Func<Type, InstanceContext, HttpRequestMessage, Object> _ResourceFactory;

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
                return _RestEndpointPostfix;
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
                return _SoapEndpointPostfix;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to clear the default WCF WebApi formatters.
        /// </summary>
        /// <remarks></remarks>
        public Boolean ClearDefaultFormatters
        {
            get
            {
                return _ClearDefaultFormatters;
            }
        }

        /// <summary>
        /// Gets the media type formatters.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<MediaTypeFormatter> MediaTypeFormatters
        {
            get
            {
                return _MediaTypeFormatters;
            }
        }

        /// <summary>
        /// Gets the resource resolver.
        /// </summary>
        /// <remarks></remarks>
        public Func<Type, InstanceContext, HttpRequestMessage, Object> ResourceFactory
        {
            get
            {
                return _ResourceFactory;
            }
        }

        /// <summary>
        /// Gets the HTTP message handler factory func.
        /// </summary>
        /// <remarks></remarks>
        public Func<IEnumerable<DelegatingHandler>> MessageHandlerFactory
        {
            get
            {
                return _MessageHandlerFactory;
            }
        }

        #endregion

        #region Methods

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
            _RestEndpointPostfix = String.IsNullOrWhiteSpace(restPostfix)
                                       ? String.Empty
                                       : String.Format("/{0}", restPostfix);

            if (soapPostfix == null)
            {
                _SoapEndpointPostfix = "/soap";
            }
            else
            {
                _SoapEndpointPostfix = String.IsNullOrWhiteSpace(soapPostfix)
                                           ? String.Empty
                                           : String.Format("/{0}", soapPostfix);
            }

            return this;
        }

        /// <summary>
        /// Sets the WCF WebApi <see cref="MediaTypeFormatter"/>.
        /// </summary>
        /// <param name="clearDefault">if set to <c>true</c> clears the default formatters 
        /// (ie. <see cref="XmlSerializer"/>, <see cref="DataContractJsonSerializer"/>).</param>
        /// <param name="mediaTypeFormatters">The media type formatters.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public RoutingConfiguration SetFormatters(Boolean clearDefault = false, params MediaTypeFormatter[] mediaTypeFormatters)
        {
            _ClearDefaultFormatters = clearDefault;
            _MediaTypeFormatters = mediaTypeFormatters;
            return this;
        }

        /// <summary>
        /// Sets the message handler factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public RoutingConfiguration SetMessageHandlerFactory(Func<IEnumerable<DelegatingHandler>> factory)
        {
            _MessageHandlerFactory = factory;
            return this;
        }

        /// <summary>
        /// Sets the resource factory.
        /// </summary>
        /// <param name="getInstance">The get instance.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public RoutingConfiguration SetResourceFactory(Func<Type, InstanceContext, HttpRequestMessage, Object> getInstance)
        {
            _ResourceFactory = getInstance;
            return this;
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