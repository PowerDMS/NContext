namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class DummyApiController : ApiController
    {
        public HttpResponseMessage PostBlogPosts(Int32 blogId, String bloggerName, IEnumerable<DummyBlogPost> blogPosts, String publishAs = null, Boolean? publishAll = null)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}