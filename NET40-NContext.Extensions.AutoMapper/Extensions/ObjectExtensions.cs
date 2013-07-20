// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AutoMapper.Extensions
{
    using System;
    using System.Collections.Generic;

    using NContext.Common;

    using Microsoft.Practices.ServiceLocation;

    using global::AutoMapper;

    public static class ObjectExtensions
    {
        private static readonly Lazy<IMappingEngine> _MappingEngine;

        static ObjectExtensions()
        {
            // TODO: (DG) Service Locator as a provider should be abstracted! Not nice.
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
        /// Maps the specified instance to the target.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="mappingOperationOptions">The mapping operation options.</param>
        /// <returns><typeparamref name="TTarget"/>.</returns>
        public static TTarget Map<TTarget>(this Object instance, Action<IMappingOperationOptions> mappingOperationOptions = null)
        {
            return GetMapper().Map<TTarget>(instance, mappingOperationOptions ?? (o => { }));
        }

        /// <summary>
        /// Maps the specified entities.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="mappingOperationOptions">The mapping operation options.</param>
        /// <returns><see cref="IEnumerable{TTarget}"/></returns>
        public static IEnumerable<TTarget> Map<TTarget>(this IEnumerable<Object> entities, Action<IMappingOperationOptions> mappingOperationOptions = null)
        {
            return GetMapper().Map<IEnumerable<TTarget>>(entities, mappingOperationOptions ?? (o => { }));
        }

        private static IMappingEngine GetMapper()
        {
            return _MappingEngine.Value;
        }
    }
}