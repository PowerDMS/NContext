namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Text;

    public class with_a_Dictionary_of_Object_values : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = A.Fake<ISanitizeText>();

            A.CallTo(() => TextSanitizer.SanitizeHtmlFragment(A<string>._))
                .Returns(_SanitizedValue);
            
            _Link = new DummyBlogLink { Text = "<script>alert('xss');</script>" };
            _NestedDictionary = new Dictionary<String, Object> { { "SomeKey", "<script>alert('xss');</script>" } };
            _NestedEnumerable = new Collection<Object> { "<script>alert('xss');</script>" };
            _Data = new Dictionary<String, Object>
                    {
                        { "Id", 5 },
                        { "FirstName", "Daniel" },
                        { "MiddleName", null },
                        { "LastName", "Gioulakis" },
                        { "Notes", "" },
                        { "BlogLink", _Link },
                        { "NestedDictionary", _NestedDictionary },
                        { "NestedEnumerable", _NestedEnumerable }
                    };
        };

        Because of = () => Sanitize(_Data);

        It should_sanitize_only_dictionary_string_values = () =>
        {
            _Data.Select(item => item.Value).ShouldContainOnly(5, _SanitizedValue, null, _SanitizedValue, "", _Link, _NestedDictionary, _NestedEnumerable);
            ((DummyBlogLink)_Data["BlogLink"]).Text.ShouldEqual(_SanitizedValue);
            ((IDictionary<String, Object>) _Data["NestedDictionary"])["SomeKey"].ShouldEqual(_SanitizedValue);
            ((IEnumerable<Object>)_Data["NestedEnumerable"]).ShouldContainOnly(_SanitizedValue);
        };

        static IDictionary<String, Object> _Data;

        static DummyBlogLink _Link;

        static IDictionary<String, Object> _NestedDictionary;

        static IEnumerable<Object> _NestedEnumerable;

        const String _SanitizedValue = "ncontext";
    }
}