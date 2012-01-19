// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityManager.cs">
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
//
// <summary>
//   Defines a manager class which implements logic for application security-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IdentityModel.Tokens;
using System.Runtime.Caching;
using System.Security.Principal;

using NContext.Application.Caching;
using NContext.Application.Configuration;

namespace NContext.Application.Security
{
    /// <summary>
    /// Defines a manager class which implements logic for application security-related operations.
    /// </summary>
    /// <remarks></remarks>
    public class SecurityManager : ISecurityManager
    {
        #region Fields

        private ICacheManager _CacheManager;

        private readonly CacheItemPolicy _AuthenticationCachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration =
                    _TokenAbsoluteExpiration == ObjectCache.InfiniteAbsoluteExpiration
                        ? ObjectCache.InfiniteAbsoluteExpiration
                        : DateTimeOffset.Now.Add(_TokenInitialLifespan),
                SlidingExpiration = _TokenSlidingExpiration,
            };

        private static DateTimeOffset _TokenAbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;

        private static TimeSpan _TokenSlidingExpiration = ObjectCache.NoSlidingExpiration;

        private static TimeSpan _TokenInitialLifespan = new TimeSpan(0, 1, 0, 0);

        private static Boolean _IsConfigured;

        private readonly SecurityConfiguration _SecurityConfiguration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityManager"/> class.
        /// </summary>
        /// <param name="securityConfiguration">The security configuration.</param>
        /// <remarks></remarks>
        public SecurityManager(SecurityConfiguration securityConfiguration)
        {
            if (securityConfiguration == null)
            {
                throw new ArgumentNullException("securityConfiguration");
            }

            _SecurityConfiguration = securityConfiguration;
        }

        #endregion

        #region Properties

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
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves the principal to cache using the application's <see cref="ICacheManager"/>.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>A token used to retrieve the <see cref="IPrincipal"/> from cache.</returns>
        /// <remarks></remarks>
        public virtual SecurityToken SavePrincipal(IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var token = new GuidToken();
            if (_CacheManager.AddOrUpdateItem(token.Id, principal, _AuthenticationCachePolicy))
            {
                return token;
            }

            // TODO: (DG) Logging
            // Could not add/update cache entry.
            return null;
        }

        /// <summary>
        /// Saves the principal to cache using the application's <see cref="ICacheManager"/>.
        /// </summary>
        /// <typeparam name="TToken">The type of the token.</typeparam>
        /// <param name="principal">The principal.</param>
        /// <returns>A <typeparamref name="TToken"/> token used to retrieve the <see cref="IPrincipal"/> from cache.</returns>
        /// <remarks></remarks>
        public virtual TToken SavePrincipal<TToken>(IPrincipal principal) where TToken : SecurityToken
        {
            throw new NotSupportedException("This method is not supported on the default implementation of SecurityManager.");
        }

        /// <summary>
        /// Adds or updates the cached principal associated with the specified <see cref="SecurityToken"/>, using the application's <see cref="ICacheManager"/>.
        /// </summary>
        /// <param name="principal">The cached <see cref="IPrincipal"/> instance.</param>
        /// <param name="token">The <see cref="SecurityToken"/> to associate with <paramref name="principal"/>.</param>
        /// <remarks></remarks>
        public void SavePrincipal(IPrincipal principal, SecurityToken token)
        {
            if (!_CacheManager.AddOrUpdateItem(token.Id, principal, _AuthenticationCachePolicy))
            {
                // TODO: (DG) Log internal
                // Could not update cache entry.
            }
        }

        /// <summary>
        /// Expires the principal associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <remarks></remarks>
        public void ExpirePrincipal(SecurityToken token)
        {
            _CacheManager.Remove(token.Id);
        }

        /// <summary>
        /// Gets the cached <see cref="IPrincipal"/> instance associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Instance of <see cref="IPrincipal"/> if found in cache, else null.</returns>
        /// <remarks></remarks>
        public IPrincipal GetPrincipal(SecurityToken token)
        {
            return _CacheManager.Get<IPrincipal>(token.Id);
        }

        /// <summary>
        /// Gets the cached <typeparamref name="TPrincipal"/> instance associated with the specified <see cref="SecurityToken"/>.
        /// </summary>
        /// <typeparam name="TPrincipal">The type of the principal.</typeparam>
        /// <param name="token">The token.</param>
        /// <returns>Instance of <typeparamref name="TPrincipal"/> if found in cache, else null.</returns>
        /// <remarks></remarks>
        public TPrincipal GetPrincipal<TPrincipal>(SecurityToken token) where TPrincipal : class, IPrincipal
        {
            return _CacheManager.Get<TPrincipal>(token.Id);
        }

        #endregion

        #region Implementation of IApplicationComponent

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public virtual void Configure(IApplicationConfiguration applicationConfiguration)
        {
            if (!_IsConfigured)
            {
                _CacheManager = applicationConfiguration.GetComponent<ICacheManager>();
                if (_CacheManager == null)
                {
                    throw new Exception("SecurityManager has a dependency on an instance of ICacheManager.");
                }

                _TokenAbsoluteExpiration = _SecurityConfiguration.TokenAbsoluteExpiration;
                _TokenSlidingExpiration = _SecurityConfiguration.TokenSlidingExpiration;
                _TokenInitialLifespan = _SecurityConfiguration.TokenInitialLifespan;

//                Ignore below for now until OAuth2 and WIF support has been added.

//                unityContainer.RegisterType<IAuthorizationProviderInstrumentationProvider, NullAuthorizationProviderInstrumentationProvider>();
//                unityContainer.RegisterType<IAuthorizationProvider, PublicAuthorizationProvider>(NetworkAuthorization.Public.ToDescriptionValue());
//                unityContainer.RegisterType<IAuthorizationProvider, ImplicitAuthorizationProvider>(NetworkAuthorization.Implicit.ToDescriptionValue());
//                unityContainer.RegisterType<IAuthorizationProvider, SiteCredentialsAuthorizationProvider>(NetworkAuthorization.SiteCredentials.ToDescriptionValue());
//                unityContainer.RegisterType<IAuthorizationProvider, SiteAndUserCredentialsAuthorizationProvider>(NetworkAuthorization.SiteAndUserCredentials.ToDescriptionValue());
//                unityContainer.RegisterType<IAuthorizationProvider, MacAuthorizationProvider>(NetworkAuthorization.Mac.ToDescriptionValue());

                _IsConfigured = true;
            }
        }

        #endregion
    }
}