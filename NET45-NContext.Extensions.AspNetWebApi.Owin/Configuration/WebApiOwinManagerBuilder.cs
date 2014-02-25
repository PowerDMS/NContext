// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiOwinManagerBuilder.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2014 Waking Venture, Inc.
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

namespace NContext.Extensions.AspNetWebApi.Owin.Configuration
{
    using System;

    using global::Owin;

    using NContext.Configuration;
    using NContext.Extensions.AspNetWebApi.Configuration;

    public class WebApiOwinManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private IAppBuilder _AppBuilder;

        private Boolean _IsConfigured;

        public WebApiOwinManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        public ApplicationConfigurationBuilder ConfigureForOwinSelfHost(IAppBuilder appBuilder)
        {
            _AppBuilder = appBuilder;
            Setup();

            return Builder;
        }

        protected override void Setup()
        {
            if (!_IsConfigured)
            {
                Builder.ApplicationConfiguration
                    .RegisterComponent<IManageWebApi>(
                        () =>
                            new WebApiOwinManager(_AppBuilder));

                _IsConfigured = true;
            }
        }
    }
}