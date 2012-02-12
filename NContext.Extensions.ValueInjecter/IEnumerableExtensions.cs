// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs">
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
//   Defines a static class for providing IEnumerable type extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Omu.ValueInjecter;

namespace NContext.Extensions.ValueInjecter
{
    /// <summary>
    /// Defines a static class for providing IEnumerable type extension methods.
    /// </summary>
    /// <remarks></remarks>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Projects an <see cref="IEnumerable&lt;Object&gt;"/> into an enumerable of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to inject the projection into.</typeparam>
        /// <param name="projections">The projections.</param>
        /// <returns><see cref="IEnumerable&lt;T&gt;"/> instance.</returns>
        /// <remarks>
        /// This extension method is useful when executing query projections of anonymous types into custom DTOs.
        /// </remarks>
        public static IEnumerable<T> InjectInto<T>(this IEnumerable<Object> projections)
            where T : class, new()
        {
            return projections.InjectInto<T, LoopValueInjection>();
        }

        /// <summary>
        /// Projects an <see cref="IEnumerable{Object}"/> into an enumerable of the specified 
        /// type <typeparamref name="T"/> using the specified <see cref="IValueInjection"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to inject the projection into.</typeparam>
        /// <typeparam name="TValueInjection">The type of the injection implementation to use.</typeparam>
        /// <param name="projections">The projections.</param>
        /// <returns><see cref="IEnumerable&lt;T&gt;"/> instance.</returns>
        /// <remarks>This extension method is useful when executing query projections of anonymous types into custom DTOs.</remarks>
        public static IEnumerable<T> InjectInto<T, TValueInjection>(this IEnumerable<Object> projections) 
            where T : class, new()
            where TValueInjection : IValueInjection, new()
        {
            return projections.ToList().Select(o => StaticValueInjecter.InjectFrom<TValueInjection>(Activator.CreateInstance<T>(), o)).Cast<T>();
        }
    }
}