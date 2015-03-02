namespace NContext.Extensions.ValueInjecter.Tests.Specs
{
    using System;

    using Machine.Specifications;

    using Telerik.JustMock;

    public class with_a_custom_mapper : when_injecting_with_fluent_value_injecter
    {
        static Guid _BlogId;

        static DummyBlogPost _BlogPost;

        Establish context = () =>
            {
                _BlogId = Guid.NewGuid();
            };

        Because of = () => _BlogPost = FluentValueInjecter.Into<DummyBlogPost>(
            new
                {
                    BlogId = _BlogId
                });

        It should_have_the_mapped_value = () => _BlogPost.BlogId.ShouldEqual(_BlogId);
    }
}