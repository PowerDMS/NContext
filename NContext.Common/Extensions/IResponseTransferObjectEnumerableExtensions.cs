// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectEnumerableExtensions.cs" company="Waking Venture, Inc.">
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
    using System.Diagnostics.Contracts;
    using System.Linq;

    using NContext.Dto;

    /// <summary>
    /// Defines extension methods for <see cref="IEnumerable{T}"/> yielding a new <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectEnumerableExtensions
    {
        public static IResponseTransferObject<T> First<T>(this IEnumerable<T> enumerable, Func<T, Boolean> predicate = null)
        {
            Contract.Requires(enumerable != null);

            // TODO: (DG) Re-write this error!
            using (var enumerator = GetEnumerator(enumerable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ServiceResponse<T>(new Error("NoMatch", new[] { "No match" }));
                }

                return new ServiceResponse<T>(enumerator.Current);
            }
        }

        public static IResponseTransferObject<T> Single<T>(this IEnumerable<T> enumerable, Func<T, Boolean> predicate = null)
        {
            Contract.Requires(enumerable != null);

            // TODO: (DG) Re-write these errors!
            using (var enumerator = GetEnumerator(enumerable, predicate))
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

        private static IEnumerator<T> GetEnumerator<T>(IEnumerable<T> enumerable, Func<T, Boolean> predicate = null)
        {
            return (predicate == null) ? enumerable.GetEnumerator() : enumerable.Where(predicate).GetEnumerator();
        }
    }
}