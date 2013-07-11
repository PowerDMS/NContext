// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensions.cs" company="Waking Venture, Inc.">
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
    using System.Diagnostics.Contracts;
    using System.Linq;

    using NContext.Common;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        /// <summary>
        /// Translates the <see cref="IResponseTransferObject{TSource}"/> instance into an <see cref="IResponseTransferObject{TTarget}"/>
        /// using <see name="LoopValueInjection"/> and the optionally-specified <paramref name="mapper"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="mapper">A custom anonymous object mapper used for post processing after value injection has taken place.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{TTarget}"/>.</returns>
        public static IResponseTransferObject<TTarget> Translate<TSource, TTarget>(this IResponseTransferObject<TSource> source, Object mapper = null)
            where TTarget : class, new()
        {
            return source.Translate<TSource, TTarget, LoopValueInjection>(mapper);
        }

        /// <summary>
        /// Translates the <see cref="IResponseTransferObject{TSource}"/> instance into an <see cref="IResponseTransferObject{TTarget}"/>
        /// using the specified <typeparamref name="TValueInjection"/> and custom <paramref name="mapper"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <typeparam name="TValueInjection">The type of <see cref="IValueInjection"/> to use.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="mapper">A custom anonymous object mapper used for post processing after value injection has taken place.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{TTarget}"/>.</returns>
        public static IResponseTransferObject<TTarget> Translate<TSource, TTarget, TValueInjection>(this IResponseTransferObject<TSource> source, Object mapper = null)
            where TTarget : class, new()
            where TValueInjection : IValueInjection, new()
        {
            return source.Translate(Activator.CreateInstance<TTarget>(), Activator.CreateInstance<TValueInjection>(), mapper);
        }

        /// <summary>
        /// Translates the <see cref="IResponseTransferObject{TSource}"/> instance into an <see cref="IResponseTransferObject{TTarget}"/>
        /// using the specified <typeparamref name="TValueInjection"/> and custom <paramref name="mapper"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <typeparam name="TValueInjection">The type of <see cref="IValueInjection"/> to use.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="target">Target instance to inject into.</param>
        /// <param name="valueInjection"><see cref="IValueInjection"/> instance to use. Default is <see cref="LoopValueInjection"/>.</param>
        /// <param name="mapper">A custom anonymous object mapper used for post processing after value injection has taken place.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{TTarget}"/>.</returns>
        public static IResponseTransferObject<TTarget> Translate<TSource, TTarget, TValueInjection>(this IResponseTransferObject<TSource> source, TTarget target, TValueInjection valueInjection, Object mapper)
            where TValueInjection : IValueInjection, new()
        {
            Contract.Requires(source != null);

            if (source.Errors.Any())
            {
                return new ServiceResponse<TTarget>(source.Errors);
            }

            return new ServiceResponse<TTarget>(source.Data.Inject().Using(valueInjection).Into(target, mapper));
        }
    }
}