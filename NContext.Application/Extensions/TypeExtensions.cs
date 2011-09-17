// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs">
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
//   Defines a static class for providing Type extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NContext.Application.Extensions
{
    /// <summary>
    /// Defines a static class for providing Type extension methods.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Evaluates whether the specified type implements the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The base type or interface type to check against.</typeparam>
        /// <param name="type">The derived type.</param>
        /// <returns><c>True</c> if <paramref name="type"/> implements or inherits from type <typeparamref name="T"/>, else <c>false</c>.</returns>
        public static Boolean Implements<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether the specified type is anonymous.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is anonymous type] [the specified type]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static Boolean IsAnonymousType(this Type type)
        {
            return type.IsGenericType && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic
                && (type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) ||
                    type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase))
                && type.Name.Contains("AnonymousType")
                && Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false);
        }
    }
}