namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using NContext.Extensions.AspNetWebApi.Patching;
    using NContext.Extensions.AspNetWebApi.Serialization;
    using NContext.Extensions.AspNetWebApi.Tests.Specs.Filters;
    using NContext.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Ploeh.AutoFixture;

    using Telerik.JustMock;

    public class with_an_enumerable_of_complex_objects : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = Mock.Create<ISanitizeText>();

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Converters.Add(new PatchRequestConverter());

            Mock.Arrange(() => TextSanitizer.SanitizeHtmlFragment(Arg.AnyString))
                .Returns(_SanitizedValue);

            var fixture = new Fixture();
            fixture.Behaviors.Remove(fixture.Behaviors.Single(b => b is ThrowingRecursionBehavior));
            fixture.Behaviors.Add(new NullRecursionBehavior(2));
            var blogPosts = fixture.CreateMany<DummyBlogPost>();
            var blogPostsJson = JsonConvert.SerializeObject(blogPosts, settings);

            _Data = JsonConvert.DeserializeObject<List<DummyBlogPost>>(blogPostsJson, settings);
        };

        Because of = () => Sanitize(_Data);

        It should_sanitize_all_strings_in_the_graph =
            () => _Data.ForEach(
                post =>
                {
                    post.Author.Email.ShouldEqual(_SanitizedValue);
                    post.Author.FirstName.ShouldEqual(_SanitizedValue);
                    post.Author.LastName.ShouldEqual(_SanitizedValue);
                    post.Author.Websites.ForEach(website => website.ShouldEqual(_SanitizedValue));
                    post.Comments.ForEach(comment => comment.Value.ShouldEqual(_SanitizedValue));
                    post.Content.ShouldEqual(_SanitizedValue);
                    post.Links.ForEach(link =>
                    {
                        link.Text.ShouldEqual(_SanitizedValue);
                        link.Uri.ShouldEqual(_SanitizedValue);
                    });
                    post.References.ForEach(reference => reference.ShouldEqual(_SanitizedValue));
                });

        static IEnumerable<DummyBlogPost> _Data;

        const String _SanitizedValue = "ncontext";
    }
}