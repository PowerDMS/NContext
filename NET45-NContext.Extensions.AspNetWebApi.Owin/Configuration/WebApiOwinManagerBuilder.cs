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