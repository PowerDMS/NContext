// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2AuthorizationCodeAuthorizationRequest.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Defines a class which can be used to help create an OAuth 2.0 authorization request 
//   following the authorization code protocol flow.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace NContext.Application.Services.Authorization
{
    /// <summary>
    /// Defines an OAuth 2.0 service authorization request for authorization code flow.
    /// </summary>
    /// <remarks><para>
    /// Defines a class which can be used to help create an OAuth 2.0 authorization request following
    /// the authorization code protocol flow.
    /// </para></remarks>
    public sealed class OAuth2AuthorizationCodeAuthorizationRequest : IDisposable
    {
        #region Fields

        private readonly String _RequestUri;

        #endregion

        #region Constructors

        public OAuth2AuthorizationCodeAuthorizationRequest(Uri requestUri, String clientId, Uri redirectUri, IEnumerable<String> scopes, String state)
            : this(requestUri.ToString(), clientId, redirectUri.ToString(), String.Join(",", scopes), state)
        {
        }

        public OAuth2AuthorizationCodeAuthorizationRequest(String requestUri, String clientId, String redirectUri, IEnumerable<String> scopes, String state)
            : this(requestUri, clientId, redirectUri, String.Join(",", scopes), state)
        {
        }

        private OAuth2AuthorizationCodeAuthorizationRequest(String requestUri, String clientId, String redirectUri, String scope, String state)
        {
            _RequestUri = requestUri;
            ClientId = clientId;
            RedirectUri = redirectUri;
            Scope = scope;
            State = state;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <remarks></remarks>
        public String ClientId { get; private set; }

        /// <summary>
        /// Gets the redirect URI.
        /// </summary>
        /// <remarks></remarks>
        public String RedirectUri { get; private set; }

        /// <summary>
        /// Gets the type of the response.
        /// </summary>
        /// <remarks></remarks>
        public String ResponseType
        {
            get
            {
                return "code";
            }
        }

        /// <summary>
        /// Gets the comma-delimited scopes.
        /// </summary>
        /// <remarks></remarks>
        public String Scope { get; private set; }

        /// <summary>
        /// Gets the the exact value received from the client.
        /// </summary>
        /// <remarks></remarks>
        public String State { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override String ToString()
        {
            var requestBuilder =
                new StringBuilder(
                    String.Format(
                        "{0}?response_type={1}&client_id={2}&redirect_uri={3}",
                        _RequestUri,
                        HttpUtility.UrlEncode(ResponseType),
                        HttpUtility.UrlEncode(ClientId), 
                        HttpUtility.UrlEncode(RedirectUri)));

            if (!String.IsNullOrWhiteSpace(Scope))
            {
                requestBuilder.Append(String.Format("&scope={0}", HttpUtility.UrlEncode(Scope)));
            }

            if (!String.IsNullOrWhiteSpace(State))
            {
                requestBuilder.Append(String.Format("&state={0}", HttpUtility.UrlEncode(State)));
            }

            return requestBuilder.ToString();
        }

        #endregion

        #region Implementation of IDisposable

        private Boolean _IsDisposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="OAuth2AuthorizationCodeAuthorizationRequest"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks></remarks>
        ~OAuth2AuthorizationCodeAuthorizationRequest()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(Boolean disposeManagedResources)
        {
            if (_IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
                // Add custom dispose logic.
            }

            _IsDisposed = true;
        }

        #endregion
    }
}