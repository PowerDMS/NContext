// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManager.cs">
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
//   Defines a default IManageCaching implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

using NContext.Configuration;
using NContext.Extensions;

namespace NContext.Caching
{
    /// <summary>
    /// Defines a default <see cref="IManageCaching"/> implementation.
    /// </summary>
    public class CacheManager : IManageCaching
    {
        #region Fields

        private Boolean _IsConfigured;

        private readonly CacheConfiguration _CacheConfiguration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="cacheConfiguration">The cache configuration settings.</param>
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
        public ObjectCache Provider
        {
            get
            {
                return _CacheConfiguration.Provider.Value;
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
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (!_IsConfigured)
            {
                _IsConfigured = true;
            }
        }

        #endregion

        #region Implementation of IManageCaching

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
            if (String.IsNullOrWhiteSpace(cacheEntryKey))
            {
                throw new ArgumentNullException("cacheEntryKey");
            }

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            Remove(cacheEntryKey);
            return Provider.Add(cacheEntryKey, instance, cacheItemPolicy ?? GetDefaultCacheItemPolicy(), regionName);
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
            return Provider.Contains(cacheEntryKey, regionName);
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
            return Provider.Get<TObject>(cacheEntryKey, regionName);
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
            return Provider.Get(cacheEntryKey, regionName);
        }

        /// <summary>
        /// Gets the number of items in cache.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>Total amount of items in cache.</returns>
        /// <remarks></remarks>
        public Int64 GetCount(String regionName = null)
        {
            return Provider.GetCount(regionName);
        }

        /// <summary>
        /// Removes the cache item associated with the specified cache entry key.
        /// </summary>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <remarks></remarks>
        public void Remove(String cacheEntryKey, String regionName = null)
        {
            if (Provider.Contains(cacheEntryKey, regionName))
            {
                Provider.Remove(cacheEntryKey, regionName);
            }
        }

        private CacheItemPolicy GetDefaultCacheItemPolicy()
        {
            return new CacheItemPolicy
                {
                    AbsoluteExpiration = _CacheConfiguration.AbsoluteExpiration,
                    SlidingExpiration = _CacheConfiguration.SlidingExpiration
                };
        }

        #endregion
    }
}