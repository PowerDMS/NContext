namespace NContext.Extensions.AutoMapper.Extensions
{
    using System;
    using System.Diagnostics.Contracts;

    using global::AutoMapper;

    using NContext.Common;

    public static class IResponseTransferObjectExtensions
    {
        /// <summary>
        /// Translates the <see cref="IResponseTransferObject{T}"/> instance into an <see cref="IResponseTransferObject{TTarget}"/>
        /// using AutoMapper.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="mappingOperationOptions">Options for a single map operation.</param>
        /// <returns>Instance of <see cref="IResponseTransferObject{TTarget}"/>.</returns>
        public static IResponseTransferObject<TTarget> BindMap<TSource, TTarget>(
            this IResponseTransferObject<TSource> source, 
            Action<IMappingOperationOptions> mappingOperationOptions = null)
            where TTarget : class, new()
        {
            Contract.Requires(source != null);

            if (source.Error != null)
            {
                return new ServiceResponse<TTarget>(source.Error);
            }

            return new ServiceResponse<TTarget>(Mapper.Map<TSource, TTarget>(source.Data, mappingOperationOptions ?? (o => { })));
        }
    }
}