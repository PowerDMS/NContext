// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteExtensions.cs">
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
//   Defines extension methods for byte.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;

namespace NContext.Extensions
{
    /// <summary>
    /// Defines extension methods for byte.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Gets the hexadecimal representation of the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes to convert to hexadecimal.</param>
        /// <returns>Hexadecimal string value.</returns>
        /// <remarks></remarks>
        public static String ToHexadecimal(this Byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentException("Byte array length must be greater than zero.", "bytes");
            }

            var stringBuilder = new StringBuilder(bytes.Length * 2);
            for (var index = 0; index < bytes.Length; ++index)
            {
                stringBuilder.Append(bytes[index].ToString("X2", CultureInfo.InvariantCulture));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the base64 encoded string from the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Base64 encoded string.</returns>
        /// <remarks></remarks>
        public static String ToBase64(this Byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Gets the UTF8 encoded string from the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>UTF8 encoded string.</returns>
        /// <exception cref=""></exception>
        /// <remarks></remarks>
        public static String ToUTF8(this Byte[] bytes)
        {
            var encoding = new UTF8Encoding(false, true);

            return encoding.GetString(bytes);
        }
    }
}