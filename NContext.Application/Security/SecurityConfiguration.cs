// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityConfiguration.cs">
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