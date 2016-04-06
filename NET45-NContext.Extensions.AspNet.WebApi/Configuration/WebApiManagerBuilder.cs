namespace NContext.Extensions.AspNet.WebApi.Configuration
{
    using System;
    using System.Web.Http;

    using NContext.Configuration;

    /// <summary>
    /// Defines a component configuration class for service routing.
    /// </summary>
    public class WebApiManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private Action _HostConfigurationAction;

        private Func<HttpConfiguration> _HttpConfigurationFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public WebApiManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        public Func<HttpConfiguration> HttpConfigurationFactory
        {
            get { return _HttpConfigurationFactory; }
            private set { _HttpConfigurationFactory = value; }
        }

        public Action HostConfigurationAction
        {
            get { return _HostConfigurationAction; }
            private set { _HostConfigurationAction = value; }
        }

        /// <summary>
        /// Sets the action to invoke for configuring the host.
        /// </summary>
        /// <remarks></remarks>
        public WebApiManagerBuilder SetHostConfigurationAction(Action hostConfigurationAction)
        {
            HostConfigurationAction = hostConfigurationAction;
            return this;
        }

        public WebApiManagerBuilder SetHttpConfigurationFactory(Func<HttpConfiguration> httpConfigurationFactory)
        {
            HttpConfigurationFactory = httpConfigurationFactory;
            return this;
        }

        /// <summary>
        /// Applies the component configuration with the <see cref="ApplicationConfigurationBase"/>.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                .RegisterComponent<IManageWebApi>(
                    () =>
                        new WebApiManager(
                            new WebApiConfiguration(HttpConfigurationFactory, HostConfigurationAction)));
        }
    }
}