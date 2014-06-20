// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyUtility.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;

namespace NContext.Security.Cryptography
{
    using System.IO;
    using System.Linq;

    public static class CryptographyUtility
    {
        public static Boolean CompareBytes(Byte[] byte1, Byte[] byte2)
        {
            if (byte1 == null || byte2 == null)
            {
                return false;
            }
            if (byte1.Length != byte2.Length)
            {
                return false;
            }

            return !byte1.Where((t, i) => t != byte2[i]).Any();
        }

        /// <summary>
        /// <para>Returns a byte array from a string representing a hexadecimal number.</para>
        /// </summary>
        /// <param name="hexadecimalNumber">
        /// <para>The string containing a valid hexadecimal number.</para>
        /// </param>
        /// <returns><para>The byte array representing the hexadecimal.</para></returns>
        public static Byte[] GetBytesFromHexString(String hexadecimalNumber)
        {
            if (hexadecimalNumber == null) throw new ArgumentNullException("hexadecimalNumber");

            var sb = new StringBuilder(hexadecimalNumber.ToUpperInvariant());
            if (sb[0].Equals('0') && sb[1].Equals('X'))
            {
                sb.Remove(0, 2);
            }

            if (sb.Length % 2 != 0)
            {
                throw new ArgumentException("String must represent a valid hexadecimal (e.g. : 0F99DD)");
            }

            var hexBytes = new Byte[sb.Length / 2];
            try
            {
                for (var i = 0; i < hexBytes.Length; i++)
                {
                    var stringIndex = i * 2;
                    hexBytes[i] = Convert.ToByte(sb.ToString(stringIndex, 2), 16);
                }
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("String must represent a valid hexadecimal (e.g. : 0F99DD)", ex);
            }

            return hexBytes;
        }

        /// <summary>
        /// <para>Returns a string from a byte array represented as a hexadecimal number (eg: 0F351A).</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to convert to a hexadecimal number.</para>
        /// </param>
        /// <returns>
        /// <para>The formatted representation of the bytes as a hexadecimal number.</para>
        /// </returns>
        public static String GetHexStringFromBytes(Byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length == 0) throw new ArgumentException("The value must be greater than 0 bytes.", "bytes");

            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var bite in bytes)
            {
                sb.Append(bite.ToString("X2", CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }

        /// <summary>
        /// <para>Combines two byte arrays into one.</para>
        /// </summary>
        /// <param name="buffer1"><para>The prefixed bytes.</para></param>
        /// <param name="buffer2"><para>The suffixed bytes.</para></param>
        /// <returns><para>The combined byte arrays.</para></returns>
        public static Byte[] CombineBytes(Byte[] buffer1, Byte[] buffer2)
        {
            if (buffer1 == null) throw new ArgumentNullException("buffer1");
            if (buffer2 == null) throw new ArgumentNullException("buffer2");

            Byte[] combinedBytes = new Byte[buffer1.Length + buffer2.Length];
            Buffer.BlockCopy(buffer1, 0, combinedBytes, 0, buffer1.Length);
            Buffer.BlockCopy(buffer2, 0, combinedBytes, buffer1.Length, buffer2.Length);

            return combinedBytes;
        }
        
        public static Byte[] GetBytes(Byte[] bytes, Int32 count, Int32 offset = 0)
        {
            var copiedBytes = new Byte[count];
            Buffer.BlockCopy(bytes, offset, copiedBytes, 0, count);

            return copiedBytes;
        }

        public static Byte[] GetBytes(Stream stream, Int32 count, Int32 offset = 0)
        {
            var copiedBytes = new Byte[count];
            stream.Read(copiedBytes, offset, count);
            stream.Position = 0;

            return copiedBytes;
        }
    }
}