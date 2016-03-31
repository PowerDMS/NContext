namespace NContext.Extensions.AutoMapper.Configuration
{
    using System;

    using NContext.Common;

    using global::AutoMapper;

    /// <summary>
    /// Defines extension methods for <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseExtensions
    {
        /// <summary>
        /// Maps the <paramref name="responseTransferObject" /> to a new instance of <see cref="IServiceResponse{T2}" />
        /// using AutoMapper. If errors exist, then this will act similarly to Bind{T2}()
        /// and return a new <see cref="ServiceResponse{T2}" /> with errors.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of data to map to.</typeparam>
        /// <param name="responseTransferObject">The current service response instance.</param>
        /// <param name="mappingOperationOptions">The mapping operation options.</param>
        /// <returns>Maps the <paramref name="responseTransferObject" /> to a new instance of <see cref="IServiceResponse{T2}" />
        /// using AutoMapper. If errors exist, then this will act similarly to Bind{T2}()
        /// and return a new <see cref="ServiceResponse{T2}" /> with errors.</returns>
        public static IServiceResponse<TTarget> Map<TSource, TTarget>(this IServiceResponse<TSource> responseTransferObject, Action<IMappingOperationOptions> mappingOperationOptions = null)
        {
            if (responseTransferObject.Error != null)
            {
                return new ErrorResponse<TTarget>(responseTransferObject.Error);
            }

            return new DataResponse<TTarget>(Mapper.Map<TTarget>(responseTransferObject, mappingOperationOptions ?? (o => { })));
        }
    }
}