// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionExtensions.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2012 Waking Venture, Inc.
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

namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using NContext.Common;

    /// <summary>
    /// Defines extension methods for exception handling.
    /// </summary>
    public static class ExceptionExtensions
    {

        /// <summary>
        /// Returns an error representing the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>Error instance.</returns>
        public static IEnumerable<Error> ToErrors(this Exception exception)
        {
            return exception.FromHierarchy(e => e.InnerException)
                            .Select(ToError);
        }

        /// <summary>
        /// Returns the <see cref="IEnumerable{Exception}" /> as an enumerable of <see cref="Error" />.
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        /// <returns>IEnumerable{Error}.</returns>
        public static IEnumerable<Error> ToErrors(this IEnumerable<Exception> exceptions)
        {
            return exceptions.SelectMany(ToErrors);
        }

        /// <summary>
        /// Returns the <see cref="AggregateException"/> as an enumerable of <see cref="Error"/>.
        /// </summary>
        /// <param name="aggregateException">The aggregate exception.</param>
        /// <returns>IEnumerable{Error}.</returns>
        public static IEnumerable<Error> ToErrors(this AggregateException aggregateException)
        {
            return new List<Error>().AddC(aggregateException.ToError())
                                    .AddRangeC(aggregateException.InnerExceptions.ToErrors());
        }

        private static Error ToError(this Exception exception)
        {
            return new Error(exception.GetType().Name, new[] { exception.Message }, HttpStatusCode.InternalServerError.ToString());
        }
    }
}