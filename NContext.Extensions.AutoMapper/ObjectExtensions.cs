// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Innovative Data Solutions Inc.">
//   Copyright © Innovative Data Solutions Inc. 2012
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.AutoMapper
{
    using global::AutoMapper;

    using Microsoft.Practices.ServiceLocation;

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public static class ObjectExtensions
    {
        private static readonly Lazy<IMappingEngine> _MappingEngine;

        static ObjectExtensions()
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
        /// Maps the specified instance to the target.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="mappingOperationOptions">The mapping operation options.</param>
        /// <returns></returns>
        /// <remarks></remarks>
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
        /// <returns></returns>
        /// <remarks></remarks>
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