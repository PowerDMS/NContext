namespace NContext.Extensions
{
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// Defines extension methods for <see cref="ObjectCache"/>.
    /// </summary>
    /// <remarks></remarks>
    public static class ObjectCacheExtensions
    {
        /// <summary>
        /// Returns an entry from the cache.
        /// </summary>
        /// <returns>
        /// A reference to the cache entry that is identified by <paramref name="cacheEntryKey"/>, if the entry exists; otherwise, null.
        /// </returns>
        /// <param name="cacheEntryKey">A unique identifier for the cache entry to get. </param>
        /// <param name="regionName">A named region in the cache to which a cache entry was added.</param>
        /// <param name="objectCache"><see cref="ObjectCache"/> instance.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="cacheEntryKey"/> is null.</exception>
        public static T Get<T>(this ObjectCache objectCache, String cacheEntryKey, String regionName = null)
        {
            return (T)objectCache.Get(cacheEntryKey, regionName);
        }

        /// <summary>
        /// Removes a cache entry from the cache.
        /// </summary>
        /// <returns>
        /// If the entry is found in the cache, the removed cache entry; otherwise, null.
        /// </returns>
        /// <param name="objectCache"></param>
        /// <param name="cacheEntryKey">A unique identifier for the cache entry to remove. </param>
        /// <param name="regionName">A named region in the cache to which a cache entry was added.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="cacheEntryKey"/> is null.</exception>
        public static T Remove<T>(this ObjectCache objectCache, String cacheEntryKey, String regionName = null)
        {
            return (T)objectCache.Remove(cacheEntryKey, regionName);
        }
    }
}