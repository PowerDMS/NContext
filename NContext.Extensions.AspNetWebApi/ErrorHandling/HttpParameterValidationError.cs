// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpParameterValidationError.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AspNetWebApi.ErrorHandling
{
    using System;
    using System.Net;
    using System.Reflection;
    using NContext.ErrorHandling;

    /// <summary>
    /// Defines validation-related errors.
    /// </summary>
    internal class HttpParameterValidationError : ErrorBase
    {
        private HttpParameterValidationError(String localizationKey, HttpStatusCode httpStatusCode, params Object[] errorMessageParameters)
            : base(localizationKey, httpStatusCode, errorMessageParameters)
        {
        }

        /// <summary>
        /// The required resource parameter '{0}' of type '{1}' could not be found within the request body for validation.
        /// </summary>
        /// <param name="parameterType">Type of the parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>HttpParameterValidationError.</returns>
        public static HttpParameterValidationError RequiredParameterNotFoundInBody(Type parameterType, String parameterName)
        {
            return new HttpParameterValidationError(MethodBase.GetCurrentMethod().Name, HttpStatusCode.BadRequest, parameterName, parameterType.Name);
        }

        /// <summary>
        /// The required resource parameter '{0}' with value '{1}' does not match the value set in the request body: '{2}'.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="requiredParameterValue">The required parameter value.</param>
        /// <param name="bodyPropertyValue">The body property value.</param>
        /// <returns>HttpParameterValidationError.</returns>
        public static HttpParameterValidationError ValidationFailed(String parameterName, Object requiredParameterValue, Object bodyPropertyValue)
        {
            return new HttpParameterValidationError(MethodBase.GetCurrentMethod().Name, HttpStatusCode.BadRequest, parameterName, requiredParameterValue, bodyPropertyValue);
        }
    }
}