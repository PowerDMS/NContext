// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueInjecterExtensions.cs">
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
//   Defines extension methods for Value Injecter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Omu.ValueInjecter;

namespace NContext.Application.Extensions
{
    /// <summary>
    /// Defines extension methods for Value Injecter.
    /// </summary>
    public static class ValueInjecterExtensions
    {
        /// <summary>
        /// Injects values from source into target using the default <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="source">The source object.</param>
        /// <returns>Instance of <typeparamref name="T"/> with the injected values.</returns>
        /// <remarks></remarks>
        public static T InjectInto<T>(this T target, Object source)
            where T : class
        {
            return target.InjectInto<T, LoopValueInjection>(source);
        }

        /// <summary>
        /// Injects values from source into target using the default <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="source">The source object.</param>
        /// <returns>Instance of <typeparamref name="T"/> with the injected values.</returns>
        /// <remarks></remarks>
        public static T InjectInto<T>(this Object target, Object source)
            where T : class
        {
            return target.InjectInto<T, LoopValueInjection>(source);
        }

        /// <summary>
        /// Injects values from source into target using the specified <see cref="ValueInjection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValueInjection">The type of the value injection.</typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="source">The source object.</param>
        /// <returns>Instance of <typeparamref name="T"/> with the injected values.</returns>
        /// <remarks></remarks>
        public static T InjectInto<T, TValueInjection>(this Object target, Object source)
            where T : class
            where TValueInjection : IValueInjection, new()
        {
            return target.InjectFrom<TValueInjection>(source) as T;
        }
    }
}