// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheManagerExtensions.cs">
//   Copyright (c) 2012
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
//   Defines extension methods for ICacheManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

using NContext.Caching;

namespace NContext.Extensions
{
    /// <summary>
    /// Defines extension methods for ICacheManager.
    /// </summary>
    /// <remarks></remarks>
    public static class ICacheManagerExtensions
    {
        /// <summary>
        /// Updates cache entry if exists, else adds the item to cache.
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean AddOrUpdateItem(this IManageCaching cacheManager, Guid cacheEntryKey, Object instance)
        {
            return cacheManager.AddOrUpdateItem(cacheEntryKey.ToString(), instance);
        }

        /// <summary>
        /// Updates cache entry if exists, else adds the item to cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean AddOrUpdateItem<TObject>(this IManageCaching cacheManager, Guid cacheEntryKey, TObject instance, CacheItemPolicy cacheItemPolicy)
        {
            return cacheManager.AddOrUpdateItem(cacheEntryKey.ToString(), instance, cacheItemPolicy);
        }

        /// <summary>
        /// Updates cache entry if exists, else adds the item to cache.
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean AddOrUpdateItem(this IManageCaching cacheManager, Guid cacheEntryKey, Object instance, CacheItemPolicy cacheItemPolicy)
        {
            return cacheManager.AddOrUpdateItem(cacheEntryKey.ToString(), instance, cacheItemPolicy);
        }
    }
}