// --------------------------------------------------------------------------------------------------------------------
// <copyright file="when_sanitizing_objects_with_ObjectGraphSanitizer.cs" company="Waking Venture, Inc.">
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

namespace NContext.Tests.Specs.Text
{
    using System;

    using Machine.Specifications;

    using NContext.Text;

    public abstract class when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                _MaxDegreeOfParallelism = 1;
                _Sanitizer = new Lazy<ObjectGraphSanitizer>(() => new ObjectGraphSanitizer(TextSanitizer, MaxDegreeOfParallelism));
            };

        protected static ISanitizeText TextSanitizer { get; set; }

        protected static Int32 MaxDegreeOfParallelism
        {
            get { return _MaxDegreeOfParallelism; }
            set { _MaxDegreeOfParallelism = value; }
        }

        protected static void Sanitize(Object o)
        {
            _Sanitizer.Value.Sanitize(o);
        }

        static Lazy<ObjectGraphSanitizer> _Sanitizer;

        static Int32 _MaxDegreeOfParallelism;
    }
}