namespace NContext.Tests.Specs.Text
{
    using System;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Text;

    public class with_a_generic_complex_object : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = A.Fake<ISanitizeText>();

            A.CallTo(() => TextSanitizer.SanitizeHtmlFragment(A<string>._))
                .Returns(_SanitizedValue);

            _Data = new DummyGeneric<int>
            {
                Title = "Daniel Gioulakis"
            };
        };

        Because of = () => Sanitize(_Data);

        It should_sanitize_the_strings = () => _Data.Title.ShouldEqual(_SanitizedValue);

        static DummyGeneric<int> _Data;

        const String _SanitizedValue = "ncontext";
    }
}