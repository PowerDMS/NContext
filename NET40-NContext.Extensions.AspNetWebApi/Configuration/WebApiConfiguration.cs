namespace NContext.Extensions.AspNetWebApi.Configuration
{
    using System;
    using System.Web.Http;

    /// <summary>
    /// Defines configuration support for ASP.NET Web API.
    /// </summary>
    public class WebApiConfiguration
    {
        private readonly Action _HostConfigurationAction;

        private readonly Func<HttpConfiguration> _HttpConfigurationFactory;
        
        public WebApiConfiguration(Func<HttpConfiguration> httpConfigurationFactory, Action hostConfigurationAction)
        {
            if (httpConfigurationFactory == null)
                throw new ArgumentNullException("httpConfigurationFactory");

            if (hostConfigurationAction == null)
                throw new ArgumentNullException("hostConfigurationAction");

            _HttpConfigurationFactory = httpConfigurationFactory;
            _HostConfigurationAction = hostConfigurationAction;
        }

        public Action HostConfigurationAction
        {
            get { return _HostConfigurationAction; }
        }

        public Func<HttpConfiguration> HttpConfigurationFactory
        {
            get { return _HttpConfigurationFactory; }
        }
    }
}