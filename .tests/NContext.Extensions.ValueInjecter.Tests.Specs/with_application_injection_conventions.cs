// --------------------------------------------------------------------------------------------------------------------
// <copyright file="with_application_injection_conventions.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
// 
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.ValueInjecter.Tests.Specs
{
    using System;

    using Machine.Specifications;

    using Omu.ValueInjecter;

    using Telerik.JustMock;

    public class with_application_injection_conventions : when_injecting_with_fluent_value_injecter
    {
        static DummyBlogPost _BlogPost;

        static ClientToUtcDateTimeConvention _Convention;

        Establish context = () =>
            {
                _Convention = Mock.Create<ClientToUtcDateTimeConvention>(
                    config => config.CallConstructor(() => new ClientToUtcDateTimeConvention(TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time")))
                                    .SetBehavior(Behavior.CallOriginal));
                
                Mock.Arrange(() => ValueInjecterManager.Conventions)
                    .Returns(new[]
                        {
                            new Func<IValueInjection>(() => _Convention)
                        });
            };

        Because of = () => _BlogPost = FluentValueInjecter.Into<DummyBlogPost>();

        It should_invoke_all_injection_convention_map_methods = () => Mock.Assert(() => _Convention.Map(Arg.AnyObject, Arg.AnyObject), Occurs.Once());

        It should_apply_applicable_injection_conventions = () => _BlogPost.Id.ShouldEqual(BlogPostDto.Id);
    }
}