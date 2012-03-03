// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizationActionFilter.cs">
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
//
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NContext.Extensions.WebApi.Authorization
{
    /// <summary>
    /// Defines an <see cref="ActionFilterAttribute"/> for resource operation authorization.
    /// </summary>
    public class AuthorizationActionFilter : ActionFilterAttribute
    {
        #region Fields

        private readonly IEnumerable<IProvideResourceAuthorization> _AuthorizationProviders;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationActionFilter"/> class.
        /// </summary>
        /// <param name="authorizationProviders">The authorization providers.</param>
        /// <remarks></remarks>
        public AuthorizationActionFilter(IEnumerable<IProvideResourceAuthorization> authorizationProviders)
        {
            _AuthorizationProviders = authorizationProviders ?? Enumerable.Empty<IProvideResourceAuthorization>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Authorizes the current request using the providers injected via constructor.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void AuthorizeRequest(HttpActionContext actionContext)
        {
            var currentPrincipal = Thread.CurrentPrincipal;
            if (!currentPrincipal.Identity.IsAuthenticated)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            if (_AuthorizationProviders.Any(provider => !provider.Authorize(currentPrincipal, actionContext.ActionDescriptor)))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }

        #endregion
        
        #region Overrides of ActionFilterAttribute

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            AuthorizeRequest(actionContext);

            base.OnActionExecuting(actionContext);
        }

        #endregion
    }
}