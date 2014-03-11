// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AggregateError.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2014 Waking Venture, Inc.
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

namespace NContext.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents one or more errors that occur during application execution.
    /// </summary>
    [DataContract]
    public class AggregateError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateError"/> class.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="code">The code.</param>
        /// <param name="errors">The errors.</param>
        public AggregateError(Int32 httpStatusCode, String code, IEnumerable<Error> errors) 
            : base(
                httpStatusCode, 
                code, 
                errors.ToMaybe()
                    .Bind(
                        errorCollection => 
                            errorCollection.SelectMany(e => e.Messages).ToMaybe())
                    .FromMaybe(Enumerable.Empty<String>()))
        {
            Errors = errors;
        }

        [DataMember]
        public IEnumerable<Error> Errors { get; private set; }
    }
}