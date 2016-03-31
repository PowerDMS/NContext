namespace NContext.Extensions.AutoMapper.Extensions
{
    using System;
    using System.Collections.Generic;

    using global::AutoMapper;

    public static class ObjectExtensions
    {
        /// <summary>
        /// Maps the specified instance to the target.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="mappingOperationOptions">The mapping operation options.</param>
        /// <returns><typeparamref name="TTarget"/>.</returns>
        public static TTarget Map<TTarget>(this Object instance, Action<IMappingOperationOptions> mappingOperationOptions = null)
        {
            return Mapper.Map<TTarget>(instance, mappingOperationOptions ?? (o => { }));
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
            return Mapper.Map<IEnumerable<TTarget>>(entities, mappingOperationOptions ?? (o => { }));
        }
    }
}