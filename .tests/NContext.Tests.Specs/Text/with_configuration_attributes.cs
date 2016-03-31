namespace NContext.Tests.Specs.Text
{
    using System;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Text;

    public class with_configuration_attributes : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                TextSanitizer = A.Fake<ISanitizeText>();

                A.CallTo(() => TextSanitizer.SanitizeHtmlFragment(A<string>._))
                    .Returns(_SanitizedValue);

                A.CallTo(() => TextSanitizer.SanitizeHtml(A<string>._))
                    .Returns(_HtmlValue);

                _Data = new DummySite
                {
                    Description = _UnsanitizedValue,
                    Message = _UnsanitizedValue,
                    Name = _UnsanitizedValue
                };
            };

        Because of = () => Sanitize(_Data);

        It should_obey_marker_attributes = () =>
        {
            _Data.Description.ShouldEqual(_SanitizedValue);
            _Data.Message.ShouldEqual(_HtmlValue);
            _Data.Name.ShouldEqual(_UnsanitizedValue);
        };

        static DummySite _Data;

        const String _SanitizedValue = "ncontext";

        const String _HtmlValue = "html";

        const String _UnsanitizedValue = "unsanitized";
    }
}