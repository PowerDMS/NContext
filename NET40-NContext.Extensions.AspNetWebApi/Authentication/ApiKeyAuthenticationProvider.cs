namespace NContext.Extensions.AspNetWebApi.Authentication
{
    using System;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Web;
    using NContext.Common;

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
        public virtual IServiceResponse<IPrincipal> Authenticate(HttpRequestMessage requestMessage)
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
        public abstract IServiceResponse<IPrincipal> AuthenticateApiKey(String apiKey);
    }
}