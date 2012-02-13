// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseTransferObjectOperationHandler.cs">
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
//   Defines and operation handler for translating response instances of IResponseTransferObject<T> to HttpResponseMessage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using Microsoft.ApplicationServer.Http.Dispatcher;

using NContext.Dto;

namespace NContext.Extensions.WCF.WebApi
{
    /// <summary>
    /// Defines and operation handler for translating response instances 
    /// of <see cref="IResponseTransferObject{T}"/> to <see cref="HttpResponseMessage"/>.
    /// </summary>
    public class ResponseTransferObjectOperationHandler : HttpOperationHandler<HttpResponseMessage, HttpResponseMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTransferObjectOperationHandler"/> class.
        /// </summary>
        /// <param name="outputParameterName">Name of the output parameter.</param>
        /// <remarks></remarks>
        public ResponseTransferObjectOperationHandler(String outputParameterName = "responseMessage")
            : base(outputParameterName)
        {
        }

        #region Overrides of HttpOperationHandler<HttpResponseMessage,HttpResponseMessage>

        /// <summary>
        /// Implemented in a derived class to provide the handling logic of the custom <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/>.
        /// </summary>
        /// <param name="input">The input value that the <see cref="T:Microsoft.ApplicationServer.Http.Dispatcher.HttpOperationHandler"/> should handle. 
        ///             </param>
        /// <returns>
        /// The output value returned.
        /// </returns>
        protected override HttpResponseMessage OnHandle(HttpResponseMessage input)
        {
            dynamic response = input.Content.ReadAsOrDefaultAsync(typeof(IResponseTransferObject<>)).Result;
            if (response != null)
            {
                HttpStatusCode statusCode;
                var errors = (IEnumerable<Error>)response.Errors;
                if (errors.Any() && Enum.TryParse<HttpStatusCode>(errors.First().ErrorCode, false, out statusCode))
                {
                    input.StatusCode = statusCode;
                }
            }

            return input;
        }

        #endregion
    }
}