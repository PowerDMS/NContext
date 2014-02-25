// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiOwinManager.cs" company="Waking Venture, Inc.">
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