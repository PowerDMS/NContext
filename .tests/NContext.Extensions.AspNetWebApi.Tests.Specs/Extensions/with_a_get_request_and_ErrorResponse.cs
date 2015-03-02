namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Extensions
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using Machine.Specifications;

    using NContext.Common;
    using NContext.Extensions.AspNetWebApi.Extensions;

    public class with_a_get_request_and_ErrorResponse : when_converting_a_service_response_to_HttpResponseMessage
    {
        Establish context = () =>
        {
            Request.Method = HttpMethod.Get;
            _ServiceResponse = new ErrorResponse<int>(new Error(409, "key_already_exists", Enumerable.Empty<String>()));
        };
        
        Because of = () => _Response = _ServiceResponse.ToHttpResponseMessage(Request);

        It should_set_the_response_content = () => _Response.Content.ShouldNotBeNull();

        It should_set_the_response_status_code_to_the_error_code = () => _Response.StatusCode.ShouldEqual(HttpStatusCode.Conflict);

        private static IServiceResponse<Int32> _ServiceResponse;

        private static HttpResponseMessage _Response;
    }
}