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

namespace NContext.Utilities
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    // TODO: (DG) Clean up, get rid of un-desireables, refactor.
    public static class CryptographyUtility
    {
        /// <summary>
        /// <para>Determine if two byte arrays are equal.</para>
        /// </summary>
        /// <param name="byte1">
        /// <para>The first byte array to compare.</para>
        /// </param>
        /// <param name="byte2">
        /// <para>The byte array to compare to the first.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the two byte arrays are equal; otherwise <see langword="false"/>.</para>
        /// </returns>
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

            bool result = true;
            for (int i = 0; i < byte1.Length; i++)
            {
                if (byte1[i] != byte2[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// <para>Returns a byte array from a string representing a hexidecimal number.</para>
        /// </summary>
        /// <param name="hexidecimalNumber">
        /// <para>The string containing a valid hexidecimal number.</para>
        /// </param>
        /// <returns><para>The byte array representing the hexidecimal.</para></returns>
        public static Byte[] GetBytesFromHexString(string hexidecimalNumber)
        {
            if (hexidecimalNumber == null) throw new ArgumentNullException("hexidecimalNumber");

            var sb = new StringBuilder(hexidecimalNumber.ToUpperInvariant());
            if (sb[0].Equals('0') && sb[1].Equals('X'))
            {
                sb.Remove(0, 2);
            }

            if (sb.Length % 2 != 0)
            {
                throw new ArgumentException("String must represent a valid hexadecimal (e.g. : 0F99DD)");
            }

            Byte[] hexBytes = new Byte[sb.Length / 2];
            try
            {
                for (int i = 0; i < hexBytes.Length; i++)
                {
                    int stringIndex = i * 2;
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
        /// <para>Returns a string from a byte array represented as a hexidecimal number (eg: 0F351A).</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to convert to forat as a hexidecimal number.</para>
        /// </param>
        /// <returns>
        /// <para>The formatted representation of the bytes as a hexidcimal number.</para>
        /// </returns>
        public static string GetHexStringFromBytes(Byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length == 0) throw new ArgumentException("The value must be greater than 0 bytes.", "bytes");

            var sb = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2", CultureInfo.InvariantCulture));
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

        /// <summary>
        /// Creates a cryptographically strong random set of bytes.
        /// </summary>
        /// <param name="size">The size of the byte array to generate.</param>
        /// <returns>The computed bytes.</returns>
        public static Byte[] GetRandomBytes(int size)
        {
            Byte[] randomBytes = new Byte[size];
            GetRandomBytes(randomBytes);
            return randomBytes;
        }

        /// <summary>
        /// <para>Fills a byte array with a cryptographically strong random set of bytes.</para>
        /// </summary>
        /// <param name="bytes"><para>The byte array to fill.</para></param>
        public static void GetRandomBytes(Byte[] bytes)
        {
            RNGCryptoServiceProvider.Create().GetBytes(bytes);
        }

        /// <summary>
        /// <para>Fills <paramref name="bytes"/> zeros.</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to fill.</para>
        /// </param>
        public static void ZeroOutBytes(Byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }

            Array.Clear(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Transforms an array of bytes according to the given cryptographic transform.
        /// </summary>
        /// <param name="transform"><see cref="ICryptoTransform" /> used to transform the given <paramref name="buffer" />.</param>
        /// <param name="buffer">Buffer to transform. It is the responsibility of the caller to clear this array when finished.</param>
        /// <returns>Transformed array of bytes. It is the responsibility of the caller to clear this byte array
        /// if necessary.</returns>
        public static Byte[] Transform(ICryptoTransform transform, Byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            Byte[] transformBuffer = null;

            using (var ms = new MemoryStream())
            {
                CryptoStream cs = null;
                try
                {
                    cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();
                    transformBuffer = ms.ToArray();
                }
                finally
                {
                    if (cs != null)
                    {
                        cs.Close();
                        ((IDisposable)cs).Dispose();
                    } // Close is not called by Dispose
                }
            }

            return transformBuffer;
        }
    }
}