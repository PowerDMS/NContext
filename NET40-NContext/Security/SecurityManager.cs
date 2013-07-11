// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityManager.cs" company="Waking Venture, Inc.">
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
    using System.ComponentModel.Composition;
    using System.IdentityModel.Tokens;
    using System.Runtime.Caching;
    using System.Security.Principal;

    using NContext.Caching;
    using NContext.Common;
    using NContext.Configuration;
    using NContext.Extensions;

    /// <summary>
    /// Defines a manager class which implements logic for application security-related operations.
    /// </summary>
    /// <remarks></remarks>
    [ApplicationComponentDependency(typeof(IManageCaching))]
    public class SecurityManager : IManageSecurity
    {
        private readonly ObjectCache _CacheProvider;

        private readonly SecurityConfiguration _SecurityConfiguration;

        private Boolean _IsConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityManager"/> class.
        /// </summary>
        /// <param name="cacheProvider">The caching manager.</param>
        /// <param name="securityConfiguration">The security configuration.</param>
        /// <remarks></remarks>
        public SecurityManager(ObjectCache cacheProvider, SecurityConfiguration securityConfiguration)
        {
            if (cacheProvider == null)
            {
                throw new Exception("SecurityManager has a dependency on an instance of IManageCaching.");
            }

            if (securityConfiguration == null)
            {
                throw new ArgumentNullException("securityConfiguration");
            }

            _CacheProvider = cacheProvider;
            _SecurityConfiguration = securityConfiguration;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <remarks></remarks>
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
        /// Gets the provider used to cache the <see cref="IToken"/> and <see cref="IPrincipal"/>.
        /// </summary>
        /// <value>The cache provider.</value>
        protected ObjectCache CacheProvider
        {
            get
            {
                return _CacheProvider;
            }
        }

        /// <summary>
        /// Saves the principal to cache using the application's <see cref="IManageCaching"/>.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>A token used to retrieve the <see cref="IPrincipal"/> from cache.</returns>
        /// <remarks></remarks>
        public virtual IToken SavePrincipal(IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var token = new GuidToken();
            SavePrincipal(principal, token);

            return token;
        }

        /// <summary>
        /// Saves the principal to cache using the application's <see cref="IManageCaching"/>.
        /// </summary>
        /// <typeparam name="TToken">The type of the token.</typeparam>
        /// <param name="principal">The principal.</param>
        /// <returns>A <typeparamref name="TToken"/> token used to retrieve the <see cref="IPrincipal"/> from cache.</returns>
        /// <remarks></remarks>
        public TToken SavePrincipal<TToken>(IPrincipal principal) where TToken : class, IToken, new()
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var token = Activator.CreateInstance<TToken>();
            SavePrincipal(principal, token);

            return token;
        }

        /// <summary>
        /// Adds or updates the cached principal associated with the specified <see cref="SecurityToken"/>, using the application's <see cref="IManageCaching"/>.
        /// </summary>
        /// <param name="principal">The cached <see cref="IPrincipal"/> instance.</param>
        /// <param name="token">The <see cref="SecurityToken"/> to associate with <paramref name="principal"/>.</param>
        /// <remarks></remarks>
        public virtual void SavePrincipal(IPrincipal principal, IToken token)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            CacheProvider.Set(token.Value, principal, CreateExpirationPolicy());
        }

        /// <summary>
        /// Expires the principal associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <remarks></remarks>
        public virtual void ExpirePrincipal(IToken token)
        {
            CacheProvider.Remove(token.Value);
        }

        /// <summary>
        /// Gets the cached <see cref="IPrincipal"/> instance associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Instance of <see cref="IPrincipal"/> if found in cache, else null.</returns>
        /// <remarks></remarks>
        public IPrincipal GetPrincipal(IToken token)
        {
            return GetPrincipal<IPrincipal>(token);
        }

        /// <summary>
        /// Gets the cached <typeparamref name="TPrincipal"/> instance associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <typeparam name="TPrincipal">The type of the principal.</typeparam>
        /// <param name="token">The token.</param>
        /// <returns>Instance of <typeparamref name="TPrincipal"/> if found in cache, else null.</returns>
        /// <remarks></remarks>
        public virtual TPrincipal GetPrincipal<TPrincipal>(IToken token) where TPrincipal : class, IPrincipal
        {
            return CacheProvider.Get<TPrincipal>(token.Value);
        }

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (!_IsConfigured)
            {
                applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageSecurity>(this);
                _IsConfigured = true;
            }
        }

        private CacheItemPolicy CreateExpirationPolicy()
        {
            return new CacheItemPolicy
            {
                AbsoluteExpiration =
                    _SecurityConfiguration.TokenExpirationPolicy.Expires && 
                    _SecurityConfiguration.TokenExpirationPolicy.IsAbsolute
                        ? DateTimeOffset.Now.Add(_SecurityConfiguration.TokenExpirationPolicy.ExpirationTime)
                        : DateTimeOffset.MaxValue,

                SlidingExpiration = 
                    _SecurityConfiguration.TokenExpirationPolicy.Expires &&
                    !_SecurityConfiguration.TokenExpirationPolicy.IsAbsolute
                        ? _SecurityConfiguration.TokenExpirationPolicy.ExpirationTime
                        : TimeSpan.Zero
            };
        }
    }
}