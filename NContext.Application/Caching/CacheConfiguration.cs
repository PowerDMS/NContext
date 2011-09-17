// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheConfiguration.cs">
//   This file is part of NContext.
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
//   Defines a configuration class to build the application's CacheManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

using NContext.Application.Configuration;

namespace NContext.Application.Caching
{
    /// <summary>
    /// Defines a configuration class to build the application's <see cref="CacheManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class CacheConfiguration : ApplicationComponentConfigurationBase
    {
        #region Fields

        private Func<ObjectCache> _Provider;

        private String _RegionName;

        private DateTimeOffset _AbsoluteExpiration;

        private TimeSpan _SlidingExpiration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfiguration"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public CacheConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <remarks></remarks>
        public Func<ObjectCache> Provider
        {
            get
            {
                return _Provider;
            }
        }

        /// <summary>
        /// Gets the name of the region.
        /// </summary>
        /// <remarks></remarks>
        public String RegionName
        {
            get
            {
                return _RegionName;
            }
        }

        /// <summary>
        /// Gets the absolute expiration.
        /// </summary>
        /// <remarks></remarks>
        public DateTimeOffset AbsoluteExpiration
        {
            get
            {
                return _AbsoluteExpiration;
            }
        }

        /// <summary>
        /// Gets the sliding expiration.
        /// </summary>
        /// <remarks></remarks>
        public TimeSpan SlidingExpiration
        {
            get
            {
                return _SlidingExpiration;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the cache provider.
        /// </summary>
        /// <typeparam name="TCacheProvider">The type of the cache provider.</typeparam>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <returns>This <see cref="CacheConfiguration"/> instance.</returns>
        public CacheConfiguration SetProvider<TCacheProvider>(Func<TCacheProvider> cacheProvider) where TCacheProvider : ObjectCache
        {
            _Provider = cacheProvider;

            return this;
        }

        /// <summary>
        /// Sets the name of the default region to use.
        /// </summary>
        /// <param name="cacheRegionName">Name of the cache region.</param>
        /// <returns>This <see cref="CacheConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public CacheConfiguration SetRegionName(String cacheRegionName)
        {
            _RegionName = cacheRegionName;

            return this;
        }

        /// <summary>
        /// Sets the cache configuration settings for cache item expiration.
        /// </summary>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <returns>This <see cref="CacheConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public CacheConfiguration SetDefaults(DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration)
        {
            _SlidingExpiration = slidingExpiration;
            _AbsoluteExpiration = absoluteExpiration;

            return this;
        }

        /// <summary>
        /// Sets the application's cache manager using 
        /// the configuration created from this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<ICacheManager>(() => new CacheManager(this));
        }

        #endregion
    }
}