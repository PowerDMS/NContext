// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBase.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines an abstraction for localized application errors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

using NContext.Application.Extensions;

namespace NContext.Application.ErrorHandling
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
        /// <param name="errorType">Type of the error.</param>
        /// <param name="errorName">Name of the error.</param>
        /// <param name="errorMessageMessageParameters">The error message parameters.</param>
        /// <remarks></remarks>
        protected ErrorBase(Type errorType, String errorName, params Object[] errorMessageMessageParameters)
        {
            if (errorType == null)
            {
                throw new ArgumentNullException("errorType");
            }

            if (errorName == null)
            {
                throw new ArgumentNullException("errorName");
            }

            ConfigureError(errorType, errorName, errorMessageMessageParameters);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error name.
        /// </summary>
        /// <remarks></remarks>
        public String Name { get; private set; }

        /// <summary>
        /// Gets the localized error message.
        /// </summary>
        /// <remarks></remarks>
        public String Message { get; private set; }

        /// <summary>
        /// Gets the localization resource.
        /// </summary>
        /// <remarks></remarks>
        protected abstract Type LocalizationResource { get; }

        #endregion

        #region Methods

        private void ConfigureError(Type errorType, String defaultLocalizationKey, params Object[] errorMessageParameters)
        {
            ErrorAttribute errorAttribute =
                errorType.GetField(defaultLocalizationKey)
                         .GetCustomAttributes(typeof(ErrorAttribute), false)
                         .Cast<ErrorAttribute>()
                         .SingleOrDefault();

            String localizationKey = defaultLocalizationKey;
            if (errorAttribute != null && !String.IsNullOrWhiteSpace(errorAttribute.LocalizationKey))
            {
                localizationKey = errorAttribute.LocalizationKey;
            }

            String errorMessage = TryGetLocalizedErrorMessage(localizationKey) ?? String.Empty;
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                var formatters = errorMessage.MinimumFormatParametersRequired();
                if (formatters > 0)
                {
                    var specifiedParameterCount = errorMessageParameters.Length;
                    if (specifiedParameterCount != formatters)
                    {
                        // TODO: (DG) Logging - Incorrect parameters being used.
                        if (specifiedParameterCount > formatters)
                        {
                            // We can still format the string, though some parameters will not be used.
                            errorMessage = String.Format(errorMessage, errorMessageParameters);
                        }
                    }
                    else
                    {
                        errorMessage = String.Format(errorMessage, errorMessageParameters);
                    }
                }
            }

            Name = localizationKey;
            Message = errorMessage;
        }

        private String TryGetLocalizedErrorMessage(String localizationKey)
        {
            if (String.IsNullOrWhiteSpace(localizationKey))
            {
                return null;
            }

            var message = String.Empty;
            var resourceManager = new ResourceManager(String.Format("{0}.{1}", LocalizationResource.Namespace, LocalizationResource.Name), Assembly.GetAssembly(LocalizationResource));
            try
            {
                if (resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true) != null)
                {
                    message = resourceManager.GetString(localizationKey, CultureInfo.CurrentUICulture);
                }
            }
            catch (MissingManifestResourceException)
            {
                // No culture-specific or neutral resource exists so let's just return message below.
            }

            return message;
        }

        #endregion
    }
}