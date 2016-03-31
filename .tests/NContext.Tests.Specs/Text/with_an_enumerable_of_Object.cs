namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using FakeItEasy;

    using Machine.Specifications;

    using NContext.Text;

    public class with_an_enumerable_of_Object : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
        {
            TextSanitizer = A.Fake<ISanitizeText>();

            A.CallTo(() => TextSanitizer.SanitizeHtmlFragment(A<string>._))
                .Returns(_SanitizedValue);

            _Link = new DummyBlogLink { Text = "<script>alert('xss');</script>" };
                _NestedDictionary = new Dictionary<String, Object> { { "SomeKey", "<script>alert('xss');</script>" } };
                _NestedEnumerable = new Collection<Object> { "<script>alert('xss');</script>" };
                _Data = new Collection<Object>
                    {
                        5,
                        "Daniel",
                        null,
                        "Gioulakis",
                        "",
                        _Link,
                        _NestedDictionary,
                        _NestedEnumerable
                    };
            };

        Because of = () => Sanitize(_Data);

        It should_sanitize_the_strings = () =>
        {
            _Data.ShouldContainOnly(5, _SanitizedValue, null, _SanitizedValue, "", _Link, _NestedDictionary, _NestedEnumerable);
            ((DummyBlogLink) _Data.ElementAt(5)).Text.ShouldEqual(_SanitizedValue);
            ((IDictionary<String, Object>) _Data.ElementAt(6))["SomeKey"].ShouldEqual(_SanitizedValue);
            ((IEnumerable<Object>) _Data.ElementAt(7)).ShouldContainOnly(_SanitizedValue);
        };

        static IEnumerable<Object> _Data;

        static DummyBlogLink _Link;

        static IDictionary<String, Object> _NestedDictionary;

        static IEnumerable<Object> _NestedEnumerable;

        const String _SanitizedValue = "ncontext";
    }
}