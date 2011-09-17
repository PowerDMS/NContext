// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2AuthorizationCodeAccessTokenRequest.cs">
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
//   Defines a class which can be used to help create an OAuth 2.0 access token 
//   request following the authorization code protocol flow.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace NContext.Application.Services.Authorization
{
    /// <summary>
    /// Defines a class which can be used to help create an OAuth 2.0 access token 
    /// request following the authorization code protocol flow.
    /// </summary>
    /// <remarks><para>
    /// </para></remarks>
    [DataContract]
    public sealed class OAuth2AuthorizationCodeAccessTokenRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2AuthorizationCodeAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="code">The authorization code received from the authorization server.</param>
        /// <param name="redirectUri">The redirection URI used by the authorization server to return the authorization response in the previous step.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <remarks></remarks>
        public OAuth2AuthorizationCodeAccessTokenRequest(String code, Uri redirectUri, String clientId, String clientSecret)
            : this(code, redirectUri.ToString(), clientId, clientSecret)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2AuthorizationCodeAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="code">The authorization code received from the authorization server.</param>
        /// <param name="redirectUri">The redirection URI used by the authorization server to return the authorization response in the previous step.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <remarks></remarks>
        public OAuth2AuthorizationCodeAccessTokenRequest(String code, String redirectUri, String clientId, String clientSecret)
        {
            Code = code;
            RedirectUri = redirectUri;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the grant.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "grant_type", IsRequired = true)]
        public String GrantType
        {
            get
            {
                return "authorization_code";
            }
        }

        /// <summary>
        /// Gets the authorization code received from the authorization server.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "code", IsRequired = true)]
        public String Code { get; private set; }

        /// <summary>
        /// Gets the redirection URI used by the authorization server to return the authorization response in the previous step.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "redirect_uri", IsRequired = true)]
        public String RedirectUri { get; private set; }

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "client_id")]
        public String ClientId { get; private set; }

        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "client_secret")]
        public String ClientSecret { get; private set; }

        #endregion
    }
}