namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Text;

    public class with_a_Dictionary_of_String_values : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = A.Fake<ISanitizeText>();

            A.CallTo(() => TextSanitizer.SanitizeHtmlFragment(A<string>._))
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
}