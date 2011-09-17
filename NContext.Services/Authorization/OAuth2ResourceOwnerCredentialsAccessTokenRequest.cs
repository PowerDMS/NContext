// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2ResourceOwnerCredentialsAccessTokenRequest.cs">
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
//   request following the resource owner credentials protocol flow.
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
    /// request following the resource owner credentials protocol flow.
    /// </summary>
    /// <remarks> 
    /// OAuth 2.0 Specification example:
    /// <para>
    /// POST /token HTTP/1.1
    /// Host: server.example.com
    /// Authorization: BASIC czZCaGRSa3F0MzpnWDFmQmF0M2JW
    /// Content-Type: application/x-www-form-urlencoded
    /// </para><para>
    /// grant_type=password&amp;client_id=s6BhdRkqt3&amp;username=johndoe&amp;password=A3ddj3w
    /// </para></remarks>
    public sealed class OAuth2ResourceOwnerCredentialsAccessTokenRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ResourceOwnerCredentialsAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <remarks></remarks>
        public OAuth2ResourceOwnerCredentialsAccessTokenRequest(String userName, String password, IEnumerable<String> scopes, String clientId, String clientSecret)
            : this(userName, password, String.Join(",", scopes), clientId, clientSecret)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ResourceOwnerCredentialsAccessTokenRequest"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <remarks></remarks>
        public OAuth2ResourceOwnerCredentialsAccessTokenRequest(String userName, String password, String scope, String clientId, String clientSecret)
        {
            UserName = userName;
            Password = password;
            Scope = scope;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the client id.
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
        /// Gets the type of the grant.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "grant_type", IsRequired = true)]
        public String GrantType
        {
            get
            {
                return "password";
            }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "password", IsRequired = true)]
        public String Password { get; private set; }

        /// <summary>
        /// Gets the comma-delimited scopes.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "scope", IsRequired = true)]
        public String Scope { get; private set; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(Name = "username", IsRequired = true)]
        public String UserName { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a <see cref="WebRequest"/> with the appropriate request parameters and headers.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>The pre-configured <see cref="WebRequest"/>.</returns>
        /// <remarks></remarks>
        public HttpWebRequest CreateRequest(String requestUri)
        {
            return CreateRequest(new Uri(requestUri));
        }
        
        /// <summary>
        /// Creates a <see cref="WebRequest"/> with the appropriate request parameters and headers.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>The pre-configured <see cref="WebRequest"/>.</returns>
        /// <remarks></remarks>
        public HttpWebRequest CreateRequest(Uri requestUri)
        {
            Byte[] body = Encoding.UTF8.GetBytes(GetRequestContent());
            var credentialCache = new CredentialCache
                {
                    {
                        requestUri, "Basic", new NetworkCredential(ClientId, Password)
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
                        "grant_type={0}&client_id={1}&username={2}&password={3}",
                        HttpUtility.UrlEncode(GrantType),
                        HttpUtility.UrlEncode(ClientId),
                        HttpUtility.UrlEncode(UserName),
                        HttpUtility.UrlEncode(Password)));

            if (!String.IsNullOrWhiteSpace(Scope))
            {
                requestBuilder.Append(String.Format("&scope={0}", HttpUtility.UrlEncode(Scope)));
            }

            return requestBuilder.ToString();
        }

        #endregion
    }
}