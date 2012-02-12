// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeUtility.cs">
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
//
// <summary>
//   Defines helper methods for using attributes and reflection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;

namespace NContext.Utilities
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
                             .FirstOrDefault(fi => 
                                 fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                   .Cast<DescriptionAttribute>()
                                   .Any(a => String.Compare(description, a.Description, StringComparison.OrdinalIgnoreCase) == 0));

            if (field == null)
            {
                throw new ArgumentOutOfRangeException("description", "Invalid argument. The enum does not contain a description attribute with the value supplied.");
            }

            return (TEnum)field.GetValue(null);
        }
    }
}