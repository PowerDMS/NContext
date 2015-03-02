namespace NContext.Extensions.ValueInjecter.Tests.Specs
{
    using System;

    using Machine.Specifications;

    using NContext.Extensions.ValueInjecter.Configuration;

    using Telerik.JustMock;

    public class when_injecting_with_fluent_value_injecter
    {
        static ValueInjecterManager _ValueInjecterManager;

        static Lazy<IFluentValueInjecter<DummyBlogPostDto>> _FluentValueInjecter;

        static DummyBlogPostDto _BlogPostDto;

        protected static IFluentValueInjecter<DummyBlogPostDto> FluentValueInjecter
        {
            get { return _FluentValueInjecter.Value; }
        }

        protected static DummyBlogPostDto BlogPostDto
        {
            get { return _BlogPostDto; }
        }

        Establish context = () =>
            {
                Mock.SetupStatic(typeof(ValueInjecterManager), Behavior.Loose, StaticConstructor.NonMocked);

                _ValueInjecterManager = Mock.Create<ValueInjecterManager>();

                Mock.Arrange(() => _ValueInjecterManager.Inject(Arg.IsAny<DummyBlogPostDto>()))
                    .CallOriginal();

                _BlogPostDto = new DummyBlogPostDto
                    {
                        Id = 1,
                        Post = "This is a test.",
                        Status = DummyBlogPostStatus.Draft,
                        Summary = "Post summary.",
                        Title = "Fluent Injection",
                        PublishedOn = new DateTime(2013, 7, 15, 0, 0, 0, DateTimeKind.Unspecified)
                    };

                _FluentValueInjecter = new Lazy<IFluentValueInjecter<DummyBlogPostDto>>(() => _ValueInjecterManager.Inject(BlogPostDto));
            };
    }
}