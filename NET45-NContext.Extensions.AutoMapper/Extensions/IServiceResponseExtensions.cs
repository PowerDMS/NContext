namespace NContext.Extensions.AutoMapper.Extensions
{
    using System;

    using global::AutoMapper;

    using NContext.Common;

    public static class IServiceResponseExtensions
    {
        /// <summary>
        /// Translates the <see cref="IServiceResponse{T}"/> instance into an <see cref="IServiceResponse{TTarget}"/>
        /// using AutoMapper.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="mappingOperationOptions">Options for a single map operation.</param>
        /// <returns>Instance of <see cref="IServiceResponse{TTarget}"/>.</returns>
        public static IServiceResponse<TTarget> BindMap<TSource, TTarget>(
            this IServiceResponse<TSource> source, 
            Action<IMappingOperationOptions> mappingOperationOptions = null)
        {
            if (source.Error != null)
            {
                return new ErrorResponse<TTarget>(source.Error);
            }

            return new DataResponse<TTarget>(Mapper.Map<TSource, TTarget>(source.Data, mappingOperationOptions ?? (o => { })));
        }
    }
}