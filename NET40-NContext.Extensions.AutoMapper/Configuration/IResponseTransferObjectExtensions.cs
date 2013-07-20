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

namespace NContext.Extensions.AutoMapper.Configuration
{
    using System;
    using System.Linq;

    using NContext.Common;

    using Microsoft.Practices.ServiceLocation;

    using global::AutoMapper;

    /// <summary>
    /// Defines extension methods for <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectExtensions
    {
        private static readonly Lazy<IMappingEngine> _MappingEngine;

        static IResponseTransferObjectExtensions()
        {
            _MappingEngine = new Lazy<IMappingEngine>(
                () =>
                {
                    return ServiceLocator.Current
                        .GetInstance<IMappingEngine>()
                        .ToMaybe()
                        .Bind(mappingEngine => mappingEngine.ToMaybe())
                        .FromMaybe(Mapper.Engine);
                });
        }

        /// <summary>
        /// Maps the <paramref name="responseTransferObject" /> to a new instance of <see cref="IResponseTransferObject{T2}" />
        /// using AutoMapper. If errors exist, then this will act similarly to Bind{T2}()
        /// and return a new <see cref="ServiceResponse{T2}" /> with errors.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of data to map to.</typeparam>
        /// <param name="responseTransferObject">The current response transfer object instance.</param>
        /// <param name="mappingOperationOptions">The mapping operation options.</param>
        /// <returns>Maps the <paramref name="responseTransferObject" /> to a new instance of <see cref="IResponseTransferObject{T2}" />
        /// using AutoMapper. If errors exist, then this will act similarly to Bind{T2}()
        /// and return a new <see cref="ServiceResponse{T2}" /> with errors.</returns>
        public static IResponseTransferObject<TTarget> Map<TSource, TTarget>(this IResponseTransferObject<TSource> responseTransferObject, Action<IMappingOperationOptions> mappingOperationOptions = null)
        {
            if (responseTransferObject.Errors.Any())
            {
                return new ServiceResponse<TTarget>(responseTransferObject.Errors);
            }

            return new ServiceResponse<TTarget>(GetMapper().Map<TTarget>(responseTransferObject, mappingOperationOptions ?? (o => { })));
        }

        private static IMappingEngine GetMapper()
        {
            return _MappingEngine.Value;
        }
    }
}