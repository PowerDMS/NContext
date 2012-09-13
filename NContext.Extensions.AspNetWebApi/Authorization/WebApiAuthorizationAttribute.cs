// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiAuthorizationAttribute.cs" company="Waking Venture, Inc.">
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