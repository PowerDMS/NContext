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
        private readonly IDictionary<String, Lazy<ObjectCache>> _Providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManagerBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public CacheManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
            _Providers = new Dictionary<String, Lazy<ObjectCache>>();
        }

        /// <summary>
        /// Sets the cache provider.
        /// </summary>
        /// <typeparam name="TCacheProvider">The type of the cache provider.</typeparam>
        /// <param name="providerName">A name to uniquely identify the cache provider.</param>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <returns>This <see cref="CacheManagerBuilder"/> instance.</returns>
        public CacheManagerBuilder AddProvider<TCacheProvider>(String providerName, Func<TCacheProvider> cacheProvider) where TCacheProvider : ObjectCache
        {
            if (_Providers.ContainsKey(providerName))
            {
                throw new ArgumentException("Provider key already exists in collection.", "providerName");
            }

            _Providers.Add(providerName, new Lazy<ObjectCache>(cacheProvider));

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