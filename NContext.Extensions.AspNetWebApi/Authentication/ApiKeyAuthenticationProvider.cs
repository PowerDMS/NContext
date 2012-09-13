// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiKeyAuthenticationProvider.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AspNetWebApi.Authentication
{
    using System;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Web;

    /// <summary>
    /// Defines a skeleton abstraction for API key authentication.
    /// </summary>
    /// <remarks></remarks>
    public abstract class ApiKeyAuthenticationProvider : IProvideRequestAuthentication
    {
        private readonly String _AuthorizationHeaderScheme;

        private readonly String _QueryStringKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyAuthenticationProvider"/> class.
        /// </summary>
        /// <param name="authorizationHeaderScheme">The authorization header scheme.</param>
        /// <param name="queryStringKey">The query string key.</param>
        /// <remarks></remarks>
        protected ApiKeyAuthenticationProvider(String authorizationHeaderScheme = "apikey", String queryStringKey = "apikey")
        {
            _AuthorizationHeaderScheme = authorizationHeaderScheme;
            _QueryStringKey = queryStringKey;
        }

        /// <summary>
        /// Authenticates the specified request message.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <returns>Instance of <see cref="IPrincipal"/>.</returns>
        /// <remarks></remarks>
        public virtual IPrincipal Authenticate(HttpRequestMessage requestMessage)
        {
            if (!CanAuthenticate(requestMessage))
            {
                return null;
            }

            if (requestMessage.Headers.Authorization != null && 
                !String.IsNullOrWhiteSpace(_AuthorizationHeaderScheme) &&
                requestMessage.Headers.Authorization.Scheme.Equals(_AuthorizationHeaderScheme, StringComparison.InvariantCultureIgnoreCase))
            {
                return AuthenticateApiKey(requestMessage.Headers.Authorization.Parameter);
            }

            if (_QueryStringKey != null && HttpUtility.ParseQueryString(requestMessage.RequestUri.Query)[_QueryStringKey] != null)
            {
                return AuthenticateApiKey(HttpUtility.ParseQueryString(requestMessage.RequestUri.Query).Get(_QueryStringKey));
            }

            return null;
        }

        /// <summary>
        /// Determines whether this instance can authenticate the specified request message.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <returns><c>true</c> if this instance can authenticate the specified request message; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public virtual Boolean CanAuthenticate(HttpRequestMessage requestMessage)
        {
            if (requestMessage.Headers.Authorization != null && 
                !String.IsNullOrWhiteSpace(_AuthorizationHeaderScheme) && 
                requestMessage.Headers.Authorization.Scheme.Equals(_AuthorizationHeaderScheme, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            if (!String.IsNullOrWhiteSpace(_QueryStringKey) && HttpUtility.ParseQueryString(requestMessage.RequestUri.Query)[_QueryStringKey] != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Authenticates the API key.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>IPrincipal.</returns>
        public abstract IPrincipal AuthenticateApiKey(String apiKey);
    }
}