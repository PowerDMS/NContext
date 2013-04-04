// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityConfigurationBuilder.cs" company="Waking Venture, Inc.">
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
    public class SecurityConfigurationBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private SecurityTokenExpirationPolicy _SecurityTokenExpirationPolicy;

        private String _CacheProviderName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public SecurityConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
            _SecurityTokenExpirationPolicy = new SecurityTokenExpirationPolicy();
        }

        public SecurityConfigurationBuilder SetCacheProviderName(String cacheProviderName)
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
        /// <returns>SecurityConfigurationBuilder.</returns>
        public SecurityConfigurationBuilder SetTokenExpirationPolicy(TimeSpan expiration, Boolean isAbsolute = false)
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