// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationOperationHandler.cs">
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
//   Defines an HttpOperationHandler for authentication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;

using Microsoft.ApplicationServer.Http.Dispatcher;

namespace NContext.Extensions.WCF.WebApi.Authentication
{
    /// <summary>
    /// Defines an <see cref="HttpOperationHandler"/> for authentication.
    /// </summary>
    public class AuthenticationOperationHandler : HttpOperationHandler<HttpRequestMessage, HttpRequestMessage>
    {
        #region Constructors

        public AuthenticationOperationHandler(String outputParameterName = "requestMessage")
            : base(outputParameterName)
        {
        }

        #endregion

        #region Overrides of HttpOperationHandler<HttpRequestMessage,HttpRequestMessage>

        /// <summary>
        /// Implemented in a derived class to provide the handling logic of the custom <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/>.
        /// </summary>
        /// <param name="input">The input value that the <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/> should handle. 
        ///             </param>
        /// <returns>
        /// The output value returned.
        /// </returns>
        protected override HttpRequestMessage OnHandle(HttpRequestMessage input)
        {
            Enumerable.Empty<IProvideResourceAuthentication>()
                          .FirstOrDefault(p => p.CanAuthenticate(input)).ToMaybe()
                          .Bind(provider => provider.Authenticate(input).ToMaybe())
                          .Let(principal =>
                              {
                                  Thread.CurrentPrincipal = principal;
                              });

            return input;
        }

        #endregion
    }
}