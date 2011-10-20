// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs">
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
//
// <summary>
//   Defines a static class for providing IEnumerable type extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Omu.ValueInjecter;

namespace NContext.Application.Extensions
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
        public static IEnumerable<T> Into<T>(this IEnumerable<Object> projections)
            where T : class, new()
        {
            return projections.Into<T, LoopValueInjection>();
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
        public static IEnumerable<T> Into<T, TValueInjection>(this IEnumerable<Object> projections) 
            where T : class, new()
            where TValueInjection : IValueInjection, new()
        {
            return projections.ToList().Select(o => Activator.CreateInstance<T>().InjectFrom<TValueInjection>(o)).Cast<T>();
        }
    }
}