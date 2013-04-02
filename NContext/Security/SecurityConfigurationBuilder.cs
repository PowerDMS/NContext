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

    using NContext.Caching;
    using NContext.Configuration;

    /// <summary>
    /// Defines a configuration class to build the application's <see cref="SecurityManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class SecurityConfigurationBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private SecurityTokenExpirationPolicy _SecurityTokenExpirationPolicy;

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
            var cachingManager = Builder.ApplicationConfiguration.GetComponent<IManageCaching>();
            if (cachingManager == null)
            {
                throw new Exception("SecurityManager has a dependency on IManageCaching which doesn't exist in configuration.");
            }

            Builder.ApplicationConfiguration.RegisterComponent<IManageSecurity>(
                () =>
                new SecurityManager(cachingManager, new SecurityConfiguration(_SecurityTokenExpirationPolicy)));
        }
    }
}