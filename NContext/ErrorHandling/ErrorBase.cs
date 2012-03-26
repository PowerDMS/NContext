// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBase.cs">
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
//   Defines an abstraction for localized application errors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

using NContext.Extensions;

namespace NContext.ErrorHandling
{
    /// <summary>
    /// Defines an abstraction for localized application errors.
    /// </summary>
    /// <remarks></remarks>
    public abstract class ErrorBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBase"/> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="errorMessageParameters">The error message parameters.</param>
        /// <remarks></remarks>
        protected ErrorBase(String localizationKey, params Object[] errorMessageParameters)
            : this(localizationKey, HttpStatusCode.InternalServerError, errorMessageParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBase"/> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="errorMessageParameters">The error message parameters.</param>
        /// <remarks></remarks>
        protected ErrorBase(String localizationKey, HttpStatusCode httpStatusCode, params Object[] errorMessageParameters)
        {
            ErrorType = GetType();
            HttpStatusCode = httpStatusCode;
            SetErrorMessage(localizationKey, errorMessageParameters);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error name.
        /// </summary>
        /// <remarks></remarks>
        public Type ErrorType { get; private set; }

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

        #endregion

        #region Methods

        private void SetErrorMessage(String localizationKey, params Object[] errorMessageParameters)
        {
            String errorMessage = GetLocalizedErrorMessage(localizationKey) ?? String.Empty;
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                var formatters = errorMessage.MinimumFormatParametersRequired();
                if (formatters > 0)
                {
                    errorMessage = String.Format(errorMessage, errorMessageParameters);
                }
            }

            Message = errorMessage;
        }

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

        #endregion
    }
}