// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheManager.cs">
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
//
// <summary>
//   Defines a contract from managing the cache provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

using NContext.Application.Configuration;

namespace NContext.Application.Caching
{
    /// <summary>
    /// Defines a contract for managing the application's cache provider.
    /// </summary>
    public interface ICacheManager : IApplicationComponent
    {
        #region Properties

        /// <summary>
        /// Gets the cache provider.
        /// </summary>
        /// <remarks></remarks>
        ObjectCache CacheProvider { get; }

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
        /// Adds or updates the specified instance to the cache.
        /// </summary>
        /// <param name="cacheEntryKey">The cache enty key.</param>
        /// <param name="instance">The object instance.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>True, if the instance has successfully been added to the cache, else false.</returns>
        /// <remarks></remarks>
        Boolean AddOrUpdateItem(String cacheEntryKey, Object instance, CacheItemPolicy cacheItemPolicy, String regionName = null);

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