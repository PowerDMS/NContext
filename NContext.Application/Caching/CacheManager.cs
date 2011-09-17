// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManager.cs">
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
//   Defines a default ICacheManager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

using NContext.Application.Extensions;
using NContext.Application.Configuration;

namespace NContext.Application.Caching
{
    /// <summary>
    /// Defines a default <see cref="ICacheManager"/> implementation.
    /// </summary>
    public class CacheManager : ICacheManager
    {
        #region Fields

        private static DateTimeOffset _DefaultCachItemAbsoluteExpiration;

        private static TimeSpan _DefaultCacheItemSlidingExpiration = ObjectCache.NoSlidingExpiration;

        private static Lazy<ObjectCache> _CacheProvider;

        private static Boolean _IsConfigured;

        private readonly CacheConfiguration _CacheConfiguration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="cacheConfiguration">The caching configuration.</param>
        /// <remarks></remarks>
        public CacheManager(CacheConfiguration cacheConfiguration)
        {
            if (cacheConfiguration == null)
            {
                throw new ArgumentNullException("cacheConfiguration");
            }

            _CacheConfiguration = cacheConfiguration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the cache provider.
        /// </summary>
        /// <remarks></remarks>
        public ObjectCache CacheProvider
        {
            get
            {
                return _CacheProvider.Value;
            }
        }

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

        #endregion

        #region Indexers

        /// <summary>
        /// Returns the item identified by the provided key.
        /// </summary>
        /// <param name="cacheEntryKey">The cache entry key to retrieve.</param>
        /// <param name="regionName">The cache region.</param>
        /// <exception cref="T:System.ArgumentNullException">Provided key is null</exception>
        /// <exception cref="T:System.ArgumentException">Provided key is an empty string</exception>
        /// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the cache items.
        /// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        public Object this[String cacheEntryKey, String regionName = null]
        {
            get
            {
                return Get(cacheEntryKey, regionName);
            }
        }

        #endregion

        #region Implementation of IApplicationComponent

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public virtual void Configure(IApplicationConfiguration applicationConfiguration)
        {
            if (!_IsConfigured)
            {
                _CacheProvider = new Lazy<ObjectCache>(_CacheConfiguration.Provider);
                _DefaultCachItemAbsoluteExpiration = _CacheConfiguration.AbsoluteExpiration;
                _DefaultCacheItemSlidingExpiration = _CacheConfiguration.SlidingExpiration;
                _IsConfigured = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds or updates the specified instance to the cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to cache.</typeparam>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="instance">The object instance.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>True, if the instance has successfully been added to the cache, else false.</returns>
        /// <remarks></remarks>
        public Boolean AddOrUpdateItem<TObject>(String cacheEntryKey, TObject instance, String regionName = null)
        {
            return AddOrUpdateItem<TObject>(cacheEntryKey, instance, GetDefaultCacheItemPolicy(), regionName);
        }

        /// <summary>
        /// Adds or updates the specified instance to the cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to cache.</typeparam>
        /// <param name="cacheEntryKey">The cache enty key.</param>
        /// <param name="instance">The object instance.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>True, if the instance has successfully been added to the cache, else false.</returns>
        /// <remarks></remarks>
        public Boolean AddOrUpdateItem<TObject>(String cacheEntryKey, TObject instance, CacheItemPolicy cacheItemPolicy, String regionName = null)
        {
            Remove(cacheEntryKey);
            return _CacheProvider.Value.Add(cacheEntryKey, instance, cacheItemPolicy ?? GetDefaultCacheItemPolicy(), regionName);
        }

        /// <summary>
        /// Adds or updates the specified instance to the cache.
        /// </summary>
        /// <param name="cacheEntryKey">The cache enty key.</param>
        /// <param name="instance">The object instance.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>True, if the instance has successfully been added to the cache, else false.</returns>
        /// <remarks></remarks>
        public Boolean AddOrUpdateItem(String cacheEntryKey, Object instance, CacheItemPolicy cacheItemPolicy, String regionName = null)
        {
            Remove(cacheEntryKey);
            return _CacheProvider.Value.Add(cacheEntryKey, instance, cacheItemPolicy ?? GetDefaultCacheItemPolicy(), regionName);
        }

        /// <summary>
        /// Returns true if key refers to item current stored in cache
        /// </summary>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>True if item referenced by key is in the cache</returns>
        /// <remarks></remarks>
        public Boolean Contains(String cacheEntryKey, String regionName = null)
        {
            return _CacheProvider.Value.Contains(cacheEntryKey, regionName);
        }

        /// <summary>
        /// Removes all items from the cache. If an error occurs during the removal, the cache is left unchanged.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
        /// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        public void Flush(String regionName = null)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the cache item associated with the specified key.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns><typeparamref name="TObject"/> instance if found in the cache, else null.</returns>
        /// <remarks></remarks>
        public TObject Get<TObject>(String cacheEntryKey, String regionName) where TObject : class
        {
            return _CacheProvider.Value.Get<TObject>(cacheEntryKey, regionName);
        }

        /// <summary>
        /// Gets the cache item associated with the specified key.
        /// </summary>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>Object instance if found in the cache, else null.</returns>
        /// <remarks></remarks>
        public Object Get(String cacheEntryKey, String regionName)
        {
            return _CacheProvider.Value.Get(cacheEntryKey, regionName);
        }

        /// <summary>
        /// Gets the number of items in cache.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>Total amount of items in cache.</returns>
        /// <remarks></remarks>
        public Int64 GetCount(String regionName = null)
        {
            return _CacheProvider.Value.GetCount(regionName);
        }

        /// <summary>
        /// Removes the cache item associated with the specified cache entry key.
        /// </summary>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <remarks></remarks>
        public void Remove(String cacheEntryKey, String regionName = null)
        {
            if (_CacheProvider.Value.Contains(cacheEntryKey, regionName))
            {
                _CacheProvider.Value.Remove(cacheEntryKey, regionName);
            }
        }

        private CacheItemPolicy GetDefaultCacheItemPolicy()
        {
            return new CacheItemPolicy
                {
                    AbsoluteExpiration = _DefaultCachItemAbsoluteExpiration,
                    SlidingExpiration = _DefaultCacheItemSlidingExpiration
                };
        }

        #endregion
    }
}