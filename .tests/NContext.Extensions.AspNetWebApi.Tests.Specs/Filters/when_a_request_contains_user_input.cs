// --------------------------------------------------------------------------------------------------------------------
// <copyright file="when_a_request_contains_user_input.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Filters
{
    using System;
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

    public class when_a_request_contains_user_input
    {
        Establish context = () =>
            {
                var config = new HttpConfiguration();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/blogs/5/posts");
                var route = config.Routes.MapHttpRoute("DefaultApi", "api/blogs/{blogId}/author/{bloggerName}/posts");
                var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "blogs", "dummyapi" } });
                var controller = new DummyApiController
                    {
                        ControllerContext = new HttpControllerContext(config, routeData, request)
                            {
                                ControllerDescriptor =
                                    new HttpControllerDescriptor(config, "dummyapi", typeof(DummyApiController))
                            },
                        Request = request
                    };

                controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                var blogPostsDto = fixture.CreateMany<DummyBlogPost>().ToList();

                var actionDescriptor = 
                    new ReflectedHttpActionDescriptor(
                        controller.ControllerContext.ControllerDescriptor, 
                        typeof(DummyApiController).GetMethod("PostBlogPosts", BindingFlags.Instance | BindingFlags.Public));

                _ActionContext = new HttpActionContext(controller.ControllerContext, actionDescriptor);
                _ActionContext.ActionArguments.Add("blogId", 5);
                _ActionContext.ActionArguments.Add("bloggerName", "danielgioulakis");
                _ActionContext.ActionArguments.Add("blogPosts", blogPostsDto);
                _ActionContext.ActionArguments.Add("publishAs", "DGDev");
                _ActionContext.ActionArguments.Add("publishAll", true);
                
                var sanitizer = Mock.Create<ISanitizeText>();
                Mock.Arrange(() => sanitizer.SanitizeHtmlFragment(Arg.AnyString)).Returns(_SanitizedResult);

                _Filter = Mock.Create<HttpParameterBindingSanitizerFilter>(c => c.CallConstructor(() => new HttpParameterBindingSanitizerFilter(sanitizer)));

                Mock.Arrange(() => _Filter.OnActionExecuting(Arg.IsAny<HttpActionContext>())).CallOriginal();
                Mock.NonPublic.Arrange<String>(_Filter, "SanitizeString", ArgExpr.IsAny<String>()).Returns(_SanitizedResult).Occurs(2);
                Mock.NonPublic.Arrange(_Filter, "SanitizeObjectGraph", ArgExpr.IsAny<Object>()).OccursOnce();
            };

        Because of = () => _Filter.OnActionExecuting(_ActionContext);

        It should_sanitize_all_user_submitted_content = () => Mock.Assert(_Filter);

        static HttpParameterBindingSanitizerFilter _Filter;

        static HttpActionContext _ActionContext;

        const string _SanitizedResult = @"ncontext";
    }
}