// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WcfWebApiConfiguration.cs">
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
//   Defines a fluent configuration interface for WCF WebApi.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Xml.Serialization;

using Microsoft.ApplicationServer.Http;

using NContext.Application.Configuration;
using NContext.Application.Extensions;

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// Defines a fluent configuration interface for WCF WebApi.
    /// </summary>
    public class WcfWebApiConfiguration : RoutingConfigurationBase
    {
        #region Fields

        private Boolean _EnableTestClient;

        private Boolean _EnableHelpPage;

        private Boolean _ClearDefaultFormatters;

        private IEnumerable<MediaTypeFormatter> _MediaTypeFormatters;

        private Func<IEnumerable<DelegatingHandler>> _MessageHandlerFactory;

        private Func<Type, InstanceContext, HttpRequestMessage, Object> _ResourceFactory;

        #endregion

        #region Constructors

        public WcfWebApiConfiguration(
            ApplicationConfigurationBuilder applicationConfigurationBuilder,
            RoutingConfigurationBuilder routingConfigurationBuilder)
            : base(applicationConfigurationBuilder, routingConfigurationBuilder)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets <see cref="HttpConfiguration.EnableTestClient"/> to the value specified.
        /// </summary>
        /// <param name="enableTestClient">if set to <c>true</c> [enable test client].</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WcfWebApiConfiguration SetEnableTestClient(Boolean enableTestClient)
        {
            _EnableTestClient = enableTestClient;
            return this;
        }

        /// <summary>
        /// Sets <see cref="HttpConfiguration.EnableHelpPage"/> to the value specified.
        /// </summary>
        /// <param name="enableHelpPage">if set to <c>true</c> [enable help page].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public WcfWebApiConfiguration SetEnableHelpPage(Boolean enableHelpPage)
        {
            _EnableHelpPage = enableHelpPage;
            return this;
        }

        /// <summary>
        /// Sets <see cref="HttpConfiguration.Formatters"/> to the params specified. Optionally 
        /// clears the default <see cref="MediaTypeFormatter"/>s.
        /// </summary>
        /// <param name="clearDefault">if set to <c>true</c> clears the default formatters 
        /// (ie. <see cref="XmlSerializer"/>, <see cref="DataContractJsonSerializer"/>).</param>
        /// <param name="mediaTypeFormatters">The media type formatters.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WcfWebApiConfiguration SetFormatters(Boolean clearDefault = false, params MediaTypeFormatter[] mediaTypeFormatters)
        {
            _ClearDefaultFormatters = clearDefault;
            _MediaTypeFormatters = mediaTypeFormatters;
            return this;
        }

        /// <summary>
        /// Sets <see cref="HttpConfiguration.MessageHandlerFactory"/> to the value specified.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WcfWebApiConfiguration SetMessageHandlerFactory(Func<IEnumerable<DelegatingHandler>> factory)
        {
            _MessageHandlerFactory = factory;
            return this;
        }

        /// <summary>
        /// Sets <see cref="HttpConfiguration.OperationHandlerFactory"/> to the value specified.
        /// </summary>
        /// <param name="getInstance">The get instance.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WcfWebApiConfiguration SetOperationHandlerFactory(Func<Type, InstanceContext, HttpRequestMessage, Object> getInstance)
        {
            _ResourceFactory = getInstance;
            return this;
        }

        /// <summary>
        /// Setup the instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Func<HttpConfiguration> httpConfigurationFactory = (CreateHttpConfiguration);
            RoutingConfigurationBuilder.RoutingConfiguration.SetHttpConfigurationFactory(httpConfigurationFactory);
        }

        private HttpConfiguration CreateHttpConfiguration()
        {
            var httpConfiguration = new WebApiConfiguration
                {
                    EnableTestClient = _EnableTestClient,
                    CreateInstance = _ResourceFactory,
                    MessageHandlerFactory = _MessageHandlerFactory,
                    EnableHelpPage = _EnableHelpPage
                };

            if (_ClearDefaultFormatters)
            {
                httpConfiguration.Formatters.Clear();
            }

            if (_MediaTypeFormatters.Any())
            {
                _MediaTypeFormatters.Reverse().ForEach(formatter => httpConfiguration.Formatters.Insert(0, formatter));
            }

            return httpConfiguration;
        }

        #endregion
    }
}