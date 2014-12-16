namespace NContext.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    /// <summary>
    /// Defines configuration settings for caching.
    /// </summary>
    public class CacheConfiguration
    {
        private readonly IDictionary<String, Lazy<ObjectCache>> _Providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheConfiguration"/> class.
        /// </summary>
        /// <param name="providers">The cache providers.</param>
        public CacheConfiguration(IDictionary<String, Lazy<ObjectCache>> providers)
        {
            _Providers = providers;
        }

        /// <summary>
        /// Gets the cache providers.
        /// </summary>
        /// <value>The providers.</value>
        public IDictionary<String, Lazy<ObjectCache>> Providers
        {
            get { return _Providers; }
        }
    }
}