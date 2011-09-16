// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICryptographyProvider.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Defines cryptographic operations for hashing and symmetric encryption.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;

namespace NContext.Application.Security.Cryptography
{
    /// <summary>
    /// Defines cryptographic operations for hashing and symmetric encryption.
    /// </summary>
    public interface ICryptographyProvider
    {
        #region Hash CreateHash Methods

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        Byte[] CreateHash(Byte[] plainText, Boolean saltEnabled = true);

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHash(String plainText, Boolean saltEnabled = true);

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        Byte[] CreateHash<THashAlgorithm>(Byte[] plainText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm;

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHash<THashAlgorithm>(String plainText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm;

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
        Boolean CompareHash(Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true);

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash(String plainText, String hashedText, Boolean saltEnabled = true);

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash<THashAlgorithm>(Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm;

        /// <summary>
        /// Compares the hash.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltEnabled">if set to <c>true</c> [salt enabled].</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash<THashAlgorithm>(String plainText, String hashedText, Boolean saltEnabled = true)
            where THashAlgorithm : HashAlgorithm;

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
        Byte[] CreateHash(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Byte[] CreateHash(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Byte[] CreateHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        Byte[] CreateHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

        /// <summary>
        /// Creates the <see cref="HMAC"/> hash with the specified symmetric key 
        /// using the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        String CreateHash(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        String CreateHash(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        String CreateHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        String CreateHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

        /// <summary>
        /// Creates a URL-safe Base64-encoded <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        String CreateHmac(String symmetricKey, String plainText);

        /// <summary>
        /// Creates a URL-safe Base64-encoded <see cref="HMAC"/> hash with the specified symmetric key 
        /// and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The hashed message authentication code. (<see cref="HMAC"/>)</returns>
        /// <remarks></remarks>
        String CreateHmac<TKeyedHashAlgorithm>(String symmetricKey, String plainText)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        Boolean CompareHash(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Boolean CompareHash(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Compares the hashes using the specified symmetric key and default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash(String symmetricKey, String plainText, String hashedText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Boolean CompareHash(String symmetricKey, String plainText, String hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Boolean CompareHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Boolean CompareHash<TKeyedHashAlgorithm>(Byte[] symmetricKey, Byte[] plainText, Byte[] hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Boolean CompareHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String hashedText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Boolean CompareHash<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String hashedText, Boolean saltEnabled = true, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Compares the hashes using the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="base64EncodedHashedText">The base64-encoded and hashed text.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHmac(String symmetricKey, String plainText, String base64EncodedHashedText);

        /// <summary>
        /// Compares the hashes using the specified symmetric key and <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TKeyedHashAlgorithm">The type of the keyed hash algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="base64EncodedHashedText">The base64-encoded and hashed text.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHmac<TKeyedHashAlgorithm>(String symmetricKey, String plainText, String base64EncodedHashedText)
            where TKeyedHashAlgorithm : KeyedHashAlgorithm;

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
        Byte[] Encrypt(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        String Encrypt(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the specified text using the application's
        /// default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        String Encrypt(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        String Encrypt<TSymmetricAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text with the specified <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        String Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

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
        Byte[] Decrypt(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the specified symmetric key.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String Decrypt(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Decrypt(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String Decrypt(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String Decrypt<TSymmetricAlgorithm>(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        #endregion
    }
}