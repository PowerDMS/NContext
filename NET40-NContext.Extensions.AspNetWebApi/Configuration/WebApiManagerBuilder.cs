namespace NContext.Extensions.AspNetWebApi.Configuration
{
    using System;
    using System.Web.Http;
    using System.Web.Http.SelfHost;

    using NContext.Configuration;

    /// <summary>
    /// Defines a component configuration class for service routing.
    /// </summary>
    public class WebApiManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private Action<HttpConfiguration> _AspNetHttpConfigurationDelegate;

        private Lazy<HttpSelfHostConfiguration> _HttpSelfHostConfigurationFactory;

        private Boolean _IsConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public WebApiManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        /// <summary>
        /// Sets the delegate to use when configuring the application's <see cref="GlobalConfiguration.Configuration"/>.
        /// </summary>
        /// <returns>Current <see cref="WebApiManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ConfigureForIIS()
        {
            return ConfigureForIIS(null);
        }

        /// <summary>
        /// Sets the delegate to use when configuring the application's <see cref="GlobalConfiguration.Configuration"/>.
        /// </summary>
        /// <param name="configurationDelegate">The configuration delegate.</param>
        /// <returns>Current <see cref="WebApiManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public ApplicationConfigurationBuilder ConfigureForIIS(Action<HttpConfiguration> configurationDelegate)
        {
            _AspNetHttpConfigurationDelegate = configurationDelegate;
            Setup();

            return Builder;
        }

        /// <summary>
        /// Sets the <see cref="HttpSelfHostConfiguration" /> instance to be used when self-hosting the API, externally from ASP.NET.
        /// </summary>
        /// <param name="configurationDelegate">The configuration delegate.</param>
        /// <returns>Current <see cref="WebApiManagerBuilder" /> instance.</returns>
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