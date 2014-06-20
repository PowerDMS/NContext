// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvideHashing.cs" company="Waking Venture, Inc.">
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

    /// <summary>
    /// Defines cryptographic operations for hashing.
    /// </summary>
    /// <remarks></remarks>
    public interface IProvideHashing
    {
        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        Byte[] CreateHash(Byte[] plainText, Int32 saltLength = 16);

        /// <summary>
        /// Creates the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHash(String plainText, Int32 saltLength = 16);

        /// <summary>
        /// Creates the base64 encoded hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHashToBase64(String plainText, Int32 saltLength = 16);

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        Byte[] CreateHash<THashAlgorithm>(Byte[] plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Creates the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHash<THashAlgorithm>(String plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Creates the base64 encoded hash using the specified <typeparamref name="THashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="saltLength">The length of bytes to use as salt.</param>
        /// <returns>The created hash.</returns>
        /// <remarks></remarks>
        String CreateHashToBase64<THashAlgorithm>(String plainText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash(Byte[] plainText, Byte[] hashedText, Int32 saltLength = 16);

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash(String plainText, String hashedText, Int32 saltLength = 16);

        /// <summary>
        /// Compares the hash using the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHashFromBase64(String plainText, String hashedText, Int32 saltLength = 16);

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash<THashAlgorithm>(Byte[] plainText, Byte[] hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHash<THashAlgorithm>(String plainText, String hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();

        /// <summary>
        /// Compares the hash using the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type of the hash algorithm.</typeparam>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <param name="saltLength">The length of bytes used for salt in the <paramref name="hashedText"/>.</param>
        /// <returns><c>true</c> if the hashes match, else <c>false</c></returns>
        /// <remarks></remarks>
        Boolean CompareHashFromBase64<THashAlgorithm>(String plainText, String hashedText, Int32 saltLength = 16)
            where THashAlgorithm : HashAlgorithm, new();
    }
}