namespace NContext.Extensions.AspNet.WebApi.Handlers
{
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a <see cref="HttpMessageHandler"/> for supporting the HTTP Accept-language header.
    /// </summary>
    public class AcceptLanguageMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.AcceptLanguage != null)
            {
                var languages = request.Headers.AcceptLanguage.OrderByDescending(language => language.Quality ?? 1);
                foreach (var language in languages)
                {
                    try
                    {
                        var culture = CultureInfo.GetCultureInfo(language.Value);
                        Thread.CurrentThread.CurrentCulture = culture;
                        Thread.CurrentThread.CurrentUICulture = culture;
                        break;
                    }
                    catch (CultureNotFoundException)
                    {
                    }
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}