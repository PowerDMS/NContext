// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheConfigurationBuilder.cs">
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

using NContext.Configuration;

namespace NContext.Caching
{
    /// <summary>
    /// Defines a configuration class to build the application's <see cref="CacheManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class CacheConfigurationBuilder : ApplicationComponentConfigurationBase
    {
        #region Fields

        private Lazy<ObjectCache> _Provider;

        private String _RegionName;

        private DateTimeOffset _AbsoluteExpiration;

        private TimeSpan _SlidingExpiration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public CacheConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
            _Provider = new Lazy<ObjectCache>(() => MemoryCache.Default);
            _AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            _SlidingExpiration = ObjectCache.NoSlidingExpiration;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the cache provider.
        /// </summary>
        /// <typeparam name="TCacheProvider">The type of the cache provider.</typeparam>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <returns>This <see cref="CacheConfigurationBuilder"/> instance.</returns>
        public CacheConfigurationBuilder SetProvider<TCacheProvider>(Func<TCacheProvider> cacheProvider) where TCacheProvider : ObjectCache
        {
            _Provider = new Lazy<ObjectCache>(cacheProvider);

            return this;
        }

        /// <summary>
        /// Sets the name of the default region to use.
        /// </summary>
        /// <param name="cacheRegionName">Name of the cache region.</param>
        /// <returns>This <see cref="CacheConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CacheConfigurationBuilder SetRegionName(String cacheRegionName)
        {
            _RegionName = cacheRegionName;

            return this;
        }

        /// <summary>
        /// Sets the cache configuration settings for cache item expiration.
        /// </summary>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <returns>This <see cref="CacheConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public CacheConfigurationBuilder SetDefaults(DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration)
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
                   .RegisterComponent<IManageCaching>(
                   () => 
                       new CacheManager(
                           new CacheConfiguration(_Provider, _RegionName, _AbsoluteExpiration, _SlidingExpiration)));
        }

        #endregion
    }
}