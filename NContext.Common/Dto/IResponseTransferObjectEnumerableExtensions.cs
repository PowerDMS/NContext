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

namespace NContext.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines extension methods for <see cref="IEnumerable{T}"/> yielding a new <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectEnumerableExtensions
    {
        public static IResponseTransferObject<T2> Fmap<T, T2>(this IResponseTransferObject<T> responseTransferObject, Func<T, T2> mappingFunction)
        {
            if (responseTransferObject.Errors.Any())
            {
                try
                {
                    return
                        Activator.CreateInstance(
                            responseTransferObject.GetType()
                                                  .GetGenericTypeDefinition()
                                                  .MakeGenericType(typeof(T2)),
                            responseTransferObject.Errors) as IResponseTransferObject<T2>;
                }
                catch (TargetInvocationException)
                {
                    // No contructor found that supported Errors! Return default.
                    return new ServiceResponse<T2>(responseTransferObject.Errors);
                }
            }

            T2 result = mappingFunction.Invoke(responseTransferObject.Data);
            try
            {
                return Activator.CreateInstance(
                    responseTransferObject.GetType()
                                          .GetGenericTypeDefinition()
                                          .MakeGenericType(typeof(T2)),
                    result) as IResponseTransferObject<T2>;
            }
            catch (TargetInvocationException)
            {
                // No contructor found that supported IEnumerable<T>! Return default.
                return new ServiceResponse<T2>(result);
            }
        }

        public static IResponseTransferObject<T> Single<T>(this IEnumerable<T> enumerable, Func<T, Boolean> predicate = null)
        {
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