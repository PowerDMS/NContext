// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashProvider.cs" company="Waking Venture, Inc.">
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

namespace NContext.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using NContext.Extensions;

    /// <summary>
    /// Defines a provider for cryptographic hash operations.
    /// </summary>
    public class HashProvider : IProvideHashing
    {
        private readonly Type _DefaultHashAlgorithm;

        private readonly RandomNumberGenerator _RngCryptoServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashProvider"/> class.
        /// </summary>
        /// <param name="defaultHashAlgorithm">The default <see cref="HashAlgorithm"/>.</param>
        /// <remarks></remarks>
        public HashProvider(Type defaultHashAlgorithm)
        {
            if (defaultHashAlgorithm == null)
            {
                throw new ArgumentNullException("defaultHashAlgorithm");
            }

            if (!defaultHashAlgorithm.Implements<HashAlgorithm>())
            {
                throw new ArgumentException("DefaultHashAlgorithm is invalid. Must be of type HashAlgorithm.", "defaultHashAlgorithm");
            }

            _DefaultHashAlgorithm = defaultHashAlgorithm;
            _RngCryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public Byte[] CreateHash(Byte[] plainText, Int32 saltLength = 16)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText, saltLength);
        }

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public String CreateHash(String plainText, Int32 saltLength = 16)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText.ToBytes<UnicodeEncoding>(), saltLength).ToHexadecimal();
        }

        /// <summary>
        /// Creates the base64 encoded hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public String CreateHashToBase64(String plainText, Int32 saltLength = 16)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText.ToBytes<UnicodeEncoding>(), saltLength).ToBase64();
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public Byte[] CreateHash<THashAlgorithm>(Byte[] plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CreateHash(typeof(THashAlgorithm), plainText, saltLength);
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public String CreateHash<THashAlgorithm>(String plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CreateHash(typeof(THashAlgorithm), plainText.ToBytes<UnicodeEncoding>(), saltLength).ToHexadecimal();
        }

        /// <summary>
        /// Creates the base64 encoded hash using the specified <typeparamref name="THashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        public String CreateHashToBase64<THashAlgorithm>(String plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CreateHash(typeof(THashAlgorithm), plainText.ToBytes<UnicodeEncoding>(), saltLength).ToBase64();
        }

        private Byte[] CreateHash(Type hashAlgorithmType, Byte[] plainText, Int32 saltLength = 16)
        {
            var hashAlgorithm = Activator.CreateInstance(hashAlgorithmType, true) as HashAlgorithm;
            if (hashAlgorithm == null)
            {
                throw new InvalidOperationException(String.Format("Could not create instance of type {0}.", hashAlgorithmType));
            }

            // Generate salt
            var salt = new Byte[saltLength];
            _RngCryptoServiceProvider.GetNonZeroBytes(salt);

            // Compute hash
            var hashedText = hashAlgorithm.ComputeHash(CryptographyUtility.CombineBytes(salt, plainText));

            // Randomize plain text
            _RngCryptoServiceProvider.GetBytes(plainText);

            return CryptographyUtility.CombineBytes(salt, hashedText);
        }

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash(Byte[] plainText, Byte[] hashedText, Int32 saltLength = 16)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText, hashedText, saltLength);
        }

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash(String plainText, String hashedText, Int32 saltLength = 16)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText.ToBytes<UnicodeEncoding>(), hashedText.ToBytesFromHexadecimal(), saltLength);
        }

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHashFromBase64(String plainText, String hashedText, Int32 saltLength = 16)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText.ToBytes<UnicodeEncoding>(), hashedText.ToBytesFromBase64(), saltLength);
        }

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash<THashAlgorithm>(Byte[] plainText, Byte[] hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CompareHash(typeof(THashAlgorithm), plainText, hashedText, saltLength);
        }

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHash<THashAlgorithm>(String plainText, String hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CompareHash(typeof(THashAlgorithm), plainText.ToBytes<UnicodeEncoding>(), hashedText.ToBytesFromHexadecimal(), saltLength);
        }

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm" />.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText" />.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        public Boolean CompareHashFromBase64<THashAlgorithm>(String plainText, String hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new()
        {
            return CompareHash(typeof(THashAlgorithm), plainText.ToBytes<UnicodeEncoding>(), hashedText.ToBytesFromBase64(), saltLength);
        }

        private Boolean CompareHash(Type hashAlgorithmType, Byte[] plainText, Byte[] hashedText, Int32 saltLength = 16)
        {
            var hashAlgorithm = Activator.CreateInstance(hashAlgorithmType, true) as HashAlgorithm;
            if (hashAlgorithm == null)
            {
                throw new InvalidOperationException(String.Format("Could not create instance of type {0}.", hashAlgorithmType));
            }

            var salt = CryptographyUtility.GetBytes(hashedText, saltLength);
            var targetHash = CryptographyUtility.CombineBytes(salt, hashAlgorithm.ComputeHash(CryptographyUtility.CombineBytes(salt, plainText)));

            return CryptographyUtility.CompareBytes(hashedText, targetHash);
        }
    }
}