namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Filters
{
    using System;
    using System.Collections.Generic;

    using Machine.Specifications;

    using NContext.Text;

    using Telerik.JustMock;

    public class with_a_Dictionary_of_complex_object_values : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                TextSanitizer = Mock.Create<ISanitizeText>();

                Mock.Arrange(() => TextSanitizer.SanitizeHtmlFragment(Arg.AnyString))
                    .Returns(_SanitizedValue);

                _Data = new Dictionary<Int32, DummyBlogAuthor>
                    {
                        { 0, new DummyBlogAuthor{ AuthorId = Guid.Empty, FirstName = "Daniel", LastName = "Gioulakis", Email = null } },
                        { 1, new DummyBlogAuthor() },
                        { 2, null }
                    };
            };

        Because of = () => Sanitize(_Data);

        It should_sanitize_all_strings_in_the_dictionary_complex_object_values = () => _Data[0].FirstName.ShouldEqual(_Data[0].LastName).ShouldEqual(_SanitizedValue);

        static IDictionary<Int32, DummyBlogAuthor> _Data;

        const String _SanitizedValue = "ncontext";
    }
}