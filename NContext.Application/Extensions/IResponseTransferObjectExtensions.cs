// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensions.cs">
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
//   Defines extension methods for IResponseTransferObject{T}.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using NContext.Application.Dto;

using Omu.ValueInjecter;

namespace NContext.Application.Extensions
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
        public static IResponseTransferObject<TDto> Translate<TDto>(this IEnumerable<Object> source)
           where TDto : class, new()
        {
            return Translate<TDto, LoopValueInjection>(source);
        }

        /// <summary>
        /// Translates the source <see cref="IResponseTransferObject{TEntity}"/> to a <see cref="IResponseTransferObject{TDto}"/> using the specified <see cref="IValueInjection"/>.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <typeparam name="TValueInjection">The type of the value injection.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{TDto}"/>.</returns>
        /// <remarks></remarks>
        public static IResponseTransferObject<TDto> Translate<TDto, TValueInjection>(this IEnumerable<Object> source)
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
                                                                                        .InjectInto<TDto, TValueInjection>(objInstance)
                                                                                        .ToMaybe())
                                                                         .FromMaybe(default(TDto))).ToMaybe())
                                               .FromMaybe(Enumerable.Empty<TDto>()));
        }
    }
}