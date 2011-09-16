// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeUtility.cs">
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
//   Defines helper methods for using attributes and reflection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Linq;

namespace NContext.Application.Utilities
{
    /// <summary>
    /// Defines helper methods for using attributes and reflection.
    /// </summary>
    public sealed class AttributeUtility
    {
        /// <summary>
        /// Gets the description attribute value from a field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The value of the <see cref="DescriptionAttribute"/>.</returns>
        /// <remarks></remarks>
        public static String GetDescriptionAttributeValueFromField(Object field)
        {
            var objectField = field.GetType().GetField(field.ToString());
            var descriptionAttribute = objectField.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

            return (descriptionAttribute != null) ? descriptionAttribute.Description : String.Empty;
        }

        /// <summary>
        /// Gets the enum value from the specified description attribute value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="description">The description.</param>
        /// <returns>The <typeparamref name="TEnum"/> value.</returns>
        /// <remarks></remarks>
        public static TEnum GetEnumValueFromDescriptionAttributeValue<TEnum>(String description)
        {
            var field =
                typeof(TEnum).GetFields()
                             .ToList()
                             .Where(fi => fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                            .Cast<DescriptionAttribute>()
                                            .Any(a => String.Compare(description, a.Description, true) == 0))
                             .FirstOrDefault();

            if (field == null)
            {
                throw new ArgumentOutOfRangeException("description", "Invalid argument. The enum does not contain a description attribute with the value supplied.");
            }

            return (TEnum)field.GetValue(null);
        }
    }
}