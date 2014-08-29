namespace NContext.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NContext.Common;

    public static class IServiceResponseEnumerableDataExtensions
    {
        /// <summary>
        /// Projects each data element for each <paramref name="serviceResponses"/> into a new <see cref="ServiceResponse{IEnumerable{T}}"/>.  
        /// If any <see cref="IServiceResponse{T}"/> has an error, then: 
        /// if <paramref name="aggregateErrors"/> is true, it will loop through all <paramref name="serviceResponses"/> 
        /// and return a <see cref="ServiceResponse{T}"/> with <see cref="AggregateError"/>, else,
        /// it will break out and return a <see cref="ServiceResponse{T}"/> with the first error encountered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponses">The response transfer objects.</param>
        /// <param name="aggregateErrors">The aggregate errors.</param>
        /// <returns>IServiceResponse&lt;IEnumerable&lt;T&gt;&gt;.</returns>
        public static IServiceResponse<IEnumerable<T>> SelectToServiceResponse<T>(
            this IEnumerable<IServiceResponse<T>> serviceResponses, 
            Boolean aggregateErrors = false)
        {
            var errors = new List<Error>();
            var data = new List<T>();
            foreach (var serviceResponse in serviceResponses.TakeWhile(responseTransferObject => responseTransferObject != null))
            {
                if (serviceResponse.Error != null)
                {
                    if (!aggregateErrors)
                    {
                        return new ErrorResponse<IEnumerable<T>>(serviceResponse.Error);
                    }

                    errors.Add(serviceResponse.Error);
                    break;
                }

                data.Add(serviceResponse.Data);
            }

            if (errors.Any())
            {
                return new ErrorResponse<IEnumerable<T>>(
                    new AggregateError(errors[0].HttpStatusCode, errors[0].Code, errors));
            }

            return new DataResponse<IEnumerable<T>>(data);
        }

        /// <summary>
        /// Projects each element of <see cref="IServiceResponse{T}.Data"/> for each <paramref name="serviceResponses"/> into an <see cref="IEnumerable{T}"/> and flattens the resulting sequences into one sequence into a new <see cref="ServiceResponse{IEnumerable{T}}" />.  
        /// If any <see cref="IServiceResponse{T}"/> has an error, then: 
        /// if <paramref name="aggregateErrors"/> is true, it will loop through all <paramref name="serviceResponses"/> 
        /// and return a <see cref="ServiceResponse{T}"/> with <see cref="AggregateError"/>, else,
        /// it will break out and return a <see cref="ServiceResponse{T}"/> with the first error encountered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResponses">The service responses.</param>
        /// <param name="aggregateErrors">The aggregate errors.</param>
        /// <returns>IServiceResponse{IEnumerable{T}}.</returns>
        public static IServiceResponse<IEnumerable<T>> SelectManyToServiceResponse<T>(
            this IEnumerable<IServiceResponse<IEnumerable<T>>> serviceResponses, 
            Boolean aggregateErrors = false)
        {
            var errors = new List<Error>();
            var data = new List<T>();
            foreach (var serviceResponse in serviceResponses.TakeWhile(responseTransferObject => responseTransferObject != null))
            {
                if (serviceResponse.Error != null)
                {
                    if (!aggregateErrors)
                    {
                        return new ErrorResponse<IEnumerable<T>>(serviceResponse.Error);
                    }

                    errors.Add(serviceResponse.Error);
                    break;
                }

                data.AddRange(serviceResponse.Data);
            }

            if (errors.Any())
            {
                return new ErrorResponse<IEnumerable<T>>(
                    new AggregateError(errors[0].HttpStatusCode, errors[0].Code, errors));
            }

            return new DataResponse<IEnumerable<T>>(data);
        }
    }
}