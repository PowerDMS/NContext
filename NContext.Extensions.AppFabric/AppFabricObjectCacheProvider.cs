// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppFabricObjectCacheProvider.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.AppFabric
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Caching;

    using Microsoft.ApplicationServer.Caching;

    /// <summary>
    /// Class AppFabricObjectCacheProvider
    /// </summary>
    public class AppFabricObjectCacheProvider : ObjectCache
    {
        private const String _RegionKeyTemplate = "{0}:{1}";

        private const String _AppFabricObjectCacheName = @"AppFabricCacheService";

        private readonly DataCacheFactory _CacheFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFabricObjectCacheProvider" /> class.
        /// </summary>
        public AppFabricObjectCacheProvider()
        {
            _CacheFactory = new DataCacheFactory();
        }

        /// <summary>
        /// When overridden in a derived class, gets a description of the features that a cache implementation provides.
        /// </summary>
        /// <value>The default cache capabilities.</value>
        /// <returns>A bitwise combination of flags that indicate the default capabilities of a cache implementation.</returns>
        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                return DefaultCacheCapabilities.OutOfProcessProvider | 
                       DefaultCacheCapabilities.AbsoluteExpirations |
                       DefaultCacheCapabilities.SlidingExpirations;
            }
        }

        /// <summary>
        /// Gets the name of a specific <see cref="T:System.Runtime.Caching.ObjectCache" /> instance.
        /// </summary>
        /// <value>The name.</value>
        /// <returns>The name of a specific cache instance.</returns>
        public override String Name
        {
            get
            {
                return _AppFabricObjectCacheName;
            }
        }

        /// <summary>
        /// Gets or sets the default indexer for the <see cref="T:System.Runtime.Caching.ObjectCache" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override Object this[String key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, specifying a key and a value for the cache entry, and information about how the entry will be evicted.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry's value; otherwise, null.</returns>
        public override Object AddOrGetExisting(String key, Object value, CacheItemPolicy policy, String regionName = null)
        {
            return AddOrGetExisting(new CacheItem(key, value, regionName), policy).Value;
        }

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, by using a key, an object for the cache entry, an absolute expiration value, and an optional region to add the cache into.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which the cache entry will expire.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry's value; otherwise, null.</returns>
        public override Object AddOrGetExisting(String key, Object value, DateTimeOffset absoluteExpiration, String regionName = null)
        {
            return
                AddOrGetExisting(
                    new CacheItem(key, value, regionName), 
                    new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration }).Value;
        }

        /// <summary>
        /// When overridden in a derived class, inserts the specified <see cref="T:System.Runtime.Caching.CacheItem" /> object into the cache, specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry; otherwise, null.</returns>
        public override CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy)
        {
            var data = Get(value.Key, value.RegionName);
            if (data != null)
            {
                return new CacheItem(value.Key, data, value.RegionName);
            }

            Set(value, policy);
            return value;
        }

        /// <summary>
        /// When overridden in a derived class, checks whether the cache entry already exists in the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache where the cache can be found, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>true if the cache contains a cache entry with the same key value as <paramref name="key" />; otherwise, false.</returns>
        public override Boolean Contains(String key, String regionName = null)
        {
            return Get(key, regionName) != null;
        }

        /// <summary>
        /// When overridden in a derived class, creates a <see cref="T:System.Runtime.Caching.CacheEntryChangeMonitor" /> object that can trigger events in response to changes to specified cache entries.
        /// </summary>
        /// <param name="keys">The unique identifiers for cache entries to monitor.</param>
        /// <param name="regionName">Optional. A named region in the cache where the cache keys in the <paramref name="keys" /> parameter exist, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>A change monitor that monitors cache entries in the cache.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<String> keys, String regionName = null)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, gets the specified cache entry from the cache as an object.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to get.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry was added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>The cache entry that is identified by <paramref name="key" />.</returns>
        public override Object Get(String key, String regionName = null)
        {
            try
            {
                return _CacheFactory.GetDefaultCache().Get(GetKey(key, regionName));
            }
            catch (DataCacheException ex)
            {
                if (ex.ErrorCode == DataCacheErrorCode.RetryLater)
                {
                    // temporal failure, ignore and continue
                    return null;
                }

                throw;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the specified cache entry from the cache as a <see cref="T:System.Runtime.Caching.CacheItem" /> instance.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to get.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache was added, if regions are implemented. Because regions are not implemented in .NET Framework 4, the default is null.</param>
        /// <returns>The cache entry that is identified by <paramref name="key" />.</returns>
        public override CacheItem GetCacheItem(String key, String regionName = null)
        {
            var data = Get(key, regionName);
            if (data != null)
            {
                return new CacheItem(key, data, regionName);
                
            }

            return null;
        }

        /// <summary>
        /// When overridden in a derived class, gets the total number of cache entries in the cache.
        /// </summary>
        /// <param name="regionName">Optional. A named region in the cache for which the cache entry count should be computed, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>The number of cache entries in the cache. If <paramref name="regionName" /> is not null, the count indicates the number of entries that are in the specified cache region.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override Int64 GetCount(String regionName = null)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, gets a set of cache entries that correspond to the specified keys.
        /// </summary>
        /// <param name="keys">A collection of unique identifiers for the cache entries to get.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry or entries were added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>A dictionary of key/value pairs that represent cache entries.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override IDictionary<String, Object> GetValues(IEnumerable<String> keys, String regionName = null)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, removes the cache entry from the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry was added, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>An object that represents the value of the removed cache entry that was specified by the key, or null if the specified entry was not found.</returns>
        public override Object Remove(String key, String regionName = null)
        {
            var data = Get(key, regionName);
            if (data != null)
            {
                _CacheFactory.GetDefaultCache().Remove(GetKey(key, regionName));
                return data;
            }

            return null;
        }

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        public override void Set(String key, Object value, CacheItemPolicy policy, String regionName = null)
        {
            Set(new CacheItem(key, value, regionName), policy);
        }

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, specifying time-based expiration details.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which the cache entry will expire.</param>
        /// <param name="regionName">Optional. A named region in the cache to which the cache entry can be added, if regions are implemented. The default value for the optional parameter is null.</param>
        public override void Set(String key, Object value, DateTimeOffset absoluteExpiration, String regionName = null)
        {
            Set(new CacheItem(key, value, regionName), new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
        }

        /// <summary>
        /// When overridden in a derived class, inserts the cache entry into the cache as a <see cref="T:System.Runtime.Caching.CacheItem" /> instance, specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="item">The cache item to add.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration.</param>
        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            if (item.Value == null)
            {
                return;
            }

            try
            {
                TimeSpan timeout;
                if (policy.SlidingExpiration != TimeSpan.Zero)
                {
                    timeout = policy.SlidingExpiration;
                }
                else
                {
                    timeout = policy.AbsoluteExpiration - DateTime.UtcNow;
                }

                _CacheFactory.GetDefaultCache().Put(GetKey(item.Key, item.RegionName), item.Value, timeout);
            }
            catch (DataCacheException ex)
            {
                if (ex.ErrorCode == DataCacheErrorCode.RetryLater)
                {
                    // temporal failure, ignore and continue
                    return;
                }

                throw;
            }
        }

        /// <summary>
        /// When overridden in a derived class, creates an enumerator that can be used to iterate through a collection of cache entries.
        /// </summary>
        /// <returns>The enumerator object that provides access to the cache entries in the cache.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        protected override IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        private static String GetKey(String key, String regionName)
        {
            if (String.IsNullOrWhiteSpace(regionName))
            {
                return key;
            }

            return String.Format(CultureInfo.InvariantCulture, _RegionKeyTemplate, key, regionName);
        }
    }
}