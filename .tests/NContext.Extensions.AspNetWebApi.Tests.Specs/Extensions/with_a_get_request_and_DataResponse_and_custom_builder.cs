namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Extensions
{
    using System;
    using System.Net;
    using System.Net.Http;

    using AspNet.WebApi.Extensions;

    using Machine.Specifications;

    using NContext.Common;

    public class with_a_get_request_and_DataResponse_and_custom_builder : when_converting_a_service_response_to_HttpResponseMessage
    {
        Establish context = () =>
        {
            Request.Method = HttpMethod.Get;
            _ServiceResponse = new DataResponse<int>(default(Int32));
        };

        Because of = () => _Response = _ServiceResponse.ToHttpResponseMessage(Request, (value, response) => response.StatusCode = HttpStatusCode.Found, false);

        It should_invoke_the_custom_builder = () => _Response.StatusCode.ShouldEqual(HttpStatusCode.Found);

        private static IServiceResponse<Int32> _ServiceResponse;

        private static HttpResponseMessage _Response;
    }
}