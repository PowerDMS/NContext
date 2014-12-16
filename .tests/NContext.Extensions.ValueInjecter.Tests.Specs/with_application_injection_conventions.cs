namespace NContext.Extensions.ValueInjecter.Tests.Specs
{
    using System;

    using Machine.Specifications;

    using NContext.Extensions.ValueInjecter.Configuration;

    using Omu.ValueInjecter;

    using Telerik.JustMock;

    public class with_application_injection_conventions : when_injecting_with_fluent_value_injecter
    {
        static DummyBlogPost _BlogPost;

        static ClientToUtcDateTimeConvention _Convention;

        Establish context = () =>
            {
                _Convention = Mock.Create<ClientToUtcDateTimeConvention>(
                    config => config.CallConstructor(() => new ClientToUtcDateTimeConvention(TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time")))
                                    .SetBehavior(Behavior.CallOriginal));
                
                Mock.Arrange(() => ValueInjecterManager.Conventions)
                    .Returns(new[]
                        {
                            new Func<IValueInjection>(() => _Convention)
                        });
            };

        Because of = () => _BlogPost = FluentValueInjecter.Into<DummyBlogPost>();

        It should_invoke_all_injection_convention_map_methods = () => Mock.Assert(() => _Convention.Map(Arg.AnyObject, Arg.AnyObject), Occurs.Once());

        It should_apply_applicable_injection_conventions = () => _BlogPost.Id.ShouldEqual(BlogPostDto.Id);
    }
}