// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs" company="Waking Venture, Inc.">
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

namespace NContext.Common.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines extension methods for IEnumerable{T}.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// ForEach extension that enumerates over all items in an <see cref="IEnumerable{T}"/> and executes 
        /// an action.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="collection">The enumerable instance that this extension operates on.</param>
        /// <param name="action">The action executed for each iten in the enumerable.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.GetEnumerator().ForEach(action);
        }

        /// <summary>
        /// ForEach extension that enumerates over all items in an <see cref="IEnumerator{T}"/> and executes 
        /// an action.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="enumerator">The enumerator instance that this extension operates on.</param>
        /// <param name="action">The action executed for each iten in the enumerable.</param>
        public static void ForEach<T>(this IEnumerator<T> enumerator, Action<T> action)
        {
            using (enumerator)
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
        }

        /// <summary>
        /// ForEach extension that enumerates over a enumerable enumerator and attempts to execute 
        /// the provided action delegate and it the action throws an exception, continues enumerating.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="collection">The IEnumerable instance that ths extension operates on.</param>
        /// <param name="action">The action excecuted for each item in the enumerable.</param>
        public static void TryForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.GetEnumerator().TryForEach(action);
        }

        /// <summary>
        /// ForEach extension that enumerates over an enumerator and attempts to execute the provided
        /// action delegate and if the action throws an exception, continues executing.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="enumerator">The IEnumerator instace</param>
        /// <param name="action">The action executed for each item in the enumerator.</param>
        public static void TryForEach<T>(this IEnumerator<T> enumerator, Action<T> action)
        {
            using (enumerator)
            {
                while (enumerator.MoveNext())
                {
                    try
                    {
                        action(enumerator.Current);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}