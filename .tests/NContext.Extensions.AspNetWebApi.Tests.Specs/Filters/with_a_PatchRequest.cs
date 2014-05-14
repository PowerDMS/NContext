namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Serialization
{
    using System;
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

    public class with_a_PatchRequest : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        private Establish context = () =>
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

            var blogPost = fixture.Create<DummyBlogPost>();
            var blogPostJson = JsonConvert.SerializeObject(blogPost, settings);

            _Data = JsonConvert.DeserializeObject<PatchRequest<DummyBlogPost>>(blogPostJson, settings);
        };

        Because of = () => Sanitize(_Data);

        It should_sanitize_all_strings_in_the_dictionary_complex_object_values =
            () => _Data["Title"].ShouldEqual(_SanitizedValue);

        static PatchRequest<DummyBlogPost> _Data;

        const String _SanitizedValue = "ncontext";
    }
}