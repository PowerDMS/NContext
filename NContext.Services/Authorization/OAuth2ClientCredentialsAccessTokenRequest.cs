// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2ClientCredentialsAccessTokenRequest.cs">
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
//   Defines a class which can be used to help create an OAuth 2.0 access token 
//   request following the client credentials protocol flow.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace NContext.Application.Services.Authorization
{
    /// <summary>
    /// Defines a class which can be used to help create an OAuth 2.0 access token 
    /// request following the client credentials protocol flow.
    /// </summary>
    /// <remarks> 
    /// OAuth 2.0 Specification example:
    /// <para>
    /// POST /token HTTP/1.1
    /// Host: server.example.com
    /// Authorization: BASIC czZCaGRSa3F0MzpnWDFmQmF0M2JW
    /// Content-Type: application/x-www-form-urlencoded
    /// </para><para>
    /// grant_type=client_credentials&amp;client_id=s6BhdRkqt3&amp;
    /// </para></remarks>
    public sealed class OAuth2ClientCredentialsAccessTokenRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ClientCredentialsAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="scopes">The scopes.</param>
        /// <remarks></remarks>
        public OAuth2ClientCredentialsAccessTokenRequest(String clientId, String clientSecret, IEnumerable<String> scopes)
            : this(clientId, clientSecret, String.Join(",", scopes))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ClientCredentialsAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="scope">The scope.</param>
        /// <remarks></remarks>
        public OAuth2ClientCredentialsAccessTokenRequest(String clientId, String clientSecret, String scope)
        {
            Scope = scope;
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
                return "client_credentials";
            }
        }

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "client_id", IsRequired = true)]
        public String ClientId { get; private set; }

        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "client_secret", IsRequired = true)]
        public String ClientSecret { get; private set; }

        /// <summary>
        /// Gets the comma-delimited scopes.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "scope", IsRequired = true)]
        public String Scope { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a <see cref="WebRequest"/> with the appropriate request parameters and headers.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>The pre-configured <see cref="WebRequest"/>.</returns>
        /// <remarks></remarks>
        public WebRequest CreateRequest(String requestUri)
        {
            return CreateRequest(new Uri(requestUri));
        }
        
        /// <summary>
        /// Creates a <see cref="WebRequest"/> with the appropriate request parameters and headers.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>The pre-configured <see cref="WebRequest"/>.</returns>
        /// <remarks></remarks>
        public WebRequest CreateRequest(Uri requestUri)
        {
            Byte[] body = Encoding.UTF8.GetBytes(GetRequestContent());
            var credentialCache = new CredentialCache
                {
                    {
                        requestUri, "Basic", new NetworkCredential(ClientId, ClientSecret)
                    }
                };

            var tokenRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.ContentLength = body.Length;
            tokenRequest.Credentials = credentialCache;
            tokenRequest.Headers[HttpRequestHeader.Authorization] = String.Format("BASIC {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", ClientId, ClientSecret))));
            using (var stream = tokenRequest.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
            }

            return tokenRequest;
        }

        private String GetRequestContent()
        {
            var requestBuilder =
                new StringBuilder(
                    String.Format(
                        "grant_type={0}&client_id={1}",
                        HttpUtility.UrlEncode(GrantType),
                        HttpUtility.UrlEncode(ClientId)));

            if (!String.IsNullOrWhiteSpace(Scope))
            {
                requestBuilder.Append(String.Format("&scope={0}", HttpUtility.UrlEncode(Scope)));
            }

            return requestBuilder.ToString();
        }

        #endregion
    }
}