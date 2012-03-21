// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaybeExtensions.cs">
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
//
// <summary>
//   Defines extension methods for IMaybe<T>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NContext.Extensions
{
    /// <summary>
    /// Defines extension methods for <see cref="IMaybe{T}"/>.
    /// </summary>
    public static class IMaybeExtensions
    {
        /// <summary>
        /// Selects the specified maybe.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the U.</typeparam>
        /// <param name="maybe">The maybe.</param>
        /// <param name="selectFunction">The selectManyFunction function.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IMaybe<TResult> Select<T, TResult>(this IMaybe<T> maybe, Func<T, IMaybe<TResult>> selectFunction)
            where T : IEnumerable
            where TResult : IEnumerable
        {
            return maybe.Bind(selectFunction);
        }

        /// <summary>
        /// Wraps the object in a <see cref="IMaybe{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of the object to wrap</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns><see cref="IMaybe{T}"/></returns>
        public static IMaybe<T> ToMaybe<T>(this T obj)
        {
            if (obj == null)
            {
                return new Nothing<T>();
            }

            return new Just<T>(obj);
        }

        /// <summary>
        /// Returns the first element in an <see cref="IEnumerable{T}"/> as a
        /// <see cref="Just{T}"/>, or, if the sequence contains no elements, returns
        /// a <see cref="Nothing{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object in the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="enumerable">The IEnumerable.</param>
        /// <returns><see cref="IMaybe{T}"/></returns>
        public static IMaybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable)
        {
            var value = enumerable.GetEnumerator();
            return value.MoveNext() 
                ? value.Current.ToMaybe() 
                : new Nothing<T>();
        }
    }
}