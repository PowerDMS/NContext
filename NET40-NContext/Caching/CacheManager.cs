// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Waking Venture, Inc.">
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
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Runtime.Caching;

    using NContext.Configuration;

    /// <summary>
    /// Defines a default <see cref="IManageCaching"/> implementation.
    /// </summary>
    public class CacheManager : IManageCaching
    {
        private readonly CacheConfiguration _CacheConfiguration;

        private Boolean _IsConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="cacheConfiguration">The cache configuration settings.</param>
        /// <remarks></remarks>
        public CacheManager(CacheConfiguration cacheConfiguration)
        {
            if (cacheConfiguration == null)
            {
                throw new ArgumentNullException("cacheConfiguration");
            }

            _CacheConfiguration = cacheConfiguration;
        }

        /// <summary>
        /// Gets the cache providers.
        /// </summary>
        /// <remarks></remarks>
        public IEnumerable<ObjectCache> Providers
        {
            get
            {
                return _CacheConfiguration.Providers.Values.Select(provider => provider.Value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>The is configured.</value>
        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
            protected set
            {
                _IsConfigured = value;
            }
        }

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageCaching>(this);
            _IsConfigured = true;
        }

        /// <summary>
        /// Gets the provider by unique name.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>ObjectCache.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">providerName;There is no cache provider registered with name:  + providerName</exception>
        public ObjectCache GetProvider(String providerName)
        {
            if (!_CacheConfiguration.Providers.ContainsKey(providerName))
            {
                throw new ArgumentOutOfRangeException("providerName", "There is no cache provider registered with name: " + providerName);
            }

            return _CacheConfiguration.Providers[providerName].Value;
        }
    }
}