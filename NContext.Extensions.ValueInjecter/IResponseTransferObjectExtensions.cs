// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensions.cs">
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
//   Defines extension methods for IResponseTransferObject{T}.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using NContext.Dto;

using Omu.ValueInjecter;

namespace NContext.Extensions.ValueInjecter
{
    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        /// <summary>
        /// Translates the source <see cref="IResponseTransferObject{TEntity}"/> to a <see cref="IResponseTransferObject{TDto}"/> using <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{TDto}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObject<TDto> InjectTo<TDto>(this IEnumerable<Object> source)
           where TDto : class, new()
        {
            return InjectTo<TDto, LoopValueInjection>(source);
        }

        /// <summary>
        /// Translates the source <see cref="IResponseTransferObject{TEntity}"/> to a <see cref="IResponseTransferObject{TDto}"/> using the specified <see cref="IValueInjection"/>.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <typeparam name="TValueInjection">The type of the value injection.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{TDto}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObject<TDto> InjectTo<TDto, TValueInjection>(this IEnumerable<Object> source)
            where TDto : class, new()
            where TValueInjection : IValueInjection, new()
        {
            return (IResponseTransferObject<TDto>)
                Activator.CreateInstance(typeof(ServiceResponse<TDto>),
                                         source.ToMaybe()
                                               .Select(objects =>
                                                       objects.Select(obj =>
                                                                      obj.ToMaybe()
                                                                         .Bind(objInstance =>
                                                                               Activator.CreateInstance(typeof(TDto))
                                                                                        .InjectFrom<TDto, TValueInjection>(objInstance)
                                                                                        .ToMaybe())
                                                                         .FromMaybe(default(TDto))).ToMaybe())
                                               .FromMaybe(Enumerable.Empty<TDto>()));
        }
    }
}