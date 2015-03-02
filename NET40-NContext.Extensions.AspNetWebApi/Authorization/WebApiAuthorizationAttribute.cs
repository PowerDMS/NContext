namespace NContext.Extensions.AspNetWebApi.Authorization
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Defines an <see cref="AuthorizationFilterAttribute"/> for resource authorization.
    /// </summary>
    public class WebApiAuthorizationAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Authorizes the current request using the providers injected via constructor.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="actionContext">The action context.</param>
        protected virtual void AuthorizeRequest(IPrincipal principal, HttpActionContext actionContext)
        {
            var unauthorizedResponseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            if (principal == null)
            {
                actionContext.Response = unauthorizedResponseMessage;
                return;
            }

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = unauthorizedResponseMessage;
            }
        }

        /// <summary>
        /// Called when [authorization].
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <remarks></remarks>
        public sealed override void OnAuthorization(HttpActionContext actionContext)
        {
            AuthorizeRequest(GetPrincipal(actionContext.Request), actionContext);
        }

        protected virtual IPrincipal GetPrincipal(HttpRequestMessage requestMessage)
        {
            return Thread.CurrentPrincipal ?? new GenericPrincipal(new GenericIdentity(String.Empty), null);
        }
    }
}