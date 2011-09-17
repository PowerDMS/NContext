// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2AccessTokenError.cs">
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
//   Defines a class which represents an OAuth 2.0 Access Token request error.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

using NContext.Application.Utilities;

namespace NContext.Application.Services.Authorization
{
    /// <summary>
    /// Defines a class which represents an OAuth 2.0 Access Token request error.
    /// </summary>
    /// <remarks><para>
    /// </para></remarks>
    [Serializable]
    public sealed class OAuth2AccessTokenError : ISerializable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2AccessTokenError"/> class.
        /// </summary>
        /// <param name="error">The <see cref="AccessTokenError"/>.</param>
        /// <param name="description">The error description.</param>
        /// <param name="uri">The error URI.</param>
        /// <remarks></remarks>
        public OAuth2AccessTokenError(AccessTokenError error, String description, String uri)
        {
            Error = error;
            Description = description;
            Uri = uri;
        }

        private OAuth2AccessTokenError(SerializationInfo info, StreamingContext context)
        {
            Error = AttributeUtility.GetEnumValueFromDescriptionAttributeValue<AccessTokenError>((String)info.GetValue("Error", typeof(String)));
            Description = (String)info.GetValue("Description", typeof(String));
            Uri = (String)info.GetValue("Uri", typeof(String));
        }

        #endregion

        #region Enums

        /// <summary>
        /// A single error code which represents why the request failed.
        /// </summary>
        /// <remarks></remarks>
        public enum AccessTokenError
        {
            /// <summary>
            /// Client authentication failed (e.g. unknown client, no client credentials included, multiple 
            /// client credentials included, or unsupported credentials type).  The authorization server 
            /// MAY return an HTTP 401 (Unauthorized) status code to indicate which HTTP authentication 
            /// schemes are supported.  If the client attempted to authenticate via the "Authorization" 
            /// request header field, the authorization server MUST respond with an HTTP 401 (Unauthorized) 
            /// status code, and include the "WWW-Authenticate" response header field matching the 
            /// authentication scheme used by the client.
            /// </summary>
            [Description("invalid_client")]
            InvalidClient,

            /// <summary>
            /// The request is missing a required parameter, includes an unsupported parameter 
            /// or parameter value, repeats a parameter, includes multiple credentials, 
            /// utilizes more than one mechanism for authenticating the client, or is otherwise malformed.
            /// </summary>
            [Description("invalid_request")]
            InvalidRequest,

            /// <summary>
            /// The provided authorization grant is invalid, expired, revoked, does not match the redirection 
            /// URI used in the authorization request, or was issued to another client.
            /// </summary>
            [Description("invalid_grant")]
            InvalidGrant,

            /// <summary>
            /// The requested scope is invalid, unknown, malformed, or exceeds the scope granted by the resource owner.
            /// </summary>
            [Description("invalid_scope")]
            InvalidScope,

            /// <summary>
            /// The authenticated client is not authorized to use this authorization grant type.
            /// </summary>
            [Description("unauthorized_client")]
            UnauthorizedClient,

            /// <summary>
            /// The authorization grant type is not supported by the authorization server.
            /// </summary>
            [Description("unsupported_grant_type")]
            UnsupportedGrantType
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error which represents why the request failed.
        /// </summary>
        /// <remarks></remarks>
        public AccessTokenError Error { get; private set; }

        /// <summary>
        /// Gets the human-readable text providing additional information, used to assist in the 
        /// understanding and resolution of the error occurred.
        /// </summary>
        /// <remarks></remarks>
        public String Description { get; private set; }

        /// <summary>
        /// Gets the URI identifying a human-readable web page with information about the error, 
        /// used to provide the resource owner with additional information about the error.
        /// </summary>
        /// <remarks></remarks>
        public String Uri { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. 
        ///                 </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. 
        ///                 </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. 
        /// </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Error", AttributeUtility.GetDescriptionAttributeValueFromField(Error));
            info.AddValue("Description", Description);
            info.AddValue("Uri", Uri);
        }

        #endregion
    }
}