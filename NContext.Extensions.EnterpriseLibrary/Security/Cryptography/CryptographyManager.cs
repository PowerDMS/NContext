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

        private static Boolean _IsConfigured;

        private static IProvideCryptographicOperations _Provider;

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

        public IProvideCryptographicOperations Provider
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