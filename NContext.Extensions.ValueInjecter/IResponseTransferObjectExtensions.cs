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
    using System.Linq;

    using NContext.Common;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        /// <summary>
        /// Translates this instance to an <see cref="IResponseTransferObject{TTarget}"/> using <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="TTarget">The type of the dto.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>Instance of <see cref="ServiceResponse{TDto}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObject<TTarget> Translate<TTarget>(this IResponseTransferObject<Object> response)
            where TTarget : class
        {
            return response.Translate<Object, TTarget, LoopValueInjection>();
        }

        /// <summary>
        /// Translates this instance to an <see cref="IResponseTransferObject{TTarget}"/>
        /// using the specified <typeparamref name="TValueInjection"/>.
        /// </summary>
        /// <typeparam name="TTarget">The type of the dto.</typeparam>
        /// <typeparam name="TValueInjection">The type of the value injection.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>Instance of <see cref="ServiceResponse{TDto}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObject<TTarget> Translate<TTarget, TValueInjection>(this IResponseTransferObject<Object> response)
            where TTarget : class
            where TValueInjection : IValueInjection, new()
        {
            return response.Translate<Object, TTarget, TValueInjection>();
        }

        /// <summary>
        /// Translates this instance to an <see cref="IResponseTransferObject{TTarget}"/> using <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget">The type of the dto.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>Instance of <see cref="ServiceResponse{TDto}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObject<TTarget> Translate<TSource, TTarget>(this IResponseTransferObject<TSource> response)
            where TTarget : class
        {
            return response.Translate<TSource, TTarget, LoopValueInjection>();
        }

        /// <summary>
        /// Translates this instance to an <see cref="IResponseTransferObject{TTarget}"/>
        /// using the specified <typeparamref name="TValueInjection"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget">The type of the dto.</typeparam>
        /// <typeparam name="TValueInjection">The type of the value injection.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>Instance of <see cref="ServiceResponse{TDto}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObject<TTarget> Translate<TSource, TTarget, TValueInjection>(this IResponseTransferObject<TSource> response)
            where TTarget : class
            where TValueInjection : IValueInjection, new()
        {
            if (response.Errors.Any())
            {
                return new ServiceResponse<TTarget>(response.Errors);
            }

            return new ServiceResponse<TTarget>(Activator.CreateInstance<TTarget>().InjectFrom<TTarget, TValueInjection>(response.Data));
        }
    }
}