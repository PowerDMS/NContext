// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Waking Venture, Inc.">
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
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines a static class for providing Type extension methods.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Evaluates whether the current type implements the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The base type or interface type to check against.</typeparam>
        /// <param name="type">The derived type.</param>
        /// <returns><c>True</c> if <paramref name="type"/> implements or inherits from type <typeparamref name="T"/>, else <c>false</c>.</returns>
        public static Boolean Implements<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        /// <summary>
        /// Evaluates whether the current type implements the type specified.
        /// </summary>
        /// <param name="type">The derived type.</param>
        /// <param name="typeToCheck">The type to check.</param>
        /// <returns><c>True</c> if <paramref name="type"/> implements or inherits from type <paramref name="typeToCheck"/>, else <c>false</c>.</returns>
        /// <remarks></remarks>
        public static Boolean Implements(this Type type, Type typeToCheck)
        {
            return typeToCheck.IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether the specified type is anonymous.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type [is anonymous]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static Boolean IsAnonymousType(this Type type)
        {
            return 
                type.IsGenericType && 
                (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic && 
                (type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) || 
                 type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase)) && 
                type.Name.Contains("AnonymousType") && 
                Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false);
        }
    }
}