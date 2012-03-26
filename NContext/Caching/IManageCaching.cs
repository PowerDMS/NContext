// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManageCaching.cs">
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
//   Defines a contract from managing the cache provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

using NContext.Configuration;

namespace NContext.Caching
{
    /// <summary>
    /// Defines a contract for managing the application's cache provider.
    /// </summary>
    public interface IManageCaching : IApplicationComponent
    {
        #region Properties

        /// <summary>
        /// Gets the cache provider.
        /// </summary>
        /// <remarks></remarks>
        ObjectCache Provider { get; }

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
        Object this[String cacheEntryKey, String regionName = null]
        {
            get;
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
        Boolean AddOrUpdateItem<TObject>(String cacheEntryKey, TObject instance, String regionName = null);

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
        Boolean AddOrUpdateItem<TObject>(String cacheEntryKey, TObject instance, CacheItemPolicy cacheItemPolicy, String regionName = null);

        /// <summary>
        /// Returns true if key refers to item current stored in cache
        /// </summary>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>True if item referenced by key is in the cache</returns>
        /// <remarks></remarks>
        Boolean Contains(String cacheEntryKey, String regionName = null);

        /// <summary>
        /// Removes all items from the cache. If an error occurs during the removal, the cache is left unchanged.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <remarks></remarks>
        void Flush(String regionName = null);

        /// <summary>
        /// Gets the cache item associated with the specified key.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns><typeparamref name="TObject"/> instance if found in the cache, else null.</returns>
        /// <remarks></remarks>
        TObject Get<TObject>(String cacheEntryKey, String regionName = null) where TObject : class;

        /// <summary>
        /// Gets the cache item associated with the specified key.
        /// </summary>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>Object instance if found in the cache, else null.</returns>
        /// <remarks></remarks>
        Object Get(String cacheEntryKey, String regionName = null);

        /// <summary>
        /// Gets the number of items in cache.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>Total amount of items in cache.</returns>
        /// <remarks></remarks>
        Int64 GetCount(String regionName = null);

        /// <summary>
        /// Removes the cache item associated with the specified cache entry key.
        /// </summary>
        /// <param name="cacheEntryKey">The cache entry key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <remarks></remarks>
        void Remove(String cacheEntryKey, String regionName = null);

        #endregion
    }
}