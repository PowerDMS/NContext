namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using NContext.Text;

    using Telerik.JustMock;

    public class with_a_Dictionary_of_string_values : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = Mock.Create<ITextSanitizer>();

            Mock.Arrange(() => TextSanitizer.Sanitize(Arg.AnyString))
                .Returns(_SanitizedValue);

            _Data = new Dictionary<String, String>
                    {
                        { "FirstName", "Daniel" },
                        { "MiddleName", null },
                        { "LastName", "Gioulakis" },
                        { "Notes", "" }
                    };
        };

        Because of = () => Sanitize(_Data);

        It should_sanitize_only_dictionary_values = () => _Data.Select(item => item.Value).ShouldContainOnly(_SanitizedValue, null, _SanitizedValue, "");

        static IDictionary<String, String> _Data;

        const String _SanitizedValue = "ncontext";
    }

    public class with_a_Dictionary_of_object_values : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = Mock.Create<ITextSanitizer>();

            Mock.Arrange(() => TextSanitizer.Sanitize(Arg.AnyString))
                .Returns(_SanitizedValue);

            _Data = new Dictionary<String, Object>
                    {
                        { "Id", 5 },
                        { "FirstName", "Daniel" },
                        { "MiddleName", null },
                        { "LastName", "Gioulakis" },
                        { "Notes", "" }
                    };
        };

        Because of = () => Sanitize(_Data);

        It should_sanitize_only_dictionary_string_values = () => _Data.Select(item => item.Value).ShouldContainOnly(5, _SanitizedValue, null, _SanitizedValue, "");

        static IDictionary<String, Object> _Data;

        const String _SanitizedValue = "ncontext";
    }

    public class with_a_Dictionary_of_complex_object_values : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = Mock.Create<ITextSanitizer>();

            Mock.Arrange(() => TextSanitizer.Sanitize(Arg.AnyString))
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