// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizationOperationHandler.cs">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

using Microsoft.ApplicationServer.Http.Description;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace NContext.Extensions.WCF.WebApi.Authorization
{
    /// <summary>
    /// Defines an <see cref="HttpOperationHandler"/> for resource operation authorization.
    /// </summary>
    public class AuthorizationOperationHandler : HttpOperationHandler
    {
        #region Fields

        private readonly HttpOperationDescription _OperationDescription;

        private readonly IEnumerable<IProvideResourceAuthorization> _AuthorizationProviders;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationOperationHandler"/> class.
        /// </summary>
        /// <param name="operationDescription">The operation description.</param>
        /// <param name="authorizationProviders">The authorization providers.</param>
        /// <remarks></remarks>
        public AuthorizationOperationHandler(HttpOperationDescription operationDescription, IEnumerable<IProvideResourceAuthorization> authorizationProviders)
        {
            if (operationDescription == null)
            {
                throw new ArgumentNullException("operationDescription");
            }

            _OperationDescription = operationDescription;
            _AuthorizationProviders = authorizationProviders ?? Enumerable.Empty<IProvideResourceAuthorization>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Authorizes the current request using the providers injected via constructor.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void AuthorizeRequest()
        {
            var currentPrincipal = Thread.CurrentPrincipal;
            if (!currentPrincipal.Identity.IsAuthenticated)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            if (_AuthorizationProviders.Any(provider => !provider.Authorize(currentPrincipal, _OperationDescription)))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }

        #endregion
        
        #region Overrides of HttpOperationHandler

        /// <summary>
        /// Implemented in a derived class to return the input <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see>
        ///             that the <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/> expects to be provided whenever the <see cref="M:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler.Handle(System.Object[])"/> method 
        ///             is called.  The <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see> must be returned in the same order
        ///             the the <see cref="M:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler.Handle(System.Object[])"/> method will expect them in the input object array.
        /// </summary>
        /// <remarks>
        /// <see cref="M:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler.OnGetInputParameters"/> is only called once and the <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see>
        ///             are cached in a read-only collection.
        /// </remarks>
        /// <returns>
        /// The input <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see> that the <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/>
        ///             expects.
        /// </returns>
        protected override IEnumerable<HttpParameter> OnGetInputParameters()
        {
            return _OperationDescription.InputParameters;
        }

        /// <summary>
        /// Implemented in a derived class to return the ouput <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see>
        ///             that the <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/> will provided whenever the <see cref="M:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler.Handle(System.Object[])"/> method 
        ///             is called.  The <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see> must be returned in the same order
        ///             the the <see cref="M:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler.Handle(System.Object[])"/> method will provide then in the output object array.
        /// </summary>
        /// <remarks>
        /// <see cref="M:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler.OnGetOutputParameters"/> is only called once and the <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see>
        ///             are cached in a read-only collection.
        /// </remarks>
        /// <returns>
        /// The output <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see> that the <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/>
        ///             will provide.
        /// </returns>
        protected override IEnumerable<HttpParameter> OnGetOutputParameters()
        {
            yield break;
        }

        /// <summary>
        /// Implemented in a derived class to provide the handling logic of the <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/>.
        /// </summary>
        /// <param name="input">The input values that the <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/> should be handled. 
        ///             The values should agree in order and type with the input <see cref="T:Microsoft.ApplicationServer.Http.Description.HttpParameter">HttpParameters</see> given by the <see cref="P:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler.InputParameters"/> property.
        ///             </param>
        /// <returns>
        /// An array that provides the output values.
        /// </returns>
        protected override Object[] OnHandle(Object[] input)
        {
            AuthorizeRequest();

            return new Object[0];
        }

        #endregion
    }
}