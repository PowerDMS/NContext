namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;

    using Machine.Specifications;

    using NContext.Text;

    using Telerik.JustMock;

    public class with_a_derived_enumerable_of_Object : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                TextSanitizer = Mock.Create<ISanitizeText>();

                Mock.Arrange(() => TextSanitizer.SanitizeHtmlFragment(Arg.AnyString))
                    .Returns(_SanitizedValue);

                _Data = new DummyEnumerable
                    {
                        5,
                        "Daniel",
                        null,
                        "Gioulakis",
                        ""
                    };
            };

        Because of = () => Sanitize(_Data);

        It should_sanitize_the_strings = () => _Data.ShouldContainOnly(5, _SanitizedValue, null, _SanitizedValue, "");

        static IEnumerable<Object> _Data;

        const String _SanitizedValue = "ncontext";
    }
}