namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using NContext.Text;

    using Ploeh.AutoFixture;

    using Telerik.JustMock;

    public class with_a_complex_object : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                TextSanitizer = Mock.Create<ISanitizeText>();

                Mock.Arrange(() => TextSanitizer.SanitizeHtmlFragment(Arg.AnyString))
                    .Returns(_SanitizedValue);

                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                _Data = fixture.CreateMany<DummyBlogPost>().ToList();
            };

        Because of = () => Sanitize(_Data);

        It should_sanitize_all_strings_in_the_object_graph = () => _Data.ToList()[0].Tags.ToList().ShouldContainOnly(_SanitizedValue, _SanitizedValue, _SanitizedValue);

        static IEnumerable<DummyBlogPost> _Data;

        const String _SanitizedValue = "ncontext";
    }
}