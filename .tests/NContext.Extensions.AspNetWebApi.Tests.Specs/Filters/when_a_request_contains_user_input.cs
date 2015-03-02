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