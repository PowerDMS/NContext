// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2AuthorizationCodeAuthorizationError.cs">
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
//   Defines a class which can be used to help create an OAuth 2.0 authorization error 
//   for an authorization code request as defined by the specification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Text;
using System.Web;

using NContext.Application.Utilities;

namespace NContext.Application.Services.Authorization
{
    /// <summary>
    /// Defines a class which can be used to help create an OAuth 2.0 authorization error 
    /// for an authorization code request as defined by the specification.
    /// </summary>
    /// <remarks><para>
    /// This class should only be used to help create the error uri.
    /// </para></remarks>
    public class OAuth2AuthorizationCodeAuthorizationError
    {
        #region Fields

        private readonly string _RequestUri;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2AuthorizationCodeAuthorizationError"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="error">The error.</param>
        /// <param name="errorDescription">The error description.</param>
        /// <param name="errorUri">The error URI.</param>
        /// <param name="state">The state.</param>
        /// <remarks></remarks>
        public OAuth2AuthorizationCodeAuthorizationError(Uri requestUri, AuthorizationCodeError error, String errorDescription, String errorUri, String state)
            : this(requestUri.ToString(), error, errorDescription, errorUri, state)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2AuthorizationCodeAuthorizationError"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="error">The error.</param>
        /// <param name="errorDescription">The error description.</param>
        /// <param name="errorUri">The error URI.</param>
        /// <param name="state">The state.</param>
        /// <remarks></remarks>
        public OAuth2AuthorizationCodeAuthorizationError(String requestUri, AuthorizationCodeError error, String errorDescription, String errorUri, String state)
        {
            _RequestUri = requestUri;
            Error = error;
            ErrorDescription = errorDescription;
            ErrorUri = errorUri;
            State = state;
        }

        #endregion

        #region Enums

        /// <summary>
        /// A single error code representing why the authorization request failed.
        /// </summary>
        /// <remarks></remarks>
        public enum AuthorizationCodeError
        {
            /// <summary>
            /// The request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.
            /// </summary>
            [Description("invalid_request")]
            InvalidRequest,

            /// <summary>
            /// The client is not authorized to request an authorization code using this method.
            /// </summary>
            [Description("unauthorized_client")]
            UnauthorizedClient,

            /// <summary>
            /// The resource owner or authorization server denied the request.
            /// </summary>
            [Description("access_denied")]
            AccessDenied,

            /// <summary>
            /// The authorization server does not support obtaining an authorization code using this method.
            /// </summary>
            [Description("unsupported_response_type")]
            UnsupportedResponseType,

            /// <summary>
            /// The requested scope is invalid, unknown, or malformed. A 4xx or 5xx HTTP status code (except for 400 and 401)
            /// The authorization server MAY set the "error" parameter value to a numerical HTTP status code from the 4xx or 5xx
            /// range, with the exception of the 400 (Bad Request) and 401 (Unauthorized) status codes.  For example, if the
            /// service is temporarily unavailable, the authorization server MAY return an error response with "error" set to 
            /// "503".
            /// </summary>
            [Description("invalid_scope")]
            InvalidScope
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <remarks></remarks>
        public AuthorizationCodeError Error { get; private set; }

        /// <summary>
        /// Gets a human-readable text providing additional information, used to assist in 
        /// the understanding and resolution of the error occurred. [[ add language and encoding information]]
        /// </summary>
        /// <remarks></remarks>
        public String ErrorDescription { get; private set; }

        /// <summary>
        /// Gets a URI identifying a human-readable web page with information about the error, used to provide 
        /// the resource owner with additional information about the error.
        /// </summary>
        /// <remarks></remarks>
        public String ErrorUri { get; private set; }

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
        public override string ToString()
        {
            var requestBuilder = new StringBuilder(String.Format("{0}?error={1}", _RequestUri, HttpUtility.UrlEncode(AttributeUtility.GetDescriptionAttributeValueFromField(Error))));
            if (!String.IsNullOrWhiteSpace(ErrorDescription))
            {
                requestBuilder.Append(String.Format("&error_description={0}", HttpUtility.UrlEncode(ErrorDescription)));
            }

            if (!String.IsNullOrWhiteSpace(ErrorUri))
            {
                requestBuilder.Append(String.Format("&error_uri={0}", HttpUtility.UrlEncode(ErrorUri)));
            }

            if (!String.IsNullOrWhiteSpace(State))
            {
                requestBuilder.Append(String.Format("&state={0}", HttpUtility.UrlEncode(State)));
            }

            return requestBuilder.ToString();
        }

        #endregion
    }
}