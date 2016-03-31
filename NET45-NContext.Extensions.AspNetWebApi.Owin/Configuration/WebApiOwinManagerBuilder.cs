namespace NContext.Extensions.AspNetWebApi.Owin.Configuration
{
    using System;
    using System.Web.Http;

    using global::Owin;

    using NContext.Configuration;
    using NContext.Extensions.AspNetWebApi.Configuration;

    public class WebApiOwinManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private IAppBuilder _AppBuilder;

        private Boolean _IsConfigured;

        private HttpConfiguration _HttpConfiguration;

        public WebApiOwinManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        public HttpConfiguration HttpConfiguration
        {
            get { return _HttpConfiguration ?? new HttpConfiguration(); }
            private set { _HttpConfiguration = value; }
        }

        public ApplicationConfigurationBuilder ConfigureForOwinSelfHost(IAppBuilder appBuilder)
        {
            _AppBuilder = appBuilder;
            Setup();

            return Builder;
        }

        public WebApiOwinManagerBuilder SetHttpConfiguration(HttpConfiguration httpConfiguration)
        {
            HttpConfiguration = httpConfiguration;
            return this;
        }

        protected override void Setup()
        {
            if (!_IsConfigured)
            {
                Builder.ApplicationConfiguration
                    .RegisterComponent<IManageWebApi>(
                        () => new WebApiOwinManager(_AppBuilder));

                _IsConfigured = true;
            }
        }
    }
}