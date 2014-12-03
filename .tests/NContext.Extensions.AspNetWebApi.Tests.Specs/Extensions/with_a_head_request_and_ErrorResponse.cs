namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Extensions
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using Machine.Specifications;

    using NContext.Common;
    using NContext.Extensions.AspNetWebApi.Extensions;

    public class with_a_head_request_and_ErrorResponse : when_converting_a_service_response_to_HttpResponseMessage
    {
        Establish context = () =>
        {
            Request.Method = HttpMethod.Head;
            _ServiceResponse = new ErrorResponse<Int32>(new Error(409, "key_already_exists", Enumerable.Empty<String>()));
        };

        Because of = () => _Response = _ServiceResponse.ToHttpResponseMessage(Request);

        It should_not_set_the_response_content = () => _Response.Content.ShouldBeNull();

        private static IServiceResponse<Int32> _ServiceResponse;

        private static HttpResponseMessage _Response;
    }
}