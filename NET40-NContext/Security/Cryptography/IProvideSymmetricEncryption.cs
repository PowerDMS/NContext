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
    using System.Threading.Tasks;

    /// <summary>
    /// Defines cryptographic operations for symmetric cryptography.
    /// </summary>
    /// <remarks></remarks>
    public interface IProvideSymmetricEncryption
    {
        /// <summary>
        /// Gets the default symmetric algorithm.
        /// </summary>
        /// <value>The default symmetric algorithm.</value>
        Type DefaultSymmetricAlgorithm { get; }

        /// <summary>
        /// Encrypts the <paramref name="plainText"/> using the specified <paramref name="symmetricKey"/> and default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt(Byte[] symmetricKey, Byte[] plainText);

        /// <summary>
        /// Encrypts the <paramref name="plainText"/> into the specified <paramref name="cipherText"/> stream using the specified <paramref name="symmetricKey"/> and default <see cref="SymmetricAlgorithm"/>.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cipherText">The cipher text.</param>
        void Encrypt(Byte[] symmetricKey, Stream plainText, Stream cipherText);

#if NET45_OR_GREATER
        /// <summary>
        /// Encrypts the <paramref name="plainText" /> into the specified <paramref name="cipherText" /> stream using the specified 
        /// <paramref name="symmetricKey" /> and default <see cref="SymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>Task.</returns>
        Task EncryptAsync(Byte[] symmetricKey, Stream plainText, Stream cipherText);
#endif

        /// <summary>
        /// Encrypts the text using the specified <paramref name="symmetricKey"/> and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] plainText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new();

        /// <summary>
        /// Encrypts the <paramref name="plainText"/> into the specified <paramref name="cipherText"/> stream using the specified <paramref name="symmetricKey"/> and <typeparamref name="TSymmetricAlgorithm"/>.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cipherText">The cipher text.</param>
        void Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Stream plainText, Stream cipherText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new();

#if NET45_OR_GREATER
        /// <summary>
        /// Encrypts the <paramref name="plainText" /> into the specified <paramref name="cipherText" /> 
        /// stream using the specified <paramref name="symmetricKey" /> and <typeparamref name="TSymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cipherText">The cipher text.</param>
        Task EncryptAsync<TSymmetricAlgorithm>(Byte[] symmetricKey, Stream plainText, Stream cipherText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new();
#endif

        /// <summary>
        /// Decrypts the <paramref name="cipherText"/> using the specified <paramref name="symmetricKey"/> and the default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Decrypt(Byte[] symmetricKey, Byte[] cipherText);

        /// <summary>
        /// Decrypts the <paramref name="cipherText"/> into the specified <paramref name="plainText"/> stream using the specified <paramref name="symmetricKey"/> and the default <see cref="SymmetricAlgorithm"/>.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="plainText">The plain text.</param>
        void Decrypt(Byte[] symmetricKey, Stream cipherText, Stream plainText);

#if NET45_OR_GREATER
        /// <summary>
        /// Decrypts the <paramref name="cipherText" /> into the specified <paramref name="plainText" /> stream using the specified 
        /// <paramref name="symmetricKey" /> and the default <see cref="SymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="plainText">The plain text.</param>
        Task DecryptAsync(Byte[] symmetricKey, Stream cipherText, Stream plainText);
#endif

        /// <summary>
        /// Decrypts the <paramref name="cipherText"/> using the specified <paramref name="symmetricKey"/> and <typeparamref name="TSymmetricAlgorithm"/>.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted byte array.</returns>
        /// <remarks></remarks>
        Byte[] Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] cipherText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new();

        /// <summary>
        /// Decrypts the <paramref name="cipherText"/> into the specified <paramref name="plainText"/> stream using the specified <paramref name="symmetricKey"/> and <typeparamref name="TSymmetricAlgorithm"/>.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="plainText">The plain text.</param>
        void Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Stream cipherText, Stream plainText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new();

#if NET45_OR_GREATER
        /// <summary>
        /// Decrypts the <paramref name="cipherText" /> into the specified <paramref name="plainText" /> stream using the specified 
        /// <paramref name="symmetricKey" /> and <typeparamref name="TSymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="plainText">The plain text.</param>
        Task DecryptAsync<TSymmetricAlgorithm>(Byte[] symmetricKey, Stream cipherText, Stream plainText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new();
#endif
    }
}