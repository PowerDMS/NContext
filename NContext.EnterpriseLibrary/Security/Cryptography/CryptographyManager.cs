// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyManager.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>

// <summary>
//   Defines manager class for application cryptography-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Application.Configuration;

namespace NContext.Application.Security.Cryptography
{
    /// <summary>
    /// Defines manager class for application cryptography-related operations.
    /// </summary>
    /// <remarks></remarks>
    public class CryptographyManager : ICryptographyManager
    {
        #region Fields

        // TODO: (DG) Add support for default symmetric key and hash key.

        private static Boolean _IsConfigured;

        private static ICryptographyProvider _Provider;

        private readonly CryptographyConfiguration _CryptographyConfiguration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyManager"/> class.
        /// </summary>
        /// <remarks></remarks>
        public CryptographyManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyManager"/> class.
        /// </summary>
        /// <param name="cryptographyConfiguration">The cryptography configuration.</param>
        /// <remarks></remarks>
        public CryptographyManager(CryptographyConfiguration cryptographyConfiguration)
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

        public ICryptographyProvider Provider
        {
            get
            {
                return _Provider;
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
                if (_CryptographyConfiguration == null)
                {
                    _Provider = new CryptographyProvider();
                }
                else
                {
                    _Provider = new CryptographyProvider(_CryptographyConfiguration.DefaultHashAlgorithm, 
                                                         _CryptographyConfiguration.DefaultKeyedHashAlgorithm, 
                                                         _CryptographyConfiguration.DefaultSymmetricAlgorithm);
                }

                _IsConfigured = true;
            }
        }

        #endregion
    }
}