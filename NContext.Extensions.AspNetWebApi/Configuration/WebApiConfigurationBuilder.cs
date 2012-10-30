// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfigurationBuilder.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AspNetWebApi.Configuration
{
    using System;
    using System.Web.Http;
    using System.Web.Http.SelfHost;

    using NContext.Configuration;

    /// <summary>
    /// Defines a component configuration class for service routing.
    /// </summary>
    public class WebApiConfigurationBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private Action<HttpConfiguration> _AspNetHttpConfigurationDelegate;

        private Lazy<HttpSelfHostConfiguration> _HttpSelfHostConfigurationFactory;

        private Boolean _IsConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public WebApiConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        /// <summary>
        /// Sets the delegate to use when configuring the application's <see cref="GlobalConfiguration.Configuration"/>.
        /// </summary>
        /// <param name="configurationDelegate">The configuration delegate.</param>
        /// <returns>Current <see cref="WebApiConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ConfigureForAspNet(Action<HttpConfiguration> configurationDelegate)
        {
            _AspNetHttpConfigurationDelegate = configurationDelegate;
            Setup();

            return Builder;
        }

        /// <summary>
        /// Sets the <see cref="HttpSelfHostConfiguration" /> instance to be used when self-hosting the API, externally from ASP.NET.
        /// </summary>
        /// <param name="configurationDelegate">The configuration delegate.</param>
        /// <returns>Current <see cref="WebApiConfigurationBuilder" /> instance.</returns>
        public ApplicationConfigurationBuilder ConfigureForSelfHosting(Func<HttpSelfHostConfiguration> configurationDelegate)
        {
            _HttpSelfHostConfigurationFactory = new Lazy<HttpSelfHostConfiguration>(configurationDelegate);
            Setup();

            return Builder;
        }

        /// <summary>
        /// Applies the component configuration with the <see cref="ApplicationConfigurationBase"/>.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            if (!_IsConfigured)
            {
                Builder.ApplicationConfiguration
                    .RegisterComponent<IManageWebApi>(
                        () => 
                            new WebApiManager(
                                new WebApiConfiguration(_AspNetHttpConfigurationDelegate, _HttpSelfHostConfigurationFactory)));

                _IsConfigured = true;
            }
        }
    }
}