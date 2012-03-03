// Type: Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.CryptographyUtility

using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// 
    /// <para>
    /// Common Cryptography methods.
    /// </para>
    /// 
    /// </summary>
    public static class CryptographyUtility
    {
        /// <summary>
        /// 
        /// <para>
        /// Determine if two byte arrays are equal.
        /// </para>
        /// 
        /// </summary>
        /// <param name="byte1">
        /// <para>
        /// The first byte array to compare.
        /// </para>
        /// </param><param name="byte2">
        /// <para>
        /// The byte array to compare to the first.
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para>
        /// <see langword="true"/> if the two byte arrays are equal; otherwise <see langword="false"/>.
        /// </para>
        /// 
        /// </returns>
        public static Boolean CompareBytes(Byte[] byte1, Byte[] byte2)
        {
            if (byte1 == null || byte2 == null || byte1.Length != byte2.Length)
            {
                return false;
            }

            return !byte1.Where((t, index) => t != (Int32)byte2[index]).Any();
        }

        /// <summary>
        /// 
        /// <para>
        /// Combines two byte arrays into one.
        /// </para>
        /// 
        /// </summary>
        /// <param name="buffer1">
        /// <para>
        /// The prefixed bytes.
        /// </para>
        /// </param><param name="buffer2">
        /// <para>
        /// The suffixed bytes.
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para>
        /// The combined byte arrays.
        /// </para>
        /// 
        /// </returns>
        public static Byte[] CombineBytes(Byte[] buffer1, Byte[] buffer2)
        {
            if (buffer1 == null)
                throw new ArgumentNullException("buffer1");
            if (buffer2 == null)
                throw new ArgumentNullException("buffer2");

            var numArray = new Byte[buffer1.Length + buffer2.Length];
            Buffer.BlockCopy(buffer1, 0, numArray, 0, buffer1.Length);
            Buffer.BlockCopy(buffer2, 0, numArray, buffer1.Length, buffer2.Length);

            return numArray;
        }

        public static Byte[] ExtractSalt(Byte[] hashedtext)
        {
            if (hashedtext == null)
                throw new ArgumentNullException("hashedtext");

            var numArray = (Byte[])null;
            if (hashedtext.Length > 16)
            {
                numArray = new byte[16];
                Buffer.BlockCopy(hashedtext, 0, numArray, 0, 16);
            }

            return numArray;
        }

        public static Byte[] AddSaltToPlainText(Byte[] salt, Byte[] plaintext)
        {
            return CombineBytes(salt, plaintext);
        }

        public static Byte[] AddSaltToHash(Byte[] salt, Byte[] hash)
        {
            return CombineBytes(salt, hash);
        }
    }
}