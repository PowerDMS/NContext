namespace NContext.Extensions.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.Caching;

    using StackExchange.Redis;

    public class RedisCache : ObjectCache, IRedisCache
    {
        private const DefaultCacheCapabilities _DefaultCacheCapabilities =
            DefaultCacheCapabilities.OutOfProcessProvider |
            DefaultCacheCapabilities.AbsoluteExpirations |
            DefaultCacheCapabilities.SlidingExpirations;

        private const String _CacheItemSlidingExpirations = @"CacheItemSlidingExpirations";

        private readonly ISerializer _Serializer;

        private readonly ConnectionMultiplexer _Multiplexer;

        public RedisCache(Func<ConfigurationOptions> redisClusterConfigurationOptionsFactory, ISerializer serializer = null)
        {
            _Serializer = serializer ?? new BinaryFormatterSerializer();
            _Multiplexer = ConnectionMultiplexer.Connect(redisClusterConfigurationOptionsFactory());
        }

        #region IRedisCache

        public IDatabase Database
        {
            get { return _Multiplexer.GetDatabase(); }
        }

        public IDatabaseAsync DatabaseAsync
        {
            get { return _Multiplexer.GetDatabase(); }
        }

        public ConnectionMultiplexer Multiplexer
        {
            get { return _Multiplexer; }
        }

        #endregion

        #region ObjectCache

        public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<String> keys, String regionName = null)
        {
            throw new NotSupportedException();
        }

        protected override IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        public override Boolean Contains(String key, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);

            return Database.KeyExists(key);
        }

        public override Object AddOrGetExisting(String key, Object value, DateTimeOffset absoluteExpiration, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);

            return AddOrGetExistingInternal(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
        }

        public override CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return new CacheItem(value.Key, AddOrGetExistingInternal(value.Key, value.Value, policy));
        }

        public override Object AddOrGetExisting(String key, Object value, CacheItemPolicy policy, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);

            return AddOrGetExistingInternal(key, value, policy);
        }

        public override Object Get(String key, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);

            return GetValue(key);
        }

        public override CacheItem GetCacheItem(String key, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);

            return new CacheItem(key, GetValue(key));
        }

        public override void Set(String key, Object value, DateTimeOffset absoluteExpiration, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);

            SetInternal(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
        }

        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            SetInternal(item.Key, item.Value, policy);
        }

        public override void Set(String key, Object value, CacheItemPolicy policy, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);

            SetInternal(key, value, policy);
        }

        public override IDictionary<String, Object> GetValues(IEnumerable<String> keys, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);
            Contract.Requires<ArgumentNullException>(keys != null);

            var keyCount = keys.Count();
            var result = keys.ToDictionary(key => key, key => (Object)null);

            keys.AsParallel()
                .WithDegreeOfParallelism(keyCount > Environment.ProcessorCount ? Environment.ProcessorCount : keyCount)
                .ForAll(key => result[key] = GetValue(key));

            return result;
        }

        public override Object Remove(String key, String regionName = null)
        {
            Contract.Requires<RegionUnsupportedException>(regionName == null);

            var value = GetValue(key);
            Database.KeyDelete(key);
            Database.HashDelete(_CacheItemSlidingExpirations, key);

            return value;
        }

        /// <summary>
        /// This property is not supported and will always throw an exception.  When overridden in a derived class, gets the total number of cache entries in the cache.
        /// </summary>
        /// <param name="regionName">Optional. A named region in the cache for which the cache entry count should be computed, if regions are implemented. The default value for the optional parameter is null.</param>
        /// <returns>The number of cache entries in the cache. If <paramref name="regionName" /> is not null, the count indicates the number of entries that are in the specified cache region.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override Int64 GetCount(String regionName = null)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, gets a description of the features that a cache implementation provides.
        /// </summary>
        /// <value>The default cache capabilities.</value>
        /// <returns>A bitwise combination of flags that indicate the default capabilities of a cache implementation.</returns>
        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get { return _DefaultCacheCapabilities; }
        }

        /// <summary>
        /// Gets the name of a specific <see cref="T:System.Runtime.Caching.ObjectCache" /> instance.
        /// </summary>
        /// <value>The name.</value>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <returns>The name of a specific cache instance.</returns>
        public override String Name
        {
            get { return _Multiplexer.ClientName; }
        }

        public override Object this[String key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        private Object AddOrGetExistingInternal(String key, Object value, CacheItemPolicy policy)
        {
            var cacheItem = GetValue(key);
            if (cacheItem != null)
            {
                return cacheItem;
            }

            if (SetInternal(key, value, policy, When.NotExists))
            {
                return value;
            }

            // TODO: (DG) Verify this logic is correct.
            // When.NotExists (above) will instruct Redis to not overwrite an existing entry. So if two threads simultaneously try to set 
            // the same key then the second will return false, and therefore we should attempt to get the now-existing value.
            cacheItem = GetValue(key);
            if (cacheItem != null)
            {
                return cacheItem;
            }

            // TODO: (DG) This exception means a value could not be set because the key already exists, yet it's no longer there right after.
            throw new Exception("could not set or get!! AHHH!!!!");
        }

        private Boolean SetInternal(String key, Object value, CacheItemPolicy policy, When condition = When.Always)
        {
            var result = SetValue(key, value, GetPolicyExpiration(policy), condition); // TODO: (DG) Is this thread-safe?
            if (result)
            {
                ConfigureSlidingExpiration(key, policy);
            }

            return result;
        }

        private void ConfigureSlidingExpiration(String key, CacheItemPolicy policy)
        {
            if (policy == null || policy.SlidingExpiration == NoSlidingExpiration) return;

            Database.HashSet(_CacheItemSlidingExpirations, key, policy.SlidingExpiration.Ticks);
        }

        private Object GetValue(String key)
        {
            if (key == null) throw new ArgumentNullException("key");

            if (!Database.KeyExists(key))
            {
                return null;
            }

            var result = Database.StringGet(key);
            if (result.IsNull)
            {
                return null;
            }

            // Check and see if we need to slide the expiration.
            var slideExpiration = Database.HashGet(_CacheItemSlidingExpirations, key);
            if (slideExpiration.HasValue)
            {
                Database.KeyExpire(key, TimeSpan.FromTicks((Int64)slideExpiration));
            }

            return _Serializer.Deserialize(result);
        }

        private Boolean SetValue(String key, Object value, TimeSpan? expiration = null, When condition = When.Always)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            return Database.StringSet(key, _Serializer.Serialize(value), expiration, condition);
        }

        #endregion

        private TimeSpan? GetPolicyExpiration(CacheItemPolicy policy)
        {
            if (policy == null)
            {
                policy = new CacheItemPolicy();
            }

            TimeSpan? expiration = null; // Infinite expiration
            if (policy.AbsoluteExpiration != InfiniteAbsoluteExpiration)
            {
                // Absolute is set
                expiration = policy.AbsoluteExpiration.Subtract(DateTimeOffset.UtcNow);
            }
            else if (policy.SlidingExpiration != NoSlidingExpiration)
            {
                // Sliding is set
                expiration = policy.SlidingExpiration;
            }

            return expiration;
        }
    }
}