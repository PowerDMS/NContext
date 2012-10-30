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
    using System.Runtime.Caching;

    using NContext.Caching;
    using NContext.Configuration;

    /// <summary>
    /// Defines a configuration class to build the application's <see cref="SecurityManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class SecurityConfigurationBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private DateTimeOffset _TokenAbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;

        private TimeSpan _TokenSlidingExpiration = ObjectCache.NoSlidingExpiration;

        private TimeSpan _TokenInitialLifespan = new TimeSpan(0, 1, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public SecurityConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        /// <summary>
        /// Sets the default cache configuration settings for security token expiration.
        /// </summary>
        /// <param name="tokenAbsoluteExpiration">The token absolute expiration.</param>
        /// <param name="tokenInitialLifespan">The token initial lifespan.</param>
        /// <param name="tokenSlidingExpiration">The token sliding expiration.</param>
        /// <returns>This <see cref="SecurityConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public SecurityConfigurationBuilder SetDefaults(DateTimeOffset tokenAbsoluteExpiration, TimeSpan tokenInitialLifespan, TimeSpan tokenSlidingExpiration)
        {
            _TokenAbsoluteExpiration = tokenAbsoluteExpiration;
            _TokenSlidingExpiration = tokenSlidingExpiration;
            _TokenInitialLifespan = tokenInitialLifespan;

            return this;
        }

        /// <summary>
        /// Sets the application's security manager using 
        /// the configuration created from this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            var cachingManager = Builder.ApplicationConfiguration.GetComponent<IManageCaching>();

            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageSecurity>(
                    () =>
                    new SecurityManager(
                        cachingManager,
                        new SecurityConfiguration(_TokenAbsoluteExpiration, _TokenSlidingExpiration, _TokenInitialLifespan)));
        }
    }
}