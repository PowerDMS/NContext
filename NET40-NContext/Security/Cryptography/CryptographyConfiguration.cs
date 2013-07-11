// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyConfiguration.cs" company="Waking Venture, Inc.">
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
    /// Defines configuration settings for application cryptographic operations.
    /// </summary>
    public class CryptographyConfiguration
    {
        private readonly Type _DefaultHashAlgorithm;

        private readonly Type _DefaultKeyedHashAlgorithm;

        private readonly Type _DefaultSymmetricAlgorithm;

        private readonly Func<IProvideHashing> _HashProviderFactory;

        private readonly Func<IProvideKeyedHashing> _KeyedHashProviderFactory;

        private readonly Func<IProvideSymmetricEncryption> _SymmetricEncryptionProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyConfiguration"/> class.
        /// </summary>
        /// <param name="defaultHashAlgorithm">The default hash algorithm.</param>
        /// <param name="defaultKeyedHashAlgorithm">The default keyed hash algorithm.</param>
        /// <param name="defaultSymmetricAlgorithm">The default symmetric algorithm.</param>
        /// <param name="hashProviderFactory">The hash provider factory.</param>
        /// <param name="keyedHashProviderFactory">The keyed hash provider factory.</param>
        /// <param name="symmetricEncryptionProviderFactory">The symmetric encryption provider factory.</param>
        /// <remarks></remarks>
        public CryptographyConfiguration(Type defaultHashAlgorithm, Type defaultKeyedHashAlgorithm, Type defaultSymmetricAlgorithm, Func<IProvideHashing> hashProviderFactory, Func<IProvideKeyedHashing> keyedHashProviderFactory, Func<IProvideSymmetricEncryption> symmetricEncryptionProviderFactory)
        {
            _DefaultHashAlgorithm = defaultHashAlgorithm;
            _DefaultKeyedHashAlgorithm = defaultKeyedHashAlgorithm;
            _DefaultSymmetricAlgorithm = defaultSymmetricAlgorithm;
            _HashProviderFactory = hashProviderFactory;
            _KeyedHashProviderFactory = keyedHashProviderFactory;
            _SymmetricEncryptionProviderFactory = symmetricEncryptionProviderFactory;
        }

        /// <summary>
        /// Gets the symmetric encryption provider factory.
        /// </summary>
        /// <remarks></remarks>
        public Func<IProvideSymmetricEncryption> SymmetricEncryptionProviderFactory
        {
            get
            {
                return _SymmetricEncryptionProviderFactory;
            }
        }

        /// <summary>
        /// Gets the keyed hash provider factory.
        /// </summary>
        /// <remarks></remarks>
        public Func<IProvideKeyedHashing> KeyedHashProviderFactory
        {
            get
            {
                return _KeyedHashProviderFactory;
            }
        }

        /// <summary>
        /// Gets the hash provider factory.
        /// </summary>
        /// <remarks></remarks>
        public Func<IProvideHashing> HashProviderFactory
        {
            get
            {
                return _HashProviderFactory;
            }
        }

        /// <summary>
        /// Gets the default <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <remarks></remarks>
        public Type DefaultHashAlgorithm
        {
            get
            {
                return _DefaultHashAlgorithm;
            }
        }

        /// <summary>
        /// Gets the default <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <remarks></remarks>
        public Type DefaultKeyedHashAlgorithm
        {
            get
            {
                return _DefaultKeyedHashAlgorithm;
            }
        }

        /// <summary>
        /// Gets the default <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <remarks></remarks>
        public Type DefaultSymmetricAlgorithm
        {
            get
            {
                return _DefaultSymmetricAlgorithm;
            }
        }
    }
}