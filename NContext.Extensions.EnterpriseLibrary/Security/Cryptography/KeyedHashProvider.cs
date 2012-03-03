// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyedHashProvider.cs">
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
//   Defines an EnterpriseLibrary implementation of IProvideKeyedHashing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;

using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

using NContext.Security.Cryptography;

namespace NContext.Extensions.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Defines an EnterpriseLibrary implementation of <see cref="IProvideKeyedHashing"/>.
    /// </summary>
    /// <remarks></remarks>
    public class KeyedHashProvider : IProvideKeyedHashing
    {
        #region Fields

        private readonly Type _DefaultKeyedHashAlgorithm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedHashProvider"/> class.
        /// </summary>
        /// <param name="defaultKeyedHashAlgorithm">The default <see cref="KeyedHashAlgorithm"/>.</param>
        /// <remarks></remarks>
        public KeyedHashProvider(Type defaultKeyedHashAlgorithm)
        {
            if (defaultKeyedHashAlgorithm == null)
            {
                throw new ArgumentNullException("defaultKeyedHashAlgorithm");
            }

            if (!defaultKeyedHashAlgorithm.Implements<KeyedHashAlgorithm>())
            {
                throw new ArgumentException("DefaultKeyedHashAlgorithm is invalid. Must be of type KeyedHashAlgorithm.", "defaultKeyedHashAlgorithm");
            }

            _DefaultKeyedHashAlgorithm = defaultKeyedHashAlgorithm;
        }

        #endregion

        #region Keyed-Hash CreateHash Methods

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// using the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey, plainText, saltEnabled, dataProtectionScope);
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key using the default <see cref="KeyedHashAlgorithm"/>.
        /// Returns the hash as hexadecimal.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>Hexadecimal representation of the hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHash(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey.ToBytes(), plainText.ToBytes(), saltEnabled, dataProtectionScope).ToHexadecimal();
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// using the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHashAsBase64(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey.ToBytes(), plainText.ToBytes(), saltEnabled, dataProtectionScope).ToBase64();
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CreateHash(typeof(TKeyedHashAlgorithm), symmetricKey, plainText, saltEnabled, dataProtectionScope);
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>Hexadecimal representation of the hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey.ToBytes(), plainText.ToBytes(), saltEnabled, dataProtectionScope).ToHexadecimal();
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHashAsBase64<TKeyedHashAlgorithm>(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CreateHash(typeof(TKeyedHashAlgorithm), symmetricKey.ToBytes(), plainText.ToBytes(), saltEnabled, dataProtectionScope).ToBase64();
        }

        private Byte[] CreateHash(Type keyedHashAlgorithm, Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            var keyedHashAlgorithmProvider =
                new KeyedHashAlgorithmProvider(keyedHashAlgorithm,
                    saltEnabled,
                    ProtectedKey.CreateFromPlaintextKey(symmetricKey, dataProtectionScope));

            return keyedHashAlgorithmProvider.CreateHash(plainText);
        }

        #endregion

        #region Keyed-Hash CompareHash Methods

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                symmetricKey,
                plainText,
                hashedText,
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hexadecimalHash">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(String symmetricKey, String plainText, String hexadecimalHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                symmetricKey.ToBytes(),
                plainText.ToBytes(),
                hexadecimalHash.ToBytesFromHexadecimal(),
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="base64EncodedHash">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHashFromBase64(String symmetricKey, String plainText, String base64EncodedHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                symmetricKey.ToBytes(),
                plainText.ToBytes(),
                base64EncodedHash.ToBytesFromBase64(),
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                symmetricKey,
                plainText,
                hashedText,
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hexadecimalHash">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String hexadecimalHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                symmetricKey.ToBytes(),
                plainText.ToBytes(),
                hexadecimalHash.ToBytesFromHexadecimal(),
                saltEnabled,
                dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="base64EncodedHash">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHashFromBase64<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String base64EncodedHash, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                symmetricKey.ToBytes(),
                plainText.ToBytes(),
                base64EncodedHash.ToBytesFromBase64(),
                saltEnabled,
                dataProtectionScope);
        }
        
        private Boolean CompareHash(Type keyedHashAlgorithm, Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            if (symmetricKey == null || plainText == null || hashedText == null)
            {
                return false;
            }

            var keyedHashAlgorithmProvider =
                new KeyedHashAlgorithmProvider(keyedHashAlgorithm,
                    saltEnabled,
                    ProtectedKey.CreateFromPlaintextKey(symmetricKey, dataProtectionScope));

            return keyedHashAlgorithmProvider.CompareHash(plainText, hashedText);
        }

        #endregion
    }
}