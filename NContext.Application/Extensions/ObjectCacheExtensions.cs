using System;
using System.Runtime.Caching;

namespace NContext.Application.Extensions
{
    public static class ObjectCacheExtensions
    {
        public static TObject Get<TObject>(this ObjectCache objectCache, String cacheEntryKey, String regionName = null)
            where TObject : class
        {
            return objectCache.Get(cacheEntryKey, regionName) as TObject;
        }
    }
}