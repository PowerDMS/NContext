// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyConfigurationBuilder.cs">
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
//   Defines a configuration class to build the application's CryptographyManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;

using NContext.Configuration;
using NContext.Security.Cryptography;

namespace NContext.Extensions.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Defines a configuration class to build the application's <see cref="CryptographyManager"/>.
    /// </summary>
    public class CryptographyConfigurationBuilder : ApplicationComponentConfigurationBase
    {
        #region Fields

        private Type _DefaultHashAlgorithm;

        private Type _DefaultKeyedHashAlgorithm;

        private Type _DefaultSymmetricAlgorithm;

        private Func<IProvideHashing> _HashProviderFactory;

        private Func<IProvideKeyedHashing> _KeyedHashProviderFactory;

        private Func<IProvideSymmetricEncryption> _SymmetricEncryptionProviderFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        /// <summary>
        /// Sets the application's default cryptographic algorithms. (<see cref="HashAlgorithm"/>, <see cref="KeyedHashAlgorithm"/>, <see cref="SymmetricAlgorithm"/>)
        /// </summary>
        /// <param name="defaultHashAlgorithm">The default <see cref="HashAlgorithm"/>.</param>
        /// <param name="defaultKeyedHashAlgorithm">The default <see cref="KeyedHashAlgorithm"/>.</param>
        /// <param name="defaultSymmetricAlgorithm">The default <see cref="SymmetricAlgorithm"/>.</param>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder SetDefaults(Type defaultHashAlgorithm, Type defaultKeyedHashAlgorithm, Type defaultSymmetricAlgorithm)
        {
            if (!defaultHashAlgorithm.Implements<HashAlgorithm>())
            {
                throw new ArgumentException("DefaultHashAlgorithm is invalid. Must be of type HashAlgorithm.", "defaultHashAlgorithm");
            }

            if (!defaultKeyedHashAlgorithm.Implements<KeyedHashAlgorithm>())
            {
                throw new ArgumentException("DefaultKeyedHashAlgorithm is invalid. Must be of type HashAlgorithm.", "defaultKeyedHashAlgorithm");
            }

            if (!defaultSymmetricAlgorithm.Implements<SymmetricAlgorithm>())
            {
                throw new ArgumentException("DefaultSymmetricAlgorithm is invalid. Must be of type HashAlgorithm.", "defaultSymmetricAlgorithm");
            }

            _DefaultHashAlgorithm = defaultHashAlgorithm;
            _DefaultKeyedHashAlgorithm = defaultKeyedHashAlgorithm;
            _DefaultSymmetricAlgorithm = defaultSymmetricAlgorithm;

            return this;
        }

        /// <summary>
        /// Sets the application's hash provider.
        /// </summary>
        /// <param name="hashProviderFactory">The hash provider factory.</param>
        /// <returns>Current <see cref="CryptographyConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder SetHashProvider(Func<IProvideHashing> hashProviderFactory)
        {
            _HashProviderFactory = hashProviderFactory;

            return this;
        }

        /// <summary>
        /// Sets the application's keyed hash provider.
        /// </summary>
        /// <param name="keyedHashProviderFactory">The keyed hash provider factory.</param>
        /// <returns>Current <see cref="CryptographyConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder SetKeyedHashProvider(Func<IProvideKeyedHashing> keyedHashProviderFactory)
        {
            _KeyedHashProviderFactory = keyedHashProviderFactory;

            return this;
        }

        /// <summary>
        /// Sets the application's symmetric encryption provider.
        /// </summary>
        /// <param name="symmetricEncryptionProvider">The symmetric encryption provider.</param>
        /// <returns>Current <see cref="CryptographyConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CryptographyConfigurationBuilder SetSymmetricEncryptionProvider(Func<IProvideSymmetricEncryption> symmetricEncryptionProvider)
        {
            _SymmetricEncryptionProviderFactory = symmetricEncryptionProvider;

            return this;
        }

        #endregion

        #region Implementation of ApplicationComponentConfigurationBase

        /// <summary>
        /// Sets the application's cryptography manager using 
        /// the configuration created from this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageCryptography>(() => new CryptographyManager(this));
        }

        #endregion
    }
}