// --------------------------------------------------------------------------------------------------------------------
// <copyright file="with_a_custom_mapper.cs" company="Waking Venture, Inc.">
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

    public class with_a_custom_mapper : when_injecting_with_fluent_value_injecter
    {
        static Guid _BlogId;

        static DummyBlogPost _BlogPost;

        Establish context = () =>
            {
                _BlogId = Guid.NewGuid();
            };

        Because of = () => _BlogPost = FluentValueInjecter.Into<DummyBlogPost>(
            new
                {
                    BlogId = _BlogId
                });

        It should_have_the_mapped_value = () => _BlogPost.BlogId.ShouldEqual(_BlogId);
    }
}