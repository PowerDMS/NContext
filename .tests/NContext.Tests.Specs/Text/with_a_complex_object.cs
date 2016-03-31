namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Text;

    using Ploeh.AutoFixture;

    public class with_a_complex_object : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                TextSanitizer = A.Fake<ISanitizeText>();

                A.CallTo(() => TextSanitizer.SanitizeHtmlFragment(A<string>._))
                    .Returns(_SanitizedValue);

                var fixture = new Fixture();
                fixture.Behaviors.Remove(fixture.Behaviors.Single(b => b is ThrowingRecursionBehavior));
                fixture.Behaviors.Add(new NullRecursionBehavior(2));

                _Data = fixture.CreateMany<DummyBlogPost>().ToList();
            };

        Because of = () => Sanitize(_Data);

        It should_sanitize_all_strings_in_the_object_graph = () =>
        {
            var data = _Data.ToList()[0];
            data.Tags.ShouldContainOnly(_SanitizedValue, _SanitizedValue, _SanitizedValue); // IEnumerable check
            data.Comments.Values.ShouldContainOnly(_SanitizedValue, _SanitizedValue, _SanitizedValue); // IDictionary check
            data.References.ShouldContainOnly(_SanitizedValue, _SanitizedValue, _SanitizedValue); // Concrete enumerable declaration check
            data.Links.Select(link => link.Text).ShouldContainOnly(_SanitizedValue, _SanitizedValue, _SanitizedValue); // Complex object array check
            data.Links.Select(link => link.Uri).ShouldContainOnly(_SanitizedValue, _SanitizedValue, _SanitizedValue); // Complex object array check
            data.Author.Email.ShouldEqual(_SanitizedValue); // Complex navigation property (object) -> property check
            data.Author.FirstName.ShouldEqual(_SanitizedValue); // Complex navigation property (object) -> property check
            data.Author.LastName.ShouldEqual(_SanitizedValue); // Complex navigation property (object) -> property check
            data.Author.Websites.ShouldContainOnly(_SanitizedValue, _SanitizedValue, _SanitizedValue); // Concrete enumerable declaration check
            data.Author.BlogPosts.First().Comments.Values.ShouldContainOnly(_SanitizedValue, _SanitizedValue, _SanitizedValue); // Circular reference check
        };

        static IEnumerable<DummyBlogPost> _Data;

        const String _SanitizedValue = "ncontext";
    }
}