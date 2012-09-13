// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvideSymmetricEncryption.cs" company="Waking Venture, Inc.">
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

    /// <summary>
    /// Defines cryptographic operations for symmetric cryptography.
    /// </summary>
    /// <remarks></remarks>
    public interface IProvideSymmetricEncryption
    {
        #region Symmetric Encryption Methods

        /// <summary>
        /// Encrypts the text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Encrypts the text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Encrypts the text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/> and returns its hexidecimal representation.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        String Encrypt(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text using the specified key file and default <see cref="SymmetricAlgorithm"/> and returns its hexidecimal representation.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        String Encrypt(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/> and returns its hexidecimal representation.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        String Encrypt<TSymmetricAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Encrypts the text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/> and returns its hexidecimal representation.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The encrypted text.</returns>
        /// <remarks></remarks>
        String Encrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Encrypts the text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/> and returns the result, base64 encoded.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The base64 encoded, encrypted text.</returns>
        /// <remarks></remarks>
        String EncryptToBase64(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text using the specified key file and default <see cref="SymmetricAlgorithm"/> and returns the result, base64 encoded.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The base64 encoded, encrypted text.</returns>
        /// <remarks></remarks>
        String EncryptToBase64(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Encrypts the text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/> and returns the result, base64 encoded.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The base64 encoded, encrypted text.</returns>
        /// <remarks></remarks>
        String EncryptToBase64<TSymmetricAlgorithm>(String symmetricKey, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Encrypts the text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/> and returns the result, base64 encoded.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The base64 encoded, encrypted text.</returns>
        /// <remarks></remarks>
        String EncryptToBase64<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String plainText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

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
        Byte[] Decrypt(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the cipher text using the specified key file and the default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Decrypt(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the cipher text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Decrypts the cipher text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, Byte[] cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Decrypts the (hexadecimal) cipher text using the specified symmetric key and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String Decrypt(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the (hexadecimal) cipher text using the specified key file and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String Decrypt(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the (hexadecimal) cipher text using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String Decrypt<TSymmetricAlgorithm>(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Decrypts the (hexadecimal) cipher text using the specified key file and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String Decrypt<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Decrypts the (base64 encoded) cipher using the specified symmetric key and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String DecryptFromBase64(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the (base64 encoded) specified cipher using the key file and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String DecryptFromBase64(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine);

        /// <summary>
        /// Decrypts the (base64 encoded) cipher using the specified symmetric key and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String DecryptFromBase64<TSymmetricAlgorithm>(String symmetricKey, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        /// <summary>
        /// Decrypts the (base64 encoded) cipher using the specified key file and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="protectedKeyFile">The protected key file.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="dataProtectionScope">The data protection scope.</param>
        /// <returns>The decrypted text.</returns>
        /// <remarks></remarks>
        String DecryptFromBase64<TSymmetricAlgorithm>(FileInfo protectedKeyFile, String cipherText, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
            where TSymmetricAlgorithm : SymmetricAlgorithm;

        #endregion
    }
}