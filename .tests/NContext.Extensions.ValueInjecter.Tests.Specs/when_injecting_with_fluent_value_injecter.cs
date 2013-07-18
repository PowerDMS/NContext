// --------------------------------------------------------------------------------------------------------------------
// <copyright file="when_injecting_with_fluent_value_injecter.cs" company="Waking Venture, Inc.">
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

    using Telerik.JustMock;

    public class when_injecting_with_fluent_value_injecter
    {
        static ValueInjecterManager _ValueInjecterManager;

        static Lazy<IFluentValueInjecter<DummyBlogPostDto>> _FluentValueInjecter;

        static DummyBlogPostDto _BlogPostDto;

        protected static IFluentValueInjecter<DummyBlogPostDto> FluentValueInjecter
        {
            get { return _FluentValueInjecter.Value; }
        }

        protected static DummyBlogPostDto BlogPostDto
        {
            get { return _BlogPostDto; }
        }

        Establish context = () =>
            {
                Mock.SetupStatic(typeof(ValueInjecterManager), Behavior.Loose, StaticConstructor.NonMocked);

                _ValueInjecterManager = Mock.Create<ValueInjecterManager>();

                Mock.Arrange(() => _ValueInjecterManager.Inject(Arg.IsAny<DummyBlogPostDto>()))
                    .CallOriginal();

                _BlogPostDto = new DummyBlogPostDto
                    {
                        Id = 1,
                        Post = "This is a test.",
                        Status = DummyBlogPostStatus.Draft,
                        Summary = "Post summary.",
                        Title = "Fluent Injection",
                        PublishedOn = new DateTime(2013, 7, 15, 0, 0, 0, DateTimeKind.Unspecified)
                    };

                _FluentValueInjecter = new Lazy<IFluentValueInjecter<DummyBlogPostDto>>(() => _ValueInjecterManager.Inject(BlogPostDto));
            };
    }
}