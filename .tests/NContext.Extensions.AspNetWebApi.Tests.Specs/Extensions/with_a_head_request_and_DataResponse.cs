namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Extensions
{
    using System;
    using System.Net.Http;

    using AspNet.WebApi.Extensions;

    using Machine.Specifications;

    using NContext.Common;

    public class with_a_head_request_and_DataResponse : when_converting_a_service_response_to_HttpResponseMessage
    {
        Establish context = () =>
        {
            Request.Method = HttpMethod.Head;
            _ServiceResponse = new DataResponse<Int32>(default(Int32));
        };

        Because of = () => _Response = _ServiceResponse.ToHttpResponseMessage(Request);

        It should_not_set_the_response_content = () => _Response.Content.ShouldBeNull();

        private static IServiceResponse<Int32> _ServiceResponse;

        private static HttpResponseMessage _Response;
    }
}