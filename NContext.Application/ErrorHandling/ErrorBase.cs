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

        protected ErrorBase(Type errorType, String localizationKey, params Object[] errorTypeParameters)
        {
            ConfigureError(errorType, localizationKey, errorTypeParameters);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <remarks></remarks>
        public String Name { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <remarks></remarks>
        public String Message { get; private set; }

        /// <summary>
        /// Gets or sets the localization directory.
        /// </summary>
        /// <value>The localization directory.</value>
        /// <remarks></remarks>
        protected internal abstract String LocalizationDirectory { get; }

        #endregion

        #region Methods

        protected internal void ConfigureError(Type errorType, String localizationKey, params Object[] errorTypeParameters)
        {
            ErrorAttribute errorAttribute = 
                errorType.GetField(localizationKey)
                         .GetCustomAttributes(typeof(ErrorAttribute), false)
                         .Cast<ErrorAttribute>()
                         .SingleOrDefault();

            if (errorAttribute != null)
            {
                Name = localizationKey;
                Message = GetErrorMessage(errorType, localizationKey, errorTypeParameters);
            }
        }

        private String GetErrorMessage(Type errorType, String localizationKey, params Object[] errorTypeParameters)
        {
            String errorLocalizationKey = null, errorDefaultMessage = null;
            ErrorAttribute errorAttribute = 
                errorType.GetField(localizationKey)
                         .GetCustomAttributes(typeof(ErrorAttribute), false)
                         .Cast<ErrorAttribute>()
                         .SingleOrDefault();

            if (errorAttribute != null)
            {
                if (!String.IsNullOrEmpty(errorAttribute.LocalizationKey))
                {
                    errorLocalizationKey = errorAttribute.LocalizationKey;
                }

                if (!String.IsNullOrEmpty(errorAttribute.DefaultMessage))
                {
                    errorDefaultMessage = errorAttribute.DefaultMessage;
                }
            }

            var tempDescription = GetLocalizedErrorMessage(errorType, errorLocalizationKey);
            String errorMessage = (String.IsNullOrEmpty(tempDescription) && !String.IsNullOrEmpty(errorDefaultMessage))
                                      ? errorDefaultMessage
                                      : tempDescription;

            if (!String.IsNullOrEmpty(errorMessage))
            {
                var formatters = errorMessage.MinimumFormatParametersRequired();
                if (formatters > 0)
                {
                    if (errorTypeParameters.Count() != formatters)
                    {
                        // TODO: (DG) Logging - Incorrect parameters being used

                    }
                    else
                    {
                        errorMessage = String.Format(errorMessage, errorTypeParameters);
                    }
                }
            }

            return errorMessage;
        }

        private String GetLocalizedErrorMessage(Type errorType, String localizationKey)
        {
            if (String.IsNullOrEmpty(localizationKey))
            {
                return String.Empty;
            }

            var message = String.Empty;
            var resourceManager = new ResourceManager(String.Format("{0}.{1}", LocalizationDirectory, errorType.Name), Assembly.GetExecutingAssembly());
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