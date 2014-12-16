namespace NContext.Security
{
    using System;
    using System.Security.Principal;
    using NContext.Caching;
    using NContext.Configuration;

    /// <summary>
    /// Defines a configuration class to build the application's <see cref="SecurityManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class SecurityManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private SecurityTokenExpirationPolicy _SecurityTokenExpirationPolicy;

        private String _CacheProviderName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityManagerBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public SecurityManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
            _SecurityTokenExpirationPolicy = new SecurityTokenExpirationPolicy();
        }

        public SecurityManagerBuilder SetCacheProviderName(String cacheProviderName)
        {
            _CacheProviderName = cacheProviderName;

            return this;
        }

        /// <summary>
        /// Sets the security token expiration policy. When used with <see cref="SecurityManager.SavePrincipal"/>,
        /// <see cref="SecurityManager"/> will cache the <see cref="IPrincipal"/> instance that is associated with the token.
        /// Subsequent calls to <see cref="SecurityManager.GetPrincipal"/> will return the cached <see cref="IPrincipal"/>. 
        /// 
        /// By default security tokens / principals do not expire. By setting an <paramref name="expiration"/> value, 
        /// the token will expire from cache. 
        /// 
        /// If <paramref name="isAbsolute"/> is false, then the cache entry will be evicted if it has not been accessed 
        /// in the given span of time (<paramref name="expiration"/>).
        /// 
        /// If <paramref name="isAbsolute"/> is true, then the cache entry will be evicted after a specified duration 
        /// (the <see cref="DateTime"/> the cache entry was created + <paramref name="expiration"/>).
        /// </summary>
        /// <param name="expiration">The expiration time span.</param>
        /// <param name="isAbsolute">The method which to use for evicting cached token / principals.</param>
        /// <returns>SecurityManagerBuilder.</returns>
        public SecurityManagerBuilder SetTokenExpirationPolicy(TimeSpan expiration, Boolean isAbsolute = false)
        {
            _SecurityTokenExpirationPolicy = new SecurityTokenExpirationPolicy(expiration, isAbsolute);

            return this;
        }

        /// <summary>
        /// Sets the application's security manager using 
        /// the configuration created from this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            var cacheManager = Builder.ApplicationConfiguration.GetComponent<IManageCaching>();
            if (cacheManager == null)
            {
                throw new Exception("SecurityManager has a dependency on IManageCaching which doesn't exist in configuration.");
            }

            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageSecurity>(() => new SecurityManager(
                       cacheManager.GetProvider(_CacheProviderName), 
                       new SecurityConfiguration(_SecurityTokenExpirationPolicy)));
        }
    }
}