namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using NContext.Text;

    using Telerik.JustMock;

    public class with_a_Dictionary_of_Object_values : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = Mock.Create<ISanitizeText>();

            Mock.Arrange(() => TextSanitizer.SanitizeHtmlFragment(Arg.AnyString))
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
}