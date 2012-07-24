// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheConfigurationBuilder.cs">
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
//   Defines a configuration class to build the application's CacheManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

using NContext.Configuration;

namespace NContext.Caching
{
    /// <summary>
    /// Defines a configuration class to build the application's <see cref="CacheManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class PersistenceConfigurationBuilder : ApplicationComponentConfigurationBase
    {
        #region Fields

        private Lazy<ObjectCache> _Provider;

        private String _RegionName;

        private DateTimeOffset _AbsoluteExpiration;

        private TimeSpan _SlidingExpiration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public PersistenceConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
            _Provider = new Lazy<ObjectCache>(() => MemoryCache.Default);
            _AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            _SlidingExpiration = ObjectCache.NoSlidingExpiration;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the cache provider.
        /// </summary>
        /// <typeparam name="TCacheProvider">The type of the cache provider.</typeparam>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <returns>This <see cref="CacheConfigurationBuilder"/> instance.</returns>
        public PersistenceConfigurationBuilder SetProvider<TCacheProvider>(Func<TCacheProvider> cacheProvider) where TCacheProvider : ObjectCache
        {
            _Provider = new Lazy<ObjectCache>(cacheProvider);

            return this;
        }

        #endregion
    }
}