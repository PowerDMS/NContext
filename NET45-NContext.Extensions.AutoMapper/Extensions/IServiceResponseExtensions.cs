namespace NContext.Extensions.AutoMapper.Extensions
{
    using System;

    using global::AutoMapper;

    using NContext.Common;

    public static class IServiceResponseExtensions
    {
        /// <summary>
        /// Translates the <see cref="IServiceResponse{TSource}"/> instance into an <see cref="IServiceResponse{TTarget}"/> using AutoMapper.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="mappingOperationOptions">Options for a single map operation.</param>
        /// <returns>Instance of <see cref="IServiceResponse{TTarget}"/>.</returns>
        [Obsolete]
        public static IServiceResponse<TTarget> BindMap<TSource, TTarget>(
            this IServiceResponse<TSource> source,
            Action<IMappingOperationOptions> mappingOperationOptions = null)
        {
            if (source.IsLeft)
            {
                return new ErrorResponse<TTarget>(source.Error);
            }

            return new DataResponse<TTarget>(Mapper.Instance.Map<TSource, TTarget>(source.Data, mappingOperationOptions ?? (o => { })));
        }

        /// <summary>
        /// Translates the <see cref="IServiceResponse{TSource}"/> instance into an <see cref="IServiceResponse{TTarget}"/> using AutoMapper.
        /// </summary>
        /// <typeparam name="TSource">The type of source object.</typeparam>
        /// <typeparam name="TTarget">The type of target object.</typeparam>
        /// <param name="source">Source instance to translate.</param>
        /// <param name="mapper"><see cref="IMapper"/> instance.</param>
        /// <param name="mappingOperationOptions">Options for a single map operation.</param>
        /// <returns>Instance of <see cref="IServiceResponse{TTarget}"/>.</returns>
        public static IServiceResponse<TTarget> BindMap<TSource, TTarget>(
            this IServiceResponse<TSource> source, 
            IMapper mapper,
            Action<IMappingOperationOptions> mappingOperationOptions = null)
        {
            if (source.IsLeft)
            {
                return new ErrorResponse<TTarget>(source.Error);
            }

            return new DataResponse<TTarget>(mapper.Map<TSource, TTarget>(source.Data, mappingOperationOptions ?? (o => { })));
        }
    }
}