// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBase.cs" company="Waking Venture, Inc.">
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

namespace NContext.ErrorHandling
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Resources;
    using System.Text.RegularExpressions;
    
    using NContext.Common;

    /// <summary>
    /// Defines an abstraction for localized application errors.
    /// </summary>
    /// <remarks></remarks>
    public abstract class ErrorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBase"/> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="errorMessageParameters">The error message parameters.</param>
        /// <remarks></remarks>
        protected ErrorBase(String localizationKey, HttpStatusCode httpStatusCode, params Object[] errorMessageParameters) 
            : this(localizationKey, httpStatusCode, null, errorMessageParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBase"/> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="code">A unique code that represents the reason for the error.</param>
        /// <param name="errorMessageParameters">The error message parameters.</param>
        /// <remarks></remarks>
        protected ErrorBase(String localizationKey, HttpStatusCode httpStatusCode, String code, params Object[] errorMessageParameters)
        {
            HttpStatusCode = httpStatusCode;
            Code = code;
            SetErrorMessage(localizationKey, errorMessageParameters);
        }

// ReSharper disable once UnusedMember.Local
        private ErrorBase()
        {
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NContext.ErrorHandling.ErrorBase"/> to <see cref="Error"/>.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator Error(ErrorBase error)
        {
            return new Error(
                (Int32)error.HttpStatusCode,
                error.Code,
                new List<String>
                {
                    error.Message
                });
        }

        /// <summary>
        /// Gets the code representing the reason for the error.
        /// </summary>
        /// <value>The code.</value>
        public String Code { get; private set; }

        /// <summary>
        /// Gets the localized error message.
        /// </summary>
        /// <remarks></remarks>
        public String Message { get; private set; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <remarks></remarks>
        public HttpStatusCode HttpStatusCode { get; private set; }

        /// <summary>
        /// Returns a default <see cref="Error"/> instance.
        /// </summary>
        /// <returns>Error instance.</returns>
        public static Error NullObject()
        {
            return new Error((Int32)HttpStatusCode.InternalServerError, String.Empty, Enumerable.Empty<String>());
        }

        /// <summary>
        /// Gets the localized error message.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <returns>String.</returns>
        protected virtual String GetLocalizedErrorMessage(String localizationKey)
        {
            if (String.IsNullOrWhiteSpace(localizationKey))
            {
                return null;
            }

            var message = String.Empty;
            var errorType = GetType();

            /* Reflect this instance type's assembly for the associated resource file.
             * The Type.Name of this instance is used as a naming convention for localizing errors.
             * If found, try to get the localized string for the error message.
             * */
            var assembly = Assembly.GetAssembly(errorType);
            var resourceBaseName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(res => res.IndexOf(errorType.Name, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToMaybe()
                .Bind(resName => Regex.Replace(resName, String.Format("(?<=.*{0})(?:\\..*)?\\.resources", errorType.Name), String.Empty).ToMaybe())
                .FromMaybe(String.Empty);

            try
            {
                var resourceManager = new ResourceManager(resourceBaseName, assembly);
                if (resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true) != null)
                {
                    message = resourceManager.GetString(localizationKey, CultureInfo.CurrentUICulture);
                }
            }
            catch
            {
                // No culture-specific or neutral resource exists so let's just return message below.
            }

            return message;
        }

        private void SetErrorMessage(String localizationKey, params Object[] errorMessageParameters)
        {
            String errorMessage = GetLocalizedErrorMessage(localizationKey) ?? String.Empty;
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                var formatters = Extensions.StringExtensions.MinimumFormatParametersRequired(errorMessage);
                if (formatters > 0)
                {
                    errorMessage = String.Format(errorMessage, errorMessageParameters);
                }
            }

            Message = errorMessage;
        }
    }
}