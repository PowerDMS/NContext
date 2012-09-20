// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectIQueryableExtensions.cs" company="Waking Venture, Inc.">
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

    using NContext.Common;

    /// <summary>
    /// Defines extension methods for <see cref="IEnumerable{T}"/> yielding a new <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectIQueryableExtensions
    {
        /// <summary>
        /// Returns an <see cref="IResponseTransferObject{T}"/> with the first element of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/> to return the first element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns>IResponseTransferObject{T} with the first element in the sequence that passes the test in the (optional) predicate function.</returns>
        public static IResponseTransferObject<T> FirstResponse<T>(this IQueryable<T> queryable, Func<T, Boolean> predicate = null)
        {
            // TODO: (DG) Re-write this error!
            using (var enumerator = GetEnumerator(queryable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ServiceResponse<T>(new Error("NoMatch", new[] { "No match" }));
                }

                return new ServiceResponse<T>(enumerator.Current);
            }
        }

        /// <summary>
        /// Returns an <see cref="IResponseTransferObject{T}"/> with the single, specific element of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querable">The <see cref="IQueryable{T}"/> to return the single element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns>IResponseTransferObject{T} with the single element in the sequence that passes the test in the (optional) predicate function.</returns>
        public static IResponseTransferObject<T> SingleResponse<T>(this IQueryable<T> querable, Func<T, Boolean> predicate = null)
        {
            // TODO: (DG) Re-write these errors!
            using (var enumerator = GetEnumerator(querable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ServiceResponse<T>(new Error("NoMatch", new[] { "No match" }));
                }

                T current = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    return new ServiceResponse<T>(current);
                }
            }

            return new ServiceResponse<T>(new Error("MoreThanOneMatch", new[] { "More than one match!" }));
        }

        private static IEnumerator<T> GetEnumerator<T>(IQueryable<T> queryable, Func<T, Boolean> predicate = null)
        {
            return (predicate == null) ? queryable.GetEnumerator() : queryable.Where(predicate).GetEnumerator();
        }
    }
}