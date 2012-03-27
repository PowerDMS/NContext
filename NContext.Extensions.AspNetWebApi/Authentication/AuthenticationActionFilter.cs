// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationActionFilter.cs">
//   Copyright (c) 2012
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
//   Defines an HttpOperationHandler for authentication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NContext.Extensions.AspNetWebApi.Authentication
{
    /// <summary>
    /// Defines an <see cref="ActionFilterAttribute"/> for authentication.
    /// </summary>
    public class AuthenticationActionFilter : ActionFilterAttribute
    {
        private readonly IEnumerable<IProvideResourceAuthentication> _AuthenticationProviders;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationActionFilter"/> class.
        /// </summary>
        /// <param name="authenticationProviders">The authentication providers.</param>
        /// <remarks></remarks>
        public AuthenticationActionFilter(IEnumerable<IProvideResourceAuthentication> authenticationProviders)
        {
            _AuthenticationProviders = authenticationProviders ?? Enumerable.Empty<IProvideResourceAuthentication>();
        }

        #endregion

        #region Overrides of ActionFilterAttribute

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <remarks></remarks>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var input = actionContext.Request;

            _AuthenticationProviders
                .FirstOrDefault(p => p.CanAuthenticate(input)).ToMaybe()
                .Bind(provider => provider.Authenticate(input).ToMaybe())
                .Let(principal =>
                    {
                        Thread.CurrentPrincipal = principal;
                    });

            base.OnActionExecuting(actionContext);
        }

        #endregion
    }
}