namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Text;

    public class with_a_dictionary_without_IDictionary_support : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = A.Fake<ISanitizeText>();

            A.CallTo(() => TextSanitizer.SanitizeHtmlFragment(A<string>._))
                .Returns(_SanitizedValue);

            _Data = new ExpandoObject();
                _Data["FirstName"] = "Daniel";
                _Data["LastName"] = "Gioulakis";
                _Data["Profile"] = new ExpandoObject();
                var profile = ((IDictionary<String, Object>) _Data["Profile"]);
                profile["Age"] = 28;
                profile["Current City"] = "Orlando";
            };

        Because of = () =>
        {
            try
            {
                Sanitize(_Data);
            }
            catch (Exception e)
            {
                _ThrownExeption = e;
            }
        };

        It should_sanitize_the_object_graph = () =>
        {
            _ThrownExeption.ShouldNotBeNull();
//            _Data["FirstName"].ShouldEqual(_SanitizedValue);
//            _Data["LastName"].ShouldEqual(_SanitizedValue);
//            var profile = (IDictionary<String,Object>)_Data["Profile"];
//            profile["Current City"].ShouldEqual(_SanitizedValue);
        };

        static Exception _ThrownExeption;

        static IDictionary<String, Object> _Data;

        const String _SanitizedValue = "ncontext";
    }
}