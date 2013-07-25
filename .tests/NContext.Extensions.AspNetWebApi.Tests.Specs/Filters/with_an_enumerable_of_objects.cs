﻿namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Machine.Specifications;

    using NContext.Text;

    using Telerik.JustMock;

    public class with_an_enumerable_of_objects : when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                TextSanitizer = Mock.Create<ITextSanitizer>();

                Mock.Arrange(() => TextSanitizer.Sanitize(Arg.AnyString))
                    .Returns(_SanitizedValue);

                _Data = new Collection<Object>
                    {
                        5,
                        "Daniel",
                        null,
                        "Gioulakis",
                        ""
                    };
            };

        Because of = () => Sanitize(_Data);

        It should_sanitize_the_enumerables_strings = () => _Data.ShouldContainOnly(5, _SanitizedValue, null, _SanitizedValue, "");

        static IEnumerable<Object> _Data;

        const String _SanitizedValue = "ncontext";
    }
}