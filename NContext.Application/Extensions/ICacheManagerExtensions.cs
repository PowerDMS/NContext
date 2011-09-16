using System;
using System.Runtime.Caching;

using NContext.Application.Caching;

namespace NContext.Application.Extensions
{
    public static class ICacheManagerExtensions
    {
        public static Boolean AddOrUpdateItem(this ICacheManager cacheManager, Guid cacheEntryKey, Object instance)
        {
            return cacheManager.AddOrUpdateItem(cacheEntryKey.ToString(), instance);
        }

        public static Boolean AddOrUpdateItem<TObject>(this ICacheManager cacheManager, Guid cacheEntryKey, TObject instance, CacheItemPolicy cacheItemPolicy)
        {
            return cacheManager.AddOrUpdateItem(cacheEntryKey.ToString(), instance, cacheItemPolicy);
        }

        public static Boolean AddOrUpdateItem(this ICacheManager cacheManager, Guid cacheEntryKey, Object instance, CacheItemPolicy cacheItemPolicy)
        {
            return cacheManager.AddOrUpdateItem(cacheEntryKey.ToString(), instance, cacheItemPolicy);
        }
    }
}