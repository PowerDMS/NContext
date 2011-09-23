// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyProvider.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines a crytographic provider using Enterprise Library.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using NContext.Application.Extensions;

using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace NContext.Application.Security.Cryptography
{
    /// <summary>
    /// Defines a crytographic provider using Enterprise Library.
    /// </summary>
    public class CryptographyProvider : ICryptographyProvider
    {
        #region Fields

        private readonly Type _DefaultHashAlgorithm;

        private readonly Type _DefaultKeyedHashAlgorithm;

        private readonly Type _DefaultSymmetricAlgorithm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class using 
        /// a default set of recommended cryptography algorithms.
        /// </summary>
        /// <remarks></remarks>
        public CryptographyProvider()
            : this(typeof(SHA256Managed), typeof(HMACSHA256), typeof(AesManaged))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyProvider"/> class.
        /// </summary>
        /// <param name="defaultHashAlgorithm">The default hash algorithm.</param>
        /// <param name="defaultKeyedHashAlgorithm">The default keyed hash algorithm.</param>
        /// <param name="defaultSymmetricAlgorithm">The default symmetric algorithm.</param>
        /// <remarks></remarks>
        public CryptographyProvider(Type defaultHashAlgorithm, Type defaultKeyedHashAlgorithm, Type defaultSymmetricAlgorithm)
        {
            if (!defaultHashAlgorithm.Implements<HashAlgorithm>())
            {
                throw new ArgumentException("DefaultHashAlgorithm is invalid. Must be of type HashAlgorithm.", "defaultHashAlgorithm");
            }

            if (!defaultKeyedHashAlgorithm.Implements<KeyedHashAlgorithm>())
            {
                throw new ArgumentException("DefaultKeyedHashAlgorithm is invalid. Must be of type KeyedHashAlgorithm.", "defaultKeyedHashAlgorithm");
            }

            if (!defaultSymmetricAlgorithm.Implements<SymmetricAlgorithm>())
            {
                throw new ArgumentException("DefaultSymmetricAlgorithm is invalid. Must be of type SymmetricAlgorithm.", "defaultSymmetricAlgorithm");
            }

            _DefaultHashAlgorithm = defaultHashAlgorithm;
            _DefaultKeyedHashAlgorithm = defaultKeyedHashAlgorithm;
            _DefaultSymmetricAlgorithm = defaultSymmetricAlgorithm;
        }

        #endregion

        #region Hash CreateHash Methods

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
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
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public String CreateHash(String plainText, Boolean saltEnabled = true)
        {
            return Encoding.UTF8.GetString(CreateHash(_DefaultHashAlgorithm, Encoding.UTF8.GetBytes(plainText), saltEnabled));
        }

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
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
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        public String CreateHash<THashAlgorithm>(String plainText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm
        {
            return Encoding.UTF8.GetString(CreateHash(typeof(THashAlgorithm), Encoding.UTF8.GetBytes(plainText), saltEnabled));
        }

        private Byte[] CreateHash(Type keyedHashAlgorithm, Byte[] plainText, Boolean saltEnabled = true)
        {
            var hashAlgorithmProvider =
                new HashAlgorithmProvider(keyedHashAlgorithm, saltEnabled);

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
            return CompareHash(_DefaultHashAlgorithm, Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(hashedText));
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
            return CompareHash(typeof(THashAlgorithm), Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(hashedText));
        }

        private Boolean CompareHash(Type hashAlgorithm, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true)
        {
            var hashAlgorithmProvider =
                new HashAlgorithmProvider(hashAlgorithm, saltEnabled);

            return hashAlgorithmProvider.CompareHash(plainText, hashedText);
        }

        #endregion

        #region Keyed-Hash CreateHash Methods

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// using the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey, plainText, true, dataProtectionScope);
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
        public Byte[] CreateHash(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CreateHash(_DefaultKeyedHashAlgorithm, symmetricKey, plainText, saltEnabled, dataProtectionScope);
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public Byte[] CreateHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CreateHash(typeof(TKeyedHashAlgorithm), symmetricKey, plainText, true, dataProtectionScope);
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
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// using the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHash(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(CreateHash(_DefaultKeyedHashAlgorithm, Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), true, dataProtectionScope));
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
        public String CreateHash(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(CreateHash(_DefaultKeyedHashAlgorithm, Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), saltEnabled, dataProtectionScope));
        }

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return Encoding.UTF8.GetString(CreateHash(typeof(TKeyedHashAlgorithm), Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), true, dataProtectionScope));
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
        public String CreateHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return Encoding.UTF8.GetString(CreateHash(typeof(TKeyedHashAlgorithm), Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), saltEnabled, dataProtectionScope));
        }

        /// <summary>
        /// Creates a URL-safe Base64-encoded <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHmac(String symmetricKey, String plainText)
        {
            return Convert.ToBase64String(CreateHash(_DefaultKeyedHashAlgorithm, Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), false));
        }

        /// <summary>
        /// Creates a URL-safe Base64-encoded <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        public String CreateHmac<TKeyedHashAlgorithm>(String symmetricKey, String plainText)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return Convert.ToBase64String(CreateHash(typeof(TKeyedHashAlgorithm), Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), false));
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
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                               symmetricKey,
                               plainText,
                               hashedText,
                               true,
                               dataProtectionScope);
        }

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
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash(String symmetricKey, String plainText, String hashedText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                               Encoding.UTF8.GetBytes(symmetricKey),
                               Encoding.UTF8.GetBytes(plainText),
                               Encoding.UTF8.GetBytes(hashedText),
                               true,
                               dataProtectionScope);
        }

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
        public Boolean CompareHash(String symmetricKey, String plainText, String hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm,
                               Encoding.UTF8.GetBytes(symmetricKey),
                               Encoding.UTF8.GetBytes(plainText),
                               Encoding.UTF8.GetBytes(hashedText),
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
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                               symmetricKey,
                               plainText,
                               hashedText,
                               true,
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
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                               symmetricKey,
                               plainText,
                               hashedText,
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
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String hashedText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                               Encoding.UTF8.GetBytes(symmetricKey),
                               Encoding.UTF8.GetBytes(plainText),
                               Encoding.UTF8.GetBytes(hashedText),
                               true,
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
        public Boolean CompareHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return CompareHash(typeof(TKeyedHashAlgorithm),
                               Encoding.UTF8.GetBytes(symmetricKey),
                               Encoding.UTF8.GetBytes(plainText),
                               Encoding.UTF8.GetBytes(hashedText),
                               saltEnabled,
                               dataProtectionScope);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="base64EncodedHashedText">The base64-encoded and hashed text.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHmac(String symmetricKey, String plainText, String base64EncodedHashedText)
        {
            return CompareHash(_DefaultKeyedHashAlgorithm, Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), Convert.FromBase64String(base64EncodedHashedText), false);
        }

        /// <summary>
        /// Compares the hashes using the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="base64EncodedHashedText">The base64-encoded and hashed text.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        public Boolean CompareHmac<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String base64EncodedHashedText)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm
        {
            return CompareHash(typeof(TKeyedHashAlgorithm), Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), Convert.FromBase64String(base64EncodedHashedText), false);
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

        #region Symmetric Encryption Methods

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, symmetricKey, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String Encrypt(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Encrypt(_DefaultSymmetricAlgorithm, Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), dataProtectionScope));
        }

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String Encrypt(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Encrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, Encoding.UTF8.GetBytes(plainText), dataProtectionScope));
        }

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(typeof(TSymmetricAlgorithm), symmetricKey, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String Encrypt<TSymmetricAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Encrypt(typeof(TSymmetricAlgorithm), Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(plainText), dataProtectionScope));
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Encrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, Encoding.UTF8.GetBytes(plainText), dataProtectionScope));
        }

        /// <summary>
        /// Encrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Encrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Byte[] plaintext, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(symmetricAlgorithm, ProtectedKey.CreateFromPlaintextKey(symmetricKey, dataProtectionScope), plaintext);
        }

        /// <summary>
        /// Encrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="protectedKeyFileName">Name of the protected key file.</param>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Encrypt(Type symmetricAlgorithm, String protectedKeyFileName, Byte[] plaintext, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(symmetricAlgorithm, KeyManager.Read(protectedKeyFileName, dataProtectionScope), plaintext);
        }

        /// <summary>
        /// Encrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="protectedKey">The protected key.</param>
        /// <param name="plaintext">The plaintext.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Encrypt(Type symmetricAlgorithm, ProtectedKey protectedKey, Byte[] plaintext)
        {
            var symmetricProvider = new SymmetricAlgorithmProvider(
                symmetricAlgorithm, protectedKey);

            return symmetricProvider.Encrypt(plaintext);
        }

        #endregion

        #region Symmetric Decryption Methods

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, symmetricKey, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the specified symmetric key.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String Decrypt(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Decrypt(_DefaultSymmetricAlgorithm, Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(cipherText), dataProtectionScope));
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String Decrypt(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Decrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, Encoding.UTF8.GetBytes(cipherText), dataProtectionScope));
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(typeof(TSymmetricAlgorithm), symmetricKey, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String Decrypt<TSymmetricAlgorithm>(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Decrypt(typeof(TSymmetricAlgorithm), Encoding.UTF8.GetBytes(symmetricKey), Encoding.UTF8.GetBytes(cipherText), dataProtectionScope));
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encoding.UTF8.GetString(Decrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, Encoding.UTF8.GetBytes(cipherText), dataProtectionScope));
        }

        /// <summary>
        /// Decrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Decrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(symmetricAlgorithm, ProtectedKey.CreateFromPlaintextKey(symmetricKey, dataProtectionScope), cipherText);
        }

        /// <summary>
        /// Decrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="protectedKeyFileName">Name of the protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Decrypt(Type symmetricAlgorithm, String protectedKeyFileName, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(symmetricAlgorithm, KeyManager.Read(protectedKeyFileName, dataProtectionScope), cipherText);
        }

        /// <summary>
        /// Decrypts the specified symmetric algorithm.
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm.</param>
        /// <param name="protectedKey">The protected key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Byte[] Decrypt(Type symmetricAlgorithm, ProtectedKey protectedKey, Byte[] cipherText)
        {
            var symmetricProvider = new SymmetricAlgorithmProvider(
                symmetricAlgorithm, protectedKey);

            return symmetricProvider.Decrypt(cipherText);
        }

        #endregion
    }
}