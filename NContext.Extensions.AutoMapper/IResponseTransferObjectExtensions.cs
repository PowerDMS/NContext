// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensions.cs">
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
//
// <summary>
//   Defines extension methods for IResponseTransferObject<T>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using AutoMapper;

using Microsoft.Practices.ServiceLocation;

using NContext.Dto;

namespace NContext.Extensions.AutoMapper
{
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
                            .GetInstance<IManageAutoMapper>()
                            .ToMaybe()
                            .Bind(manager => manager.MappingEngine.ToMaybe())
                            .FromMaybe(Mapper.Engine);
                    });
        }

        public static IResponseTransferObject<T2> Map<T, T2>(this IResponseTransferObject<T> responseTransferObject)
        {
            return new ServiceResponse<T2>(GetMapper().Map<IEnumerable<T>, IEnumerable<T2>>(responseTransferObject.Data));
        }

        public static IResponseTransferObject<T2> Map<T, T2>(this IResponseTransferObject<T> responseTransferObject, Action<IMappingOperationOptions> mappingOperationOptions)
        {
            return new ServiceResponse<T2>(GetMapper().Map<IEnumerable<T>, IEnumerable<T2>>(responseTransferObject.Data, mappingOperationOptions));
        }

        private static IMappingEngine GetMapper()
        {
            return _MappingEngine.Value;
        }
    }
}