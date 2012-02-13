// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiRoutingConfiguration.cs">
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
//   Defines a fluent configuration interface for WCF WebApi.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml.Serialization;

using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.ApplicationServer.Http.Dispatcher;

using NContext.Configuration;
using NContext.Extensions.WCF.Routing;

namespace NContext.Extensions.WCF.WebApi.Routing
{
    /// <summary>
    /// Defines a fluent configuration interface for WCF WebApi.
    /// </summary>
    public class WebApiRoutingConfiguration : RoutingConfigurationBase
    {
        #region Fields

        private Boolean _EnableTestClient;

        private Boolean _EnableHelpPage;

        private Boolean _ClearDefaultFormatters;

        private IEnumerable<MediaTypeFormatter> _MediaTypeFormatters;

        private Func<IEnumerable<DelegatingHandler>> _MessageHandlerFactory;

        private Func<Type, InstanceContext, HttpRequestMessage, Object> _ResourceInstanceFactory;

        private HttpErrorHandler[] _HttpErrorHandlers;

        private IEnumerable<Type> _MessageHandlers;

        private TrailingSlashMode? _TrailingSlashMode;

        private Action<Collection<HttpOperationHandler>, ServiceEndpoint, HttpOperationDescription> _RequestHandlers;

        private Action<Collection<HttpOperationHandler>, ServiceEndpoint, HttpOperationDescription> _ResponseHandlers;

        private Action<Uri, HttpBindingSecurity> _Security;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiRoutingConfiguration"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration builder.</param>
        /// <param name="routingConfigurationBuilder">The routing configuration builder.</param>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration(
            ApplicationConfigurationBuilder applicationConfigurationBuilder,
            RoutingConfigurationBuilder routingConfigurationBuilder)
            : base(applicationConfigurationBuilder, routingConfigurationBuilder)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets <see cref="HttpConfiguration.CreateInstance"/> to the value specified which in turn 
        /// is used to resolve a service instance for the incoming request.
        /// </summary>
        /// <param name="instanceFactory">The function to resolve the appropriate service instance.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetCreateInstanceFactory(Func<Type, InstanceContext, HttpRequestMessage, Object> instanceFactory)
        {
            _ResourceInstanceFactory = instanceFactory;

            return this;
        }

        /// <summary>
        /// Sets <see cref="HttpConfiguration.EnableHelpPage"/> to the value specified.
        /// </summary>
        /// <param name="enableHelpPage">if set to <c>true</c> [enable help page].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetEnableHelpPage(Boolean enableHelpPage)
        {
            _EnableHelpPage = enableHelpPage;

            return this;
        }

        /// <summary>
        /// Sets <see cref="HttpConfiguration.EnableTestClient"/> to the value specified.
        /// </summary>
        /// <param name="enableTestClient">if set to <c>true</c> [enable test client].</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetEnableTestClient(Boolean enableTestClient)
        {
            _EnableTestClient = enableTestClient;

            return this;
        }

        /// <summary>
        /// Inserts the specified <see cref="MediaTypeFormatter"/>s to the <see cref="HttpConfiguration.Formatters"/> collection at the zeroth position. 
        /// If <paramref name="clearDefault"/> is set to <c>true</c>, the default <see cref="MediaTypeFormatter"/>s will be removed.
        /// </summary>
        /// <param name="clearDefault">if set to <c>true</c> clears the default formatters 
        /// (ie. <see cref="XmlSerializer"/>, <see cref="DataContractJsonSerializer"/>).</param>
        /// <param name="mediaTypeFormatters">The media type formatters.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetFormatters(Boolean clearDefault = false, params MediaTypeFormatter[] mediaTypeFormatters)
        {
            _ClearDefaultFormatters = clearDefault;
            _MediaTypeFormatters = mediaTypeFormatters;

            return this;
        }

        /// <summary>
        /// Sets the <see cref="HttpConfiguration.ErrorHandlers"/> collection.
        /// </summary>
        /// <param name="errorHandlers">The error handlers.</param>
        /// <returns>Current <see cref="WebApiRoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetErrorHandlers(params HttpErrorHandler[] errorHandlers)
        {
            _HttpErrorHandlers = errorHandlers;

            return this;
        }

        /// <summary>
        /// Sets <see cref="HttpConfiguration.MessageHandlerFactory"/> to the value specified.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>Current <see cref="RoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetMessageHandlerFactory(Func<IEnumerable<DelegatingHandler>> factory)
        {
            _MessageHandlerFactory = factory;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="HttpConfiguration.MessageHandlers"/> collection.
        /// </summary>
        /// <param name="messageHandlers">The message handler types.</param>
        /// <returns>Current <see cref="WebApiRoutingConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetMessageHandlers(params Type[] messageHandlers)
        {
            _MessageHandlers = messageHandlers;

            return this;
        }

        /// <summary>
        /// Sets the request operation handlers.
        /// </summary>
        /// <param name="requestHandlers">The request handlers.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetRequestHandlers(Action<Collection<HttpOperationHandler>, ServiceEndpoint, HttpOperationDescription> requestHandlers)
        {
            _RequestHandlers = requestHandlers;

            return this;
        }

        /// <summary>
        /// Sets the response operation handlers.
        /// </summary>
        /// <param name="responseHandlers">The response handlers.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetResponseHandlers(Action<Collection<HttpOperationHandler>, ServiceEndpoint, HttpOperationDescription> responseHandlers)
        {
            _ResponseHandlers = responseHandlers;

            return this;
        }

        /// <summary>
        /// Sets the WCF binding security.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetSecurity(Action<Uri, HttpBindingSecurity> security)
        {
            _Security = security;

            return this;
        }

        /// <summary>
        /// Sets the trailing slash mode.
        /// </summary>
        /// <param name="trailingSlashMode">The trailing slash mode.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public WebApiRoutingConfiguration SetTrailingSlashMode(TrailingSlashMode trailingSlashMode = TrailingSlashMode.Ignore)
        {
            _TrailingSlashMode = trailingSlashMode;

            return this;
        }

        /// <summary>
        /// Setup the instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            RoutingConfigurationBuilder.RoutingConfiguration.SetHttpConfigurationFactory(CreateHttpConfiguration);
        }

        protected virtual HttpConfiguration CreateHttpConfiguration()
        {
            var httpConfiguration = new WebApiConfiguration
                {
                    CreateInstance = _ResourceInstanceFactory,
                    EnableTestClient = _EnableTestClient,
                    EnableHelpPage = _EnableHelpPage,
                    MessageHandlerFactory = _MessageHandlerFactory,
                    RequestHandlers = _RequestHandlers,
                    ResponseHandlers = _ResponseHandlers,
                    Security = _Security,
                    TrailingSlashMode = _TrailingSlashMode ?? TrailingSlashMode.Ignore
                };

            if (_MediaTypeFormatters != null && _MediaTypeFormatters.Any())
            {
                if (_ClearDefaultFormatters)
                {
                    httpConfiguration.Formatters.Clear();
                }

                _MediaTypeFormatters.Reverse().ForEach(formatter => httpConfiguration.Formatters.Insert(0, formatter));
            }

            if (_HttpErrorHandlers != null && _HttpErrorHandlers.Any())
            {
                httpConfiguration.ErrorHandlers = (handlers, serviceEndpoint, descriptions) => _HttpErrorHandlers.ForEach(handlers.Add);
            }

            if (_MessageHandlers != null && _MessageHandlers.Any())
            {
                _MessageHandlers.ForEach(handler => httpConfiguration.MessageHandlers.Add(handler));
            }

            return httpConfiguration;
        }

        #endregion
    }
}