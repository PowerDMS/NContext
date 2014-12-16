namespace NContext.Extensions.AspNetWebApi.Owin.Configuration
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Web.Http;

    using global::Owin;

    using NContext.Configuration;
    using NContext.Extensions.AspNetWebApi.Configuration;
    using NContext.Extensions.AspNetWebApi.Routing;

    public class WebApiOwinManager : WebApiManager
    {
        private readonly IAppBuilder _AppBuilder;

        private static readonly Lazy<HttpConfiguration> _HttpConfiguration =
            new Lazy<HttpConfiguration>(() => new HttpConfiguration());

        public WebApiOwinManager(IAppBuilder appBuilder)
            : base(null)
        {
            _AppBuilder = appBuilder;
        }

        /// <summary>
        /// Gets the OWIN application builder.
        /// </summary>
        /// <value>The application builder.</value>
        public IAppBuilder AppBuilder
        {
            get { return _AppBuilder; }
        }

        /// <summary>
        /// Configures the specified application configuration.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        public override void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (IsConfigured) return;

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageWebApi>(this);
            CompositionContainer = applicationConfiguration.CompositionContainer;

            var webApiConfigurations = CompositionContainer.GetExportedValues<IConfigureWebApi>();
            foreach (var webApiConfiguration in webApiConfigurations)
            {
                webApiConfiguration.Configure(HttpConfiguration);
            }

            var routingConfigurations = CompositionContainer.GetExportedValues<IConfigureHttpRouting>().OrderBy(c => c.Priority);
            foreach (var routingConfiguration in routingConfigurations)
            {
                routingConfiguration.Configure(this);
            }

            CreateRoutes();

            var owinConfigurations = CompositionContainer.GetExportedValues<IConfigureOwin>().OrderBy(oc => oc.Priority);
            foreach (var owinConfiguration in owinConfigurations)
            {
                owinConfiguration.Configure(AppBuilder);
            }

            AppBuilder.UseWebApi(HttpConfiguration);

            IsConfigured = true;
        }

        protected override HttpConfiguration CreateOrGetHttpConfiguration()
        {
            return _HttpConfiguration.Value;
        }

        /// <summary>
        /// Creates the routes.
        /// </summary>
        protected override void CreateRoutes()
        {
            HttpRoutes.ForEach(
                route =>
                {
                    HttpConfiguration
                        .Routes
                        .MapHttpRoute(route.RouteName, route.RouteTemplate, route.Defaults, route.Constraints);
                });
        }
    }
}