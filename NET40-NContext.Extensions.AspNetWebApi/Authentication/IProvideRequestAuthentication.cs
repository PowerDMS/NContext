namespace NContext.Extensions.AspNetWebApi.Authentication
{
    using System;
    using System.Net.Http;
    using System.Security.Principal;
    using NContext.Common;

    /// <summary>
    /// Defines a provider role for request authentication.
    /// </summary>
    public interface IProvideRequestAuthentication
    {
        /// <summary>
        /// Determines whether this instance can authenticate the specified request message.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <returns><c>true</c> if this instance can authenticate the specified request message; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        Boolean CanAuthenticate(HttpRequestMessage requestMessage);

        /// <summary>
        /// Authenticates the specified request message.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <returns>Instance of <see cref="IPrincipal"/>.</returns>
        /// <remarks></remarks>
        IServiceResponse<IPrincipal> Authenticate(HttpRequestMessage requestMessage);
    }
}