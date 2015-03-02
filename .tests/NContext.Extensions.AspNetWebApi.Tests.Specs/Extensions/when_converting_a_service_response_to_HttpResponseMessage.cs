namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Extensions
{
    using System;
    using System.Net.Http;
    using System.Web.Http;

    using Machine.Specifications;

    public abstract class when_converting_a_service_response_to_HttpResponseMessage
    {
        Establish context = () =>
        {
            Request = new HttpRequestMessage { RequestUri = new Uri("http://localhost/test") };
            Request.SetConfiguration(new HttpConfiguration());
        };

        protected static HttpRequestMessage Request;
    }
}