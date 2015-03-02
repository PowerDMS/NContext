// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManagerBuilder.cs" company="Waking Venture, Inc.">
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

namespace NContext.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    using NContext.Configuration;

    /// <summary>
    /// Defines a configuration class to build the application's <see cref="CacheManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class CacheManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private readonly IDictionary<String, Func<ObjectCache>> _Providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManagerBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public CacheManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
            _Providers = new Dictionary<String, Func<ObjectCache>>();
        }

        /// <summary>
        /// Sets the cache provider.
        /// </summary>
        /// <typeparam name="TCacheProvider">The type of the cache provider.</typeparam>
        /// <param name="providerName">A name to uniquely identify the cache provider.</param>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <returns>This <see cref="CacheManagerBuilder"/> instance.</returns>
        public CacheManagerBuilder AddProvider<TCacheProvider>(String providerName, Func<TCacheProvider> cacheProvider) 
            where TCacheProvider : ObjectCache
        {
            if (_Providers.ContainsKey(providerName))
            {
                throw new ArgumentException("Provider key already exists in collection.", "providerName");
            }

            _Providers.Add(providerName, cacheProvider);

            return this;
        }

        /// <summary>
        /// Sets the application's cache manager using 
        /// the configuration created from this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageCaching>(() => new CacheManager(new CacheConfiguration(_Providers)));
        }
    }
}