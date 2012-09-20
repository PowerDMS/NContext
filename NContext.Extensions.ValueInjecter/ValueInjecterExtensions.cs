// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueInjecterExtensions.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.ValueInjecter
{
    using System;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines extension methods for Value Injecter.
    /// </summary>
    public static class ValueInjecterExtensions
    {
        /// <summary>
        /// Injects values from source into target using the default <see cref="LoopValueInjection" />.
        /// </summary>
        /// <typeparam name="TTarget">The type of the T target.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <returns>TTarget instance.</returns>
        public static TTarget InjectFrom<TTarget>(this TTarget target, Object source)
            where TTarget : class
        {
            return target.InjectFrom<TTarget, LoopValueInjection>(source);
        }

        /// <summary>
        /// Injects values from source into target using the specified <see cref="ValueInjection" />.
        /// </summary>
        /// <typeparam name="TTarget">The type of the T target.</typeparam>
        /// <typeparam name="TValueInjection">The type of the T value injection.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <returns>TTarget instance.</returns>
        public static TTarget InjectFrom<TTarget, TValueInjection>(this TTarget target, Object source)
            where TTarget : class
            where TValueInjection : IValueInjection, new()
        {
            return target.InjectFrom((TValueInjection)Activator.CreateInstance<TValueInjection>(), source) as TTarget;
        }
    }
}