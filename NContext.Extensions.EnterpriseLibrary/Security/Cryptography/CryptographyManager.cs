// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyManager.cs">
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
//   Defines manager class for application cryptography-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;

using NContext.Configuration;
using NContext.Security.Cryptography;

namespace NContext.Extensions.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Defines manager class for application cryptography-related operations.
    /// </summary>
    /// <remarks></remarks>
    public class CryptographyManager : IManageCryptography
    {
        #region Fields

        // TODO: (DG) Add support for default symmetric key and hash key.

        private readonly CryptographyConfigurationBuilder _CryptographyConfiguration;

        private Type _DefaultHashAlgorithm;

        private Type _DefaultKeyedHashAlgorithm;

        private Type _DefaultSymmetricAlgorithm;

        private Lazy<IProvideHashing> _HashProvider;

        private Lazy<IProvideKeyedHashing> _KeyedHashProvider;

        private Lazy<IProvideSymmetricEncryption> _SymmetricEncryptionProvider;

        private Boolean _IsConfigured;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyManager"/> class.
        /// </summary>
        /// <param name="cryptographyConfiguration">The cryptography configuration.</param>
        /// <remarks></remarks>
        public CryptographyManager(CryptographyConfigurationBuilder cryptographyConfiguration)
        {
            if (cryptographyConfiguration == null)
            {
                throw new ArgumentNullException("cryptographyConfiguration");
            }

            _CryptographyConfiguration = cryptographyConfiguration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
        }

        /// <summary>
        /// Gets or sets the default symmetric algorithm. Default is <see cref="AesManaged"/>.
        /// </summary>
        /// <value>The default symmetric algorithm.</value>
        /// <remarks></remarks>
        public Type DefaultSymmetricAlgorithm
        {
            get
            {
                return _DefaultSymmetricAlgorithm ?? typeof(AesManaged);
            }
            set
            {
                _DefaultSymmetricAlgorithm = value;
            }
        }

        /// <summary>
        /// Gets or sets the default keyed hash algorithm. Default is <see cref="HMACSHA256"/>.
        /// </summary>
        /// <value>The default keyed hash algorithm.</value>
        /// <remarks></remarks>
        public Type DefaultKeyedHashAlgorithm
        {
            get
            {
                return _DefaultKeyedHashAlgorithm ?? typeof(HMACSHA256);
            }
            set
            {
                _DefaultKeyedHashAlgorithm = value;
            }
        }

        /// <summary>
        /// Gets the default hash algorithm. Default is <see cref="SHA256Managed"/>.
        /// </summary>
        /// <remarks></remarks>
        public Type DefaultHashAlgorithm
        {
            get
            {
                return _DefaultHashAlgorithm ?? typeof(SHA256Managed);
            }
            set
            {
                _DefaultHashAlgorithm = value;
            }
        }

        /// <summary>
        /// Gets the hash provider.
        /// </summary>
        /// <remarks></remarks>
        public IProvideHashing HashProvider
        {
            get
            {
                return _HashProvider.Value;
            }
        }

        /// <summary>
        /// Gets the keyed hash provider.
        /// </summary>
        /// <remarks></remarks>
        public IProvideKeyedHashing KeyedHashProvider
        {
            get
            {
                return _KeyedHashProvider.Value;
            }
        }

        /// <summary>
        /// Gets the symmetric encryption provider.
        /// </summary>
        /// <remarks></remarks>
        public IProvideSymmetricEncryption SymmetricEncryptionProvider
        {
            get
            {
                return _SymmetricEncryptionProvider.Value;
            }
        }

        #endregion

        #region Implementation of IApplicationComponent

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public void Configure(IApplicationConfiguration applicationConfiguration)
        {
            if (!_IsConfigured)
            {
                _DefaultHashAlgorithm = _CryptographyConfiguration.DefaultHashAlgorithm;
                _DefaultKeyedHashAlgorithm = _CryptographyConfiguration.DefaultKeyedHashAlgorithm;
                _DefaultSymmetricAlgorithm = _CryptographyConfiguration.DefaultSymmetricAlgorithm;

                _HashProvider = new Lazy<IProvideHashing>(
                    _CryptographyConfiguration.HashProviderFactory 
                        ?? (() => new HashProvider(DefaultHashAlgorithm)));

                _KeyedHashProvider = new Lazy<IProvideKeyedHashing>(
                    _CryptographyConfiguration.KeyedHashProviderFactory 
                        ?? (() => new KeyedHashProvider(DefaultKeyedHashAlgorithm)));

                _SymmetricEncryptionProvider = new Lazy<IProvideSymmetricEncryption>(
                    _CryptographyConfiguration.SymmetricEncryptionProviderFactory 
                        ?? (() => new SymmetricEncryptionProvider(DefaultSymmetricAlgorithm)));

                _IsConfigured = true;
            }
        }

        #endregion
    }
}