// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorAttribute.cs">
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

// <summary>
//   ErrorAttribute can be applied to enum fields within a ErrorBase derived class.
//   The attribute allows for automated localization via the supported properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Application.ErrorHandling
{
    /// <summary>
    /// ErrorAttribute can be applied to enum fields within a ErrorBase derived class.
    /// The attribute allows for automated localization via the supported properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ErrorAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the default message.
        /// </summary>
        /// <value>The default message.</value>
        public String DefaultMessage { get; set; }

        /// <summary>
        /// Gets or sets the help link url.
        /// </summary>
        /// <value>The help link.</value>
        public String HelpLinkUrl { get; set; }

        /// <summary>
        /// Gets or sets the localization key.
        /// </summary>
        /// <value>The localization key.</value>
        public String LocalizationKey { get; set; }
    }
}