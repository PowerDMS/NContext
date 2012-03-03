// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AcceptLanguageActionFilter.cs">
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
//   Defines an operation handler for supporting the HTTP AcceptLanguage header.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NContext.Extensions.WebApi
{
    /// <summary>
    /// Defines an <see cref="ActionFilterAttribute"/> for supporting the HTTP Accept-language header.
    /// </summary>
    public class AcceptLanguageActionFilter : ActionFilterAttribute
    {
        #region Overrides of ActionFilterAttribute

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <remarks></remarks>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.Headers.AcceptLanguage == null)
            {
                return;
            }

            var languages = request.Headers.AcceptLanguage.OrderByDescending(language => language.Quality ?? 1);
            foreach (var language in languages)
            {
                try
                {
                    var culture = CultureInfo.GetCultureInfo(language.Value);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                    break;
                }
                catch (CultureNotFoundException)
                {
                }
            }

            base.OnActionExecuting(actionContext);
        }

        #endregion
    }
}