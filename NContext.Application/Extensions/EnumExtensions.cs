// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Defines a static class for providing enum type extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;

using NContext.Application.Utilities;

namespace NContext.Application.Extensions
{
    /// <summary>
    /// Defines a static class for providing enum type extension methods.
    /// </summary>
    /// <remarks></remarks>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the <see cref="DescriptionAttribute"/> value based upon the enum value specified.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of the <see cref="DescriptionAttribute"/> if one exists, else <see cref="String.Empty"/>.</returns>
        /// <remarks></remarks>
        public static String ToDescriptionValue(this Enum value)
        {
            return AttributeUtility.GetDescriptionAttributeValueFromField(value);
        }
    }
}