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
//
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
        /// Gets or sets the localization key. If none is set, <see cref="ErrorBase"/> will 
        /// attempt to lookup the localized error message using the enumeration value as a key.
        /// </summary>
        /// <value>The localization key.</value>
        public String LocalizationKey { get; set; }
    }
}