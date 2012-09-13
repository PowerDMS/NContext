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

namespace NContext.Extensions.EnterpriseLibrary.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

    using NContext.Security.Cryptography;

    /// <summary>
    /// Defines a provider for cryptographic hash operations.
    /// </summary>
    /// <remarks></remarks>
    public class HashProvider : IProvideHashing
    {
        private readonly Type _DefaultHashAlgorithm;

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
        }

        #region Hash CreateHash Methods

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash(Byte[] plainText, Boolean saltEnabled = true)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText, saltEnabled);
        }

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public String CreateHash(String plainText, Boolean saltEnabled = true)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText.ToBytes(), saltEnabled).ToHexadecimal();
        }

        /// <summary>
        /// Creates the base64 encoded hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public String CreateHashToBase64(String plainText, Boolean saltEnabled = true)
        {
            return CreateHash(_DefaultHashAlgorithm, plainText.ToBytes(), saltEnabled).ToBase64();
        }

        /// <summary>
        /// Creates the hash using the specified <typeparamref name="THashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash<THashAlgorithm>(Byte[] plainText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CreateHash(typeof(THashAlgorithm), plainText, saltEnabled);
        }

        /// <summary>
        /// Creates the hash using the specified <typeparamref name="THashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public String CreateHash<THashAlgorithm>(String plainText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CreateHash(typeof(THashAlgorithm), plainText.ToBytes(), saltEnabled).ToHexadecimal();
        }
        
        /// <summary>
        /// Creates the base64 encoded hash using the specified <typeparamref name="THashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public String CreateHashToBase64<THashAlgorithm>(String plainText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CreateHash(typeof(THashAlgorithm), plainText.ToBytes(), saltEnabled).ToBase64();
        }

        private Byte[] CreateHash(Type keyedHashAlgorithm, Byte[] plainText, Boolean saltEnabled = true)
        {
            var hashAlgorithmProvider = new HashAlgorithmProvider(keyedHashAlgorithm, saltEnabled);

            return hashAlgorithmProvider.CreateHash(plainText);
        }

        #endregion

        #region Hash CompareHash Methods

        /// <summary>
        /// Compares the hashes.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText, hashedText, saltEnabled);
        }

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(String plainText, String hashedText, Boolean saltEnabled = true)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText.ToBytes(), hashedText.ToBytesFromHexadecimal(), saltEnabled);
        }

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHashFromBase64(String plainText, String hashedText, Boolean saltEnabled = true)
        {
            return CompareHash(_DefaultHashAlgorithm, plainText.ToBytes(), hashedText.ToBytesFromBase64());
        }

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<THashAlgorithm>(Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CompareHash(typeof(THashAlgorithm), plainText, hashedText, saltEnabled);
        }

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<THashAlgorithm>(String plainText, String hashedText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CompareHash(typeof(THashAlgorithm), plainText.ToBytes(), hashedText.ToBytesFromHexadecimal(), saltEnabled);
        }

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHashFromBase64<THashAlgorithm>(String plainText, String hashedText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return CompareHash(typeof(THashAlgorithm), plainText.ToBytes(), hashedText.ToBytesFromBase64(), saltEnabled);
        }

        private Boolean CompareHash(Type hashAlgorithm, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true)
        {
            var hashAlgorithmProvider =
                new HashAlgorithmProvider(hashAlgorithm, saltEnabled);

            return hashAlgorithmProvider.CompareHash(plainText, hashedText);
        }

        #endregion
    }
}