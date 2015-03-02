namespace NContext.Extensions.AspNetWebApi.Configuration
{
    using System;
    using System.Web.Http;
    using System.Web.Http.SelfHost;

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
        protected internal Boolean IsSelfHosted
        {
            get
            {
                return _HttpSelfHostConfiguration != null;
            }
        }
    }
}