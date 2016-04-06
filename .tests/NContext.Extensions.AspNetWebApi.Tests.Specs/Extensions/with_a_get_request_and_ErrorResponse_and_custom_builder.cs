namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Extensions
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using AspNet.WebApi.Extensions;

    using Machine.Specifications;

    using NContext.Common;

    public class with_a_get_request_and_ErrorResponse_and_custom_builder : when_converting_a_service_response_to_HttpResponseMessage
    {
        Establish context = () =>
        {
            Request.Method = HttpMethod.Get;
            _ServiceResponse = new ErrorResponse<Int32>(new Error(409, "key_already_exists", Enumerable.Empty<String>()));
        };

        Because of = () => _Response = _ServiceResponse.ToHttpResponseMessage(Request, (value, response) => response.StatusCode = HttpStatusCode.OK, false);

        It should_not_invoke_the_custom_builder = () => _Response.StatusCode.ShouldEqual(HttpStatusCode.Conflict);

        private static IServiceResponse<Int32> _ServiceResponse;

        private static HttpResponseMessage _Response;
    }
}