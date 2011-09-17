// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaybeExtensions.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>

// <summary>
//   Defines extension methods for IMaybe<T>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace NContext.Application.Extensions
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
        /// <typeparam name="TU">The type of the U.</typeparam>
        /// <param name="maybe">The maybe.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IMaybe<TU> Select<T, TU>(this IMaybe<T> maybe, Func<T, IMaybe<TU>> func)
        {
            return maybe.Bind(func);
        }

        /// <summary>
        /// Selects the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU">The type of the U.</typeparam>
        /// <param name="maybe">The maybe.</param>
        /// <param name="func">The func.</param>
        /// <param name="select">The select.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IMaybe<TU> SelectMany<T, TU>(this IMaybe<T> maybe, Func<T, IMaybe<TU>> func, Func<T, TU, IMaybe<TU>> @select)
        {
            return maybe.Bind(x => func.Invoke(x).Bind(y => @select(x, y)));
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