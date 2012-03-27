//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
/*
    Microsoft Public License (Ms-PL)
    This license governs use of the accompanying software. If you use the software, you accept 
    this license. If you do not accept the license, do not use the software.

    1. Definitions
    The terms "reproduce," "reproduction," "derivative works," and "distribution" have the 
    same meaning here as under U.S. copyright law.

    A "contribution" is the original software, or any additions or changes to the software.
    A "contributor" is any person that distributes its contribution under this license.
    "Licensed patents" are a contributor's patent claims that read directly on its contribution.

    2. Grant of Rights
    (A) Copyright Grant- Subject to the terms of this license, including the license conditions 
        and limitations in section 3, each contributor grants you a non-exclusive, worldwide, 
        royalty-free copyright license to reproduce its contribution, prepare derivative works 
        of its contribution, and distribute its contribution or any derivative works that you create.
    (B) Patent Grant- Subject to the terms of this license, including the license conditions 
        and limitations in section 3, each contributor grants you a non-exclusive, worldwide, 
        royalty-free license under its licensed patents to make, have made, use, sell, offer 
        for sale, import, and/or otherwise dispose of its contribution in the software or 
        derivative works of the contribution in the software.

    3. Conditions and Limitations
    (A) No Trademark License- This license does not grant you rights to use any 
        contributors' name, logo, or trademarks.
    (B) If you bring a patent claim against any contributor over patents that you claim are 
        infringed by the software, your patent license from such contributor to the software 
        ends automatically.
    (C) If you distribute any portion of the software, you must retain all copyright, patent, 
        trademark, and attribution notices that are present in the software.
    (D) If you distribute any portion of the software in source code form, you may do so only 
        under this license by including a complete copy of this license with your distribution. 
        If you distribute any portion of the software in compiled or object code form, you may 
        only do so under a license that complies with this license.
    (E) The software is licensed "as-is." You bear the risk of using it. The contributors give 
        no express warranties, guarantees or conditions. You may have additional consumer rights 
        under your local laws which this license cannot change. To the extent permitted under your 
        local laws, the contributors exclude the implied warranties of merchantability, fitness for 
        a particular purpose and non-infringement.
*/

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace NContext.Utilities
{
    /// <summary>
    /// <para>Common Cryptography methods.</para>
    /// </summary>
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
        public static bool CompareBytes(byte[] byte1, byte[] byte2)
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
        public static byte[] GetBytesFromHexString(string hexidecimalNumber)
        {
            if (hexidecimalNumber == null) throw new ArgumentNullException("hexidecimalNumber");

            StringBuilder sb = new StringBuilder(hexidecimalNumber.ToUpperInvariant());

            if (sb[0].Equals('0') && sb[1].Equals('X'))
            {
                sb.Remove(0, 2);
            }

            if (sb.Length % 2 != 0)
            {
                throw new ArgumentException("String must represent a valid hexadecimal (e.g. : 0F99DD)");
            }

            byte[] hexBytes = new byte[sb.Length / 2];
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
        public static string GetHexStringFromBytes(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length == 0) throw new ArgumentException("The value must be greater than 0 bytes.", "bytes");

            StringBuilder sb = new StringBuilder(bytes.Length * 2);
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
        public static byte[] CombineBytes(byte[] buffer1, byte[] buffer2)
        {
            if (buffer1 == null) throw new ArgumentNullException("buffer1");
            if (buffer2 == null) throw new ArgumentNullException("buffer2");

            byte[] combinedBytes = new byte[buffer1.Length + buffer2.Length];
            Buffer.BlockCopy(buffer1, 0, combinedBytes, 0, buffer1.Length);
            Buffer.BlockCopy(buffer2, 0, combinedBytes, buffer1.Length, buffer2.Length);

            return combinedBytes;
        }

        /// <summary>
        /// Creates a cryptographically strong random set of bytes.
        /// </summary>
        /// <param name="size">The size of the byte array to generate.</param>
        /// <returns>The computed bytes.</returns>
        public static byte[] GetRandomBytes(int size)
        {
            byte[] randomBytes = new byte[size];
            GetRandomBytes(randomBytes);
            return randomBytes;
        }

        /// <summary>
        /// <para>Fills a byte array with a cryptographically strong random set of bytes.</para>
        /// </summary>
        /// <param name="bytes"><para>The byte array to fill.</para></param>
        public static void GetRandomBytes(byte[] bytes)
        {
            RNGCryptoServiceProvider.Create().GetBytes(bytes);
        }

        /// <summary>
        /// <para>Fills <paramref name="bytes"/> zeros.</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to fill.</para>
        /// </param>
        public static void ZeroOutBytes(byte[] bytes)
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
        public static byte[] Transform(ICryptoTransform transform, byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            byte[] transformBuffer = null;

            using (MemoryStream ms = new MemoryStream())
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