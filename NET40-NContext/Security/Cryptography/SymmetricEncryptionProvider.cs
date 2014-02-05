// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SymmetricEncryptionProvider.cs" company="Waking Venture, Inc.">
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
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    using NContext.Extensions;

    /// <summary>
    /// Defines a provider for symmetric cryptographic operations.
    /// </summary>
    /// <remarks></remarks>
    public class SymmetricEncryptionProvider : IProvideSymmetricEncryption
    {
        private readonly Type _DefaultSymmetricAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricEncryptionProvider"/> class.
        /// </summary>
        /// <param name="defaultSymmetricAlgorithm">The default symmetric algorithm.</param>
        /// <remarks></remarks>
        public SymmetricEncryptionProvider(Type defaultSymmetricAlgorithm)
        {
            if (!defaultSymmetricAlgorithm.Implements<SymmetricAlgorithm>())
            {
                throw new ArgumentException("DefaultSymmetricAlgorithm is invalid. Must be of type SymmetricAlgorithm.", "defaultSymmetricAlgorithm");
            }

            _DefaultSymmetricAlgorithm = defaultSymmetricAlgorithm;
        }

        #region Symmetric Encryption Methods

        /// <summary>
        /// Encrypts the text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/>.
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
        /// Encrypts the text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/>.
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
        /// Encrypts the text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Encrypt(typeof(TSymmetricAlgorithm), symmetricKey, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Encrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, plainText, dataProtectionScope);
        }

        /// <summary>
        /// Encrypts the text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/> and returns its hexidecimal representation.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String Encrypt(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, symmetricKey.ToBytes<UnicodeEncoding>(), plainText.ToBytes<UnicodeEncoding>(), dataProtectionScope).ToHexadecimal();
        }

        /// <summary>
        /// Encrypts the text using the specified key file and default <see cref="SymmetricAlgorithm"/> and returns its hexidecimal representation.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String Encrypt(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, plainText.ToBytes<UnicodeEncoding>(), dataProtectionScope).ToHexadecimal();
        }

        /// <summary>
        /// Encrypts the text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/> and returns its hexidecimal representation.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String Encrypt<TSymmetricAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Encrypt(typeof(TSymmetricAlgorithm), symmetricKey.ToBytes<UnicodeEncoding>(), plainText.ToBytes<UnicodeEncoding>(), dataProtectionScope).ToHexadecimal();
        }

        /// <summary>
        /// Encrypts the text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/> and returns its hexidecimal representation.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        public String Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Encrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, plainText.ToBytes<UnicodeEncoding>(), dataProtectionScope).ToHexadecimal();
        }

        /// <summary>
        /// Encrypts the text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/> and returns the result, base64 encoded.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The base64 encoded, encrypted text.</returns>
        /// <remarks></remarks>
        public String EncryptToBase64(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, symmetricKey.ToBytes<UnicodeEncoding>(), plainText.ToBytes<UnicodeEncoding>(), dataProtectionScope).ToBase64();
        }

        /// <summary>
        /// Encrypts the text using the specified key file and default <see cref="SymmetricAlgorithm"/> and returns the result, base64 encoded.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The base64 encoded, encrypted text.</returns>
        /// <remarks></remarks>
        public String EncryptToBase64(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, plainText.ToBytes<UnicodeEncoding>(), dataProtectionScope).ToBase64();
        }

        /// <summary>
        /// Encrypts the text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/> and returns the result, base64 encoded.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The base64 encoded, encrypted text.</returns>
        /// <remarks></remarks>
        public String EncryptToBase64<TSymmetricAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Encrypt(typeof(TSymmetricAlgorithm), symmetricKey.ToBytes<UnicodeEncoding>(), plainText.ToBytes<UnicodeEncoding>(), dataProtectionScope).ToBase64();
        }

        /// <summary>
        /// Encrypts the text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/> and returns the result, base64 encoded.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The base64 encoded, encrypted text.</returns>
        /// <remarks></remarks>
        public String EncryptToBase64<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Encrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, plainText.ToBytes<UnicodeEncoding>(), dataProtectionScope).ToBase64();
        }

        private Byte[] Encrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            // TODO: (DG) IMPLEMENT!
            throw new NotImplementedException();
        }

        private Byte[] Encrypt(Type symmetricAlgorithm, String protectedKeyFileName, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            // TODO: (DG) IMPLEMENT!
            throw new NotImplementedException();
        }

        #endregion

        #region Symmetric Decryption Methods

        /// <summary>
        /// Decrypts the cipher text using the specified symmetric key and the default <see cref="SymmetricAlgorithm"/>.
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
        /// Decrypts the cipher text using the specified key file and the default <see cref="SymmetricAlgorithm"/>.
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
        /// Decrypts the cipher text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Decrypt(typeof(TSymmetricAlgorithm), symmetricKey, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the cipher text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        public Byte[] Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Decrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, cipherText, dataProtectionScope);
        }

        /// <summary>
        /// Decrypts the (hexadecimal) cipher text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String Decrypt(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, symmetricKey.ToBytes<UnicodeEncoding>(), cipherText.ToBytesFromHexadecimal(), dataProtectionScope).ToUTF8();
        }

        /// <summary>
        /// Decrypts the (hexadecimal) cipher text using the specified key file and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String Decrypt(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, cipherText.ToBytesFromHexadecimal(), dataProtectionScope).ToUTF8();
        }

        /// <summary>
        /// Decrypts the (hexadecimal) cipher text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String Decrypt<TSymmetricAlgorithm>(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Decrypt(typeof(TSymmetricAlgorithm), symmetricKey.ToBytes<UnicodeEncoding>(), cipherText.ToBytesFromHexadecimal(), dataProtectionScope).ToUTF8();
        }

        /// <summary>
        /// Decrypts the (hexadecimal) cipher text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Decrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, cipherText.ToBytesFromHexadecimal(), dataProtectionScope).ToUTF8();
        }

        /// <summary>
        /// Decrypts the (base64 encoded) cipher using the specified symmetric key and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String DecryptFromBase64(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, symmetricKey.ToBytes<UnicodeEncoding>(), cipherText.ToBytesFromBase64(), dataProtectionScope).ToUTF8();
        }

        /// <summary>
        /// Decrypts the (base64 encoded) specified cipher using the key file and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String DecryptFromBase64(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, protectedKeyFile.FullName, cipherText.ToBytesFromBase64(), dataProtectionScope).ToUTF8();
        }

        /// <summary>
        /// Decrypts the (base64 encoded) cipher using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String DecryptFromBase64<TSymmetricAlgorithm>(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Decrypt(typeof(TSymmetricAlgorithm), symmetricKey.ToBytes<UnicodeEncoding>(), cipherText.ToBytesFromBase64(), dataProtectionScope).ToUTF8();
        }

        /// <summary>
        /// Decrypts the (base64 encoded) cipher using the specified key file and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        public String DecryptFromBase64<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm
        {
            return Decrypt(typeof(TSymmetricAlgorithm), protectedKeyFile.FullName, cipherText.ToBytesFromBase64(), dataProtectionScope).ToUTF8();
        }

        private Byte[] Decrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            // TODO: IMPLEMENT!
            throw new NotImplementedException();
        }

        private Byte[] Decrypt(Type symmetricAlgorithm, String protectedKeyFileName, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
        {
            // TODO: IMPLEMENT!
            throw new NotImplementedException();
        }

        #endregion
    }
}