namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using NContext.Text;

    using Ploeh.AutoFixture;

    using Telerik.JustMock;

    public class with_configuration_attributes : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                TextSanitizer = Mock.Create<ISanitizeText>();

                Mock.Arrange(() => TextSanitizer.SanitizeHtml(Arg.AnyString))
                    .Returns(_HtmlValue);

                Mock.Arrange(() => TextSanitizer.SanitizeHtmlFragment(Arg.AnyString))
                    .Returns(_SanitizedValue);

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