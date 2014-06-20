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
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using NContext.Extensions;

    /// <summary>
    /// Defines a provider for symmetric cryptographic operations.
    /// </summary>
    public class SymmetricEncryptionProvider : IProvideSymmetricEncryption
    {
        private readonly Type _DefaultSymmetricAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricEncryptionProvider"/> class.
        /// </summary>
        /// <param name="defaultSymmetricAlgorithm">The default symmetric algorithm.</param>
        /// <exception cref="System.ArgumentException">DefaultSymmetricAlgorithm is invalid. Must be of type derived from SymmetricAlgorithm.;defaultSymmetricAlgorithm</exception>
        public SymmetricEncryptionProvider(Type defaultSymmetricAlgorithm)
        {
            if (defaultSymmetricAlgorithm == typeof(SymmetricAlgorithm) || !defaultSymmetricAlgorithm.Implements<SymmetricAlgorithm>())
            {
                throw new ArgumentException("DefaultSymmetricAlgorithm is invalid. Must be of type derived from SymmetricAlgorithm.", "defaultSymmetricAlgorithm");
            }

            _DefaultSymmetricAlgorithm = defaultSymmetricAlgorithm;
        }

        /// <summary>
        /// Gets the default symmetric algorithm.
        /// </summary>
        /// <value>The default symmetric algorithm.</value>
        public Type DefaultSymmetricAlgorithm
        {
            get { return _DefaultSymmetricAlgorithm; }
        }

        /// <summary>
        /// Encrypts the <paramref name="plainText" /> using the specified <paramref name="symmetricKey" /> and default <see cref="SymmetricAlgorithm" />.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted byte array.</returns>
        public Byte[] Encrypt(Byte[] symmetricKey, Byte[] plainText)
        {
            return Encrypt(_DefaultSymmetricAlgorithm, symmetricKey, plainText);
        }

        /// <summary>
        /// Encrypts the <paramref name="plainText" /> into the specified <paramref name="cipherText" /> stream using the specified 
        /// <paramref name="symmetricKey" /> and default <see cref="SymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cipherText">The cipher text.</param>
        public void Encrypt(Byte[] symmetricKey, Stream plainText, Stream cipherText)
        {
            Encrypt(_DefaultSymmetricAlgorithm, symmetricKey, plainText, cipherText);
        }

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
        public Task EncryptAsync(Byte[] symmetricKey, Stream plainText, Stream cipherText)
        {
            return EncryptAsync(_DefaultSymmetricAlgorithm, symmetricKey, plainText, cipherText);
        }
#endif

        /// <summary>
        /// Encrypts the text using the specified <paramref name="symmetricKey" /> and <typeparamref name="TSymmetricAlgorithm" />.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted byte array.</returns>
        public Byte[] Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] plainText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new()
        {
            return Encrypt(typeof(TSymmetricAlgorithm), symmetricKey, plainText);
        }

        /// <summary>
        /// Encrypts the <paramref name="plainText" /> into the specified <paramref name="cipherText" /> 
        /// stream using the specified <paramref name="symmetricKey" /> and <typeparamref name="TSymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cipherText">The cipher text.</param>
        public void Encrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Stream plainText, Stream cipherText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new()
        {
            Encrypt(typeof(TSymmetricAlgorithm), symmetricKey, plainText, cipherText);
        }

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
        public Task EncryptAsync<TSymmetricAlgorithm>(Byte[] symmetricKey, Stream plainText, Stream cipherText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new()
        {
            return EncryptAsync(typeof(TSymmetricAlgorithm), symmetricKey, plainText, cipherText);
        }
#endif

        private Byte[] Encrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Byte[] plainText)
        {
            Contract.Requires(symmetricAlgorithm != null);
            Contract.Requires(symmetricKey != null && symmetricKey.Length > 0);
            Contract.Requires(plainText != null && plainText.Length > 0);

            var algorithm = (SymmetricAlgorithm)Activator.CreateInstance(symmetricAlgorithm);
            var ivLength = algorithm.BlockSize / 8;

            algorithm.Key = symmetricKey;
            algorithm.GenerateIV();

            using (var cipherText = new MemoryStream())
            using (var encryptionStream = new CryptoStream(cipherText, algorithm.CreateEncryptor(algorithm.Key, algorithm.IV), CryptoStreamMode.Write))
            {
                cipherText.Write(algorithm.IV, 0, ivLength);
                encryptionStream.Write(plainText, 0, plainText.Length);
                if (!encryptionStream.HasFlushedFinalBlock)
                {
                    encryptionStream.FlushFinalBlock();
                }

                return cipherText.ToArray();
            }
        }

        private void Encrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Stream plainText, Stream cipherText)
        {
            Contract.Requires(symmetricAlgorithm != null);
            Contract.Requires(symmetricKey != null && symmetricKey.Length > 0);
            Contract.Requires(plainText != null);
            Contract.Requires(cipherText != null);

            var algorithm = (SymmetricAlgorithm) Activator.CreateInstance(symmetricAlgorithm);
            var ivLength = algorithm.BlockSize / 8;
            algorithm.Key = symmetricKey;
            algorithm.GenerateIV();

            plainText.Position = 0;
            cipherText.Position = 0;
            cipherText.Write(algorithm.IV, 0, ivLength);

            var encryptionStream = new CryptoStream(cipherText, algorithm.CreateEncryptor(algorithm.Key, algorithm.IV), CryptoStreamMode.Write);
            plainText.CopyTo(encryptionStream);
            if (!encryptionStream.HasFlushedFinalBlock)
            {
                encryptionStream.FlushFinalBlock();
            }
            
            plainText.Position = 0;
            cipherText.Position = 0;
        }

#if NET45_OR_GREATER
        private async Task EncryptAsync(Type symmetricAlgorithm, Byte[] symmetricKey, Stream plainText, Stream cipherText)
        {
            Contract.Requires(symmetricAlgorithm != null);
            Contract.Requires(symmetricKey != null && symmetricKey.Length > 0);
            Contract.Requires(plainText != null);
            Contract.Requires(cipherText != null);

            var algorithm = (SymmetricAlgorithm)Activator.CreateInstance(symmetricAlgorithm);
            var ivLength = algorithm.BlockSize / 8;
            algorithm.Key = symmetricKey;
            algorithm.GenerateIV();

            plainText.Position = 0;
            cipherText.Position = 0;
            cipherText.Write(algorithm.IV, 0, ivLength);

            var encryptionStream = new CryptoStream(cipherText, algorithm.CreateEncryptor(algorithm.Key, algorithm.IV), CryptoStreamMode.Write);

            await plainText.CopyToAsync(encryptionStream);

            if (!encryptionStream.HasFlushedFinalBlock)
            {
                encryptionStream.FlushFinalBlock();
            }

            plainText.Position = 0;
            cipherText.Position = 0;
        }
#endif

        /// <summary>
        /// Decrypts the <paramref name="cipherText" /> using the specified <paramref name="symmetricKey" /> and the default <see cref="SymmetricAlgorithm" />.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted byte array.</returns>
        public Byte[] Decrypt(Byte[] symmetricKey, Byte[] cipherText)
        {
            return Decrypt(_DefaultSymmetricAlgorithm, symmetricKey, cipherText);
        }

        /// <summary>
        /// Decrypts the <paramref name="cipherText" /> into the specified <paramref name="plainText" /> stream using the specified 
        /// <paramref name="symmetricKey" /> and the default <see cref="SymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="plainText">The plain text.</param>
        public void Decrypt(Byte[] symmetricKey, Stream cipherText, Stream plainText)
        {
            Decrypt(_DefaultSymmetricAlgorithm, symmetricKey, cipherText, plainText);
        }

#if NET45_OR_GREATER
        /// <summary>
        /// Decrypts the <paramref name="cipherText" /> into the specified <paramref name="plainText" /> stream using the specified 
        /// <paramref name="symmetricKey" /> and the default <see cref="SymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="plainText">The plain text.</param>
        public Task DecryptAsync(Byte[] symmetricKey, Stream cipherText, Stream plainText)
        {
            return DecryptAsync(_DefaultSymmetricAlgorithm, symmetricKey, cipherText, plainText);
        }
#endif

        /// <summary>
        /// Decrypts the <paramref name="cipherText" /> using the specified <paramref name="symmetricKey" /> and <typeparamref name="TSymmetricAlgorithm" />.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted byte array.</returns>
        public Byte[] Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Byte[] cipherText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new()
        {
            return Decrypt(typeof(TSymmetricAlgorithm), symmetricKey, cipherText);
        }

        /// <summary>
        /// Decrypts the <paramref name="cipherText" /> into the specified <paramref name="plainText" /> stream using the specified 
        /// <paramref name="symmetricKey" /> and <typeparamref name="TSymmetricAlgorithm" />.
        /// Both streams remain unclosed, however, their positions are reset.  It is the caller's responsibility to close and dispose the streams.
        /// </summary>
        /// <typeparam name="TSymmetricAlgorithm">The type of the symmetric algorithm.</typeparam>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="plainText">The plain text.</param>
        public void Decrypt<TSymmetricAlgorithm>(Byte[] symmetricKey, Stream cipherText, Stream plainText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new()
        {
            Decrypt(typeof(TSymmetricAlgorithm), symmetricKey, cipherText, plainText);
        }

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
        public Task DecryptAsync<TSymmetricAlgorithm>(Byte[] symmetricKey, Stream cipherText, Stream plainText)
            where TSymmetricAlgorithm : SymmetricAlgorithm, new()
        {
            return DecryptAsync(typeof(TSymmetricAlgorithm), symmetricKey, cipherText, plainText);
        }
#endif

        private Byte[] Decrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Byte[] cipherText)
        {
            Contract.Requires(symmetricAlgorithm != null);
            Contract.Requires(symmetricKey != null && symmetricKey.Length > 0);
            Contract.Requires(cipherText != null && cipherText.Length > 0);

            var algorithm = (SymmetricAlgorithm)Activator.CreateInstance(symmetricAlgorithm);
            var ivSize = algorithm.BlockSize / 8;
            if (cipherText.Length < ivSize)
            {
                throw new ArgumentException("The cipherText specified is invalid and does not contain the initialization vector.");
            }

            algorithm.Key = symmetricKey;
            algorithm.IV = CryptographyUtility.GetBytes(cipherText, ivSize);
            var cipher = CryptographyUtility.GetBytes(cipherText, cipherText.Length - ivSize, ivSize);

            using (var encryptedStream = new MemoryStream(cipher))
            using (var plainTextStream = new MemoryStream())
            using (var decryptionStream = new CryptoStream(encryptedStream, algorithm.CreateDecryptor(algorithm.Key, algorithm.IV), CryptoStreamMode.Read))
            {
                decryptionStream.CopyTo(plainTextStream);

                return plainTextStream.ToArray();
            }
        }

        private void Decrypt(Type symmetricAlgorithm, Byte[] symmetricKey, Stream cipherText, Stream plainText)
        {
            Contract.Requires(symmetricAlgorithm != null);
            Contract.Requires(symmetricKey != null && symmetricKey.Length > 0);
            Contract.Requires(cipherText != null);
            Contract.Requires(plainText != null);

            var algorithm = (SymmetricAlgorithm) Activator.CreateInstance(symmetricAlgorithm);
            var ivSize = algorithm.BlockSize / 8;
            if (cipherText.Length < ivSize)
            {
                throw new ArgumentException(
                    "The cipherText specified is invalid and does not contain the initialization vector.");
            }

            cipherText.Position = 0;
            plainText.Position = 0;

            algorithm.Key = symmetricKey;
            algorithm.IV = CryptographyUtility.GetBytes(cipherText, ivSize);

            cipherText.Position = ivSize;

            var decryptionStream = new CryptoStream(cipherText, algorithm.CreateDecryptor(algorithm.Key, algorithm.IV), CryptoStreamMode.Read);
            decryptionStream.CopyTo(plainText);

            cipherText.Position = 0;
            plainText.Position = 0;
        }

#if NET45_OR_GREATER
        private async Task DecryptAsync(Type symmetricAlgorithm, Byte[] symmetricKey, Stream cipherText, Stream plainText)
        {
            Contract.Requires(symmetricAlgorithm != null);
            Contract.Requires(symmetricKey != null && symmetricKey.Length > 0);
            Contract.Requires(cipherText != null);
            Contract.Requires(plainText != null);

            var algorithm = (SymmetricAlgorithm)Activator.CreateInstance(symmetricAlgorithm);
            var ivSize = algorithm.BlockSize / 8;
            if (cipherText.Length < ivSize)
            {
                throw new ArgumentException("The cipherText specified is invalid and does not contain the initialization vector.");
            }

            cipherText.Position = 0;
            plainText.Position = 0;

            algorithm.Key = symmetricKey;
            algorithm.IV = CryptographyUtility.GetBytes(cipherText, ivSize);

            cipherText.Position = ivSize;

            var decryptionStream = new CryptoStream(cipherText, algorithm.CreateDecryptor(algorithm.Key, algorithm.IV), CryptoStreamMode.Read);
            
            await decryptionStream.CopyToAsync(plainText);

            cipherText.Position = 0;
            plainText.Position = 0;
        }
#endif
    }
}