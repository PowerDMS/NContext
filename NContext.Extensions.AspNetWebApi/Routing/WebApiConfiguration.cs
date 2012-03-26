// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfiguration.cs">
//   Copyright (c) 2012
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
//   Defines configuration support for ASP.NET Web API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace NContext.Extensions.AspNetWebApi.Routing
{
    /// <summary>
    /// Defines configuration support for ASP.NET Web API.
    /// </summary>
    public class WebApiConfiguration
    {
        private readonly Action<HttpConfiguration> _AspNetHttpConfigurationDelegate;

        private readonly Lazy<HttpSelfHostConfiguration> _HttpSelfHostConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiConfiguration"/> class.
        /// </summary>
        /// <param name="aspNetHttpConfigurationDelegate">The ASP net HTTP configuration delegate.</param>
        /// <param name="httpSelfHostConfiguration">The HTTP self host configuration.</param>
        /// <remarks></remarks>
        public WebApiConfiguration(Action<HttpConfiguration> aspNetHttpConfigurationDelegate, Lazy<HttpSelfHostConfiguration> httpSelfHostConfiguration)
        {
            _AspNetHttpConfigurationDelegate = aspNetHttpConfigurationDelegate;
            _HttpSelfHostConfiguration = httpSelfHostConfiguration;
        }

        /// <summary>
        /// Gets the <see cref="AspNetHttpConfigurationDelegate"/> instance.
        /// </summary>
        /// <remarks></remarks>
        public Action<HttpConfiguration> AspNetHttpConfigurationDelegate
        {
            get
            {
                return _AspNetHttpConfigurationDelegate;
            }
        }

        /// <summary>
        /// Gets the <see cref="HttpSelfHostConfiguration"/> instance.
        /// </summary>
        /// <remarks></remarks>
        public HttpSelfHostConfiguration HttpSelfHostConfiguration
        {
            get
            {
                return _HttpSelfHostConfiguration.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the API is self-hosted.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsSelfHosted
        {
            get
            {
                return _HttpSelfHostConfiguration != null;
            }
        }
    }
}