// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensions.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AspNetWebApi.Extensions
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using NContext.Common;

    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        /// <summary>
        /// Returns a new <see cref="HttpResponseMessage"/> with the <see cref="HttpResponseMessage.Content"/> set to <paramref name="responseContent"/>. If 
        /// <paramref name="responseContent"/> contains an error, it will attempt to parse the <see cref="Error.ErrorCode"/> as an <see cref="HttpStatusCode"/> 
        /// and assign it to the response message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseContent">The content contained in the HTTP response.</param>
        /// <param name="httpRequestMessage">The active <see cref="HttpRequestMessage"/>.</param>
        /// <param name="nonErrorHttpStatusCode">The <see cref="HttpStatusCode"/> to set if <paramref name="responseContent"/> has no errors.</param>
        /// <returns>HttpResponseMessage instance.</returns>
        public static HttpResponseMessage ToHttpResponseMessage<T>(this IResponseTransferObject<T> responseContent, HttpRequestMessage httpRequestMessage, HttpStatusCode nonErrorHttpStatusCode = HttpStatusCode.OK)
        {
            if (responseContent == null)
            {
                throw new ArgumentNullException("responseContent");
            }

            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }

            HttpStatusCode statusCode = nonErrorHttpStatusCode;
            if (responseContent.Errors.Any())
            {
                Enum.TryParse<HttpStatusCode>(responseContent.Errors.First().ErrorCode, true, out statusCode);
            }

            return httpRequestMessage.CreateResponse(statusCode, responseContent);
        }

        /// <summary>
        /// Invokes the specified <paramref name="responseBuilder" /> action if <paramref name="responseContent" /> does not contain an error - returning the configured <see cref="HttpResponseMessage" />.
        /// If <paramref name="responseContent" /> contains errors, the returned response with contain the error content and will attempt to parse the <see cref="Error.ErrorCode" /> as an
        /// <see cref="HttpStatusCode" /> and assign it to the response message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseContent">The <see cref="IResponseTransferObject{t}" /> used to build the <see cref="HttpResponseMessage" />.</param>
        /// <param name="httpRequestMessage">The active <see cref="HttpRequestMessage" />.</param>
        /// <param name="responseBuilder">The response builder method to invoke when no errors exist.</param>
        /// <returns>HttpResponseMessage instance.</returns>
        /// <exception cref="System.ArgumentNullException">responseContent</exception>
        public static HttpResponseMessage ToHttpResponseMessage<T>(this IResponseTransferObject<T> responseContent, HttpRequestMessage httpRequestMessage, Action<T, HttpResponseMessage> responseBuilder)
        {
            if (responseContent == null)
            {
                throw new ArgumentNullException("responseContent");
            }

            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }
            
            if (responseContent.Errors.Any())
            {
                var statusCode = HttpStatusCode.BadRequest;
                Enum.TryParse<HttpStatusCode>(responseContent.Errors.First().ErrorCode, true, out statusCode);

                return httpRequestMessage.CreateResponse(statusCode, responseContent);
            }

            var response = httpRequestMessage.CreateResponse();
            responseBuilder.Invoke(responseContent.Data, response);

            return response;
        }
    }
}