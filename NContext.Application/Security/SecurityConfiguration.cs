// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityConfiguration.cs">
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
//   Defines a configuration class to build the application's SecurityManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Application.Configuration;

namespace NContext.Application.Security
{
    /// <summary>
    /// Defines a configuration class to build the application's <see cref="SecurityManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class SecurityConfiguration : ApplicationComponentConfigurationBase
    {
        #region Fields

        private DateTimeOffset _TokenAbsoluteExpiration;

        private TimeSpan _TokenSlidingExpiration;

        private TimeSpan _TokenInitialLifespan;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityConfiguration"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public SecurityConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the token absolute expiration.
        /// </summary>
        /// <remarks></remarks>
        public DateTimeOffset TokenAbsoluteExpiration
        {
            get
            {
                return _TokenAbsoluteExpiration;
            }
        }

        /// <summary>
        /// Gets the token sliding expiration.
        /// </summary>
        /// <remarks></remarks>
        public TimeSpan TokenSlidingExpiration
        {
            get
            {
                return _TokenSlidingExpiration;
            }
        }

        /// <summary>
        /// Gets the token initial lifetime.
        /// </summary>
        /// <remarks></remarks>
        public TimeSpan TokenInitialLifespan
        {
            get
            {
                return _TokenInitialLifespan;
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Sets the default cache configuration settings for security token expiration.
        /// </summary>
        /// <param name="tokenAbsoluteExpiration">The token absolute expiration.</param>
        /// <param name="tokenInitialLifespan">The token initial lifespan.</param>
        /// <param name="tokenSlidingExpiration">The token sliding expiration.</param>
        /// <returns>This <see cref="SecurityConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public SecurityConfiguration SetDefaults(DateTimeOffset tokenAbsoluteExpiration, TimeSpan tokenInitialLifespan, TimeSpan tokenSlidingExpiration)
        {
            _TokenAbsoluteExpiration = tokenAbsoluteExpiration;
            _TokenSlidingExpiration = tokenSlidingExpiration;
            _TokenInitialLifespan = tokenInitialLifespan;

            return this;
        }

        /// <summary>
        /// Sets the application's security manager using 
        /// the configuration created from this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<ISecurityManager>(() => new SecurityManager(this));
        }

        #endregion
    }
}