// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Waking Venture, Inc.">
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

    /// <summary>
    /// Defines extension methods for ValueInjecter.
    /// </summary>
    /// <remarks></remarks>
    public static class Extensions
    {
        /// <summary>
        /// Returns a new <see cref="IFluentValueInjector{T}"/> instance using <paramref name="source"/>
        /// as the source object for value injection.
        /// </summary>
        /// <typeparam name="T">Source type.</typeparam>
        /// <param name="source">The source instance.</param>
        /// <returns>IFluentValueInjector{T}.</returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static IFluentValueInjector<T> Inject<T>(this T source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new FluentValueInjector<T>(source);
        }
    }
}