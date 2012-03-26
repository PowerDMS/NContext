// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyManager.cs">
//   Copyright (c) 2012
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
//   Defines an interface for application cryptography-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;

using NContext.Configuration;

namespace NContext.Security.Cryptography
{
    /// <summary>
    /// Defines an interface for application-wide cryptographic management.
    /// </summary>
    /// <remarks></remarks>
    public interface IManageCryptography : IApplicationComponent
    {
        /// <summary>
        /// Gets the hash provider.
        /// </summary>
        /// <remarks></remarks>
        IProvideHashing HashProvider { get; }

        /// <summary>
        /// Gets the keyed hash provider.
        /// </summary>
        /// <remarks></remarks>
        IProvideKeyedHashing KeyedHashProvider { get; }

        /// <summary>
        /// Gets the symmetric encryption provider.
        /// </summary>
        /// <remarks></remarks>
        IProvideSymmetricEncryption SymmetricEncryptionProvider { get; }

        /// <summary>
        /// Gets or sets the default symmetric algorithm. Default is <see cref="AesManaged"/>.
        /// </summary>
        /// <value>The default symmetric algorithm.</value>
        /// <remarks></remarks>
        Type DefaultSymmetricAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the default keyed hash algorithm. Default is <see cref="HMACSHA256"/>.
        /// </summary>
        /// <value>The default keyed hash algorithm.</value>
        /// <remarks></remarks>
        Type DefaultKeyedHashAlgorithm { get; set; }

        /// <summary>
        /// Gets the default hash algorithm. Default is <see cref="SHA256Managed"/>.
        /// </summary>
        /// <remarks></remarks>
        Type DefaultHashAlgorithm { get; set; }
    }
}