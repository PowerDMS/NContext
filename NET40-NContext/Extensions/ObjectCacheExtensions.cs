// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectCacheExtensions.cs" company="Waking Venture, Inc.">
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