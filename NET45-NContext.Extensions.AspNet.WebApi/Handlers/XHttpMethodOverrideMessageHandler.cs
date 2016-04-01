namespace NContext.Extensions.AspNetWebApi.Handlers
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a <see cref="HttpMessageHandler"/> for supporting the HTTP X-HTTP-Method-Override header.
    /// </summary>
    public class XHttpMethodOverrideMessageHandler : DelegatingHandler
    {
        private const String _XHttpMethodOverride = @"X-HTTP-Method-Override";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains(_XHttpMethodOverride))
            {
                var httpMethod = request.Headers.GetValues(_XHttpMethodOverride).FirstOrDefault();
                if (!String.IsNullOrWhiteSpace(httpMethod))
                {
                    request.Method = new HttpMethod(httpMethod);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}