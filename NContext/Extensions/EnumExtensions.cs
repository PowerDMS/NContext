// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs">
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
//   Defines a static class for providing enum type extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;

using NContext.Utilities;

namespace NContext.Extensions
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