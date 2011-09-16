// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyConfiguration.cs">
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
//   Defines a configuration class to build the application's CryptographyManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;

using NContext.Application.Configuration;
using NContext.Application.Extensions;

namespace NContext.Application.Security.Cryptography
{
    /// <summary>
    /// Defines a configuration class to build the application's <see cref="CryptographyManager"/>.
    /// </summary>
    public class CryptographyConfiguration : ApplicationComponentConfigurationBase
    {
        #region Fields

        private Type _DefaultHashAlgorithm;

        private Type _DefaultKeyedHashAlgorithm;

        private Type _DefaultSymmetricAlgorithm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyConfiguration"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public CryptographyConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        #endregion

        #region Properties

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
        public CryptographyConfiguration SetDefaults(Type defaultHashAlgorithm, Type defaultKeyedHashAlgorithm, Type defaultSymmetricAlgorithm)
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
                   .RegisterComponent<ICryptographyManager>(() => new CryptographyManager(this));
        }

        #endregion
    }
}