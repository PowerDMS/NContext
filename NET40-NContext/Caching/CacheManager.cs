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

        /// <summary>
        /// Gets the provider by unique name.
        /// </summary>
        /// <typeparam name="TProvider">The type of the cache provider.</typeparam>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>ObjectCache.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">providerName;There is no cache provider registered with name:  + providerName</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Occurs when the cache provider associated with <paramref name="providerName"/> 
        /// is not of type <typeparamref name="TProvider"/>
        /// </exception>
        public TProvider GetProvider<TProvider>(String providerName) where TProvider : ObjectCache
        {
            if (!_CacheConfiguration.Providers.ContainsKey(providerName))
            {
                throw new ArgumentOutOfRangeException("providerName", "There is no cache provider registered with name: " + providerName);
            }

            var cacheProvider = _CacheConfiguration.Providers[providerName].Value;
            if (!(cacheProvider is TProvider))
            {
                throw new InvalidOperationException(String.Format("Cache provider '{0}' is not of type '{1}'.", providerName, typeof(TProvider).Name));
            }

            return (TProvider)cacheProvider;
        }
    }
}