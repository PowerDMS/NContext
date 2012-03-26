// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueInjecterExtensions.cs">
//   Copyright (c) 2012
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
//   Defines extension methods for Value Injecter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Omu.ValueInjecter;

namespace NContext.Extensions.ValueInjecter
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
        public static T InjectFrom<T>(this T target, Object source)
            where T : class
        {
            return target.InjectFrom<T, LoopValueInjection>(source);
        }

        /// <summary>
        /// Injects values from source into target using the default <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="source">The source object.</param>
        /// <returns>Instance of <typeparamref name="T"/> with the injected values.</returns>
        /// <remarks></remarks>
        public static T InjectFrom<T>(this Object target, Object source)
            where T : class
        {
            return target.InjectFrom<T, LoopValueInjection>(source);
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
        public static T InjectFrom<T, TValueInjection>(this Object target, Object source)
            where T : class
            where TValueInjection : IValueInjection, new()
        {
            return StaticValueInjecter.InjectFrom<TValueInjection>(target, source) as T;
        }
    }
}