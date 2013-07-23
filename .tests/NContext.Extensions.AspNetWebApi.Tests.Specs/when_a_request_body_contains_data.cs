// --------------------------------------------------------------------------------------------------------------------
// <copyright file="when_a_request_body_contains_data.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
// 
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.AspNetWebApi.Tests.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;
    
    using Ploeh.AutoFixture;

    using Machine.Specifications;

    using NContext.Text;
    using NContext.Extensions.AspNetWebApi.Filters;

    using Telerik.JustMock;

    public class when_a_request_body_contains_data
    {
        const string _SanitizedResult = @"Sanitized!";

        Establish context = () =>
            {
                var config = new HttpConfiguration();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/blogs/5/posts");
                var route = config.Routes.MapHttpRoute("DefaultApi", "api/blogs/{blogId}/posts");
                var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "blogs", "dummyapi" } });

                var controller = new DummyApiController();
                controller.ControllerContext = new HttpControllerContext(config, routeData, request);
                controller.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(config, "dummyapi", typeof(DummyApiController));
                controller.Request = request;
                controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

                var fixture = new Fixture().Customize(new CompositeCustomization(new StaticStringCustomization("NContext")));
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                _BlogPostsDto = fixture.CreateMany<DummyBlogPost>().ToList();

                var actionDescriptor = 
                    new ReflectedHttpActionDescriptor(
                        controller.ControllerContext.ControllerDescriptor, 
                        typeof(DummyApiController).GetMethod("PostBlogPosts", BindingFlags.Instance | BindingFlags.Public));

                _ActionContext = new HttpActionContext(controller.ControllerContext, actionDescriptor);
                _ActionContext.ActionArguments.Add("blogId", 5);
                _ActionContext.ActionArguments.Add("blogPosts", _BlogPostsDto);
                
                var sanitizer = Mock.Create<ITextSanitizer>();
                Mock.Arrange(() => sanitizer.Sanitize(Arg.AnyString))
                    .Returns(_SanitizedResult);

                _Filter = new FormatterParameterBindingSanitizerFilter(sanitizer);
            };

        Because of = () => _Filter.OnActionExecuting(_ActionContext);

        It should_sanitize_the_posted_object = () => _BlogPostsDto.ToList()[0].Tags.ToList()[0].ShouldEqual(_SanitizedResult);

        static IEnumerable<DummyBlogPost> _BlogPostsDto;

        static FormatterParameterBindingSanitizerFilter _Filter;

        static HttpActionContext _ActionContext;
    }

    internal class StaticStringCustomization : ICustomization
    {
        private readonly string _Value;

        public StaticStringCustomization(String value)
        {
            _Value = value;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new StringGenerator(() => _Value));
        }
    }
}