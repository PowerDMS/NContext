namespace NContext.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NContext.Common;

    public static class IResponseTransferObjectEnumerableDataExtensions
    {
        /// <summary>
        /// Projects each data element for each <paramref name="responseTransferObjects"/> into a new <see cref="ServiceResponse{IEnumerable{T}}"/>.  
        /// If any <see cref="IResponseTransferObject{T}"/> has an error, then: 
        /// if <paramref name="aggregateErrors"/> is true, it will loop through all <paramref name="responseTransferObjects"/> 
        /// and return a <see cref="ServiceResponse{T}"/> with <see cref="AggregateError"/>, else,
        /// it will break out and return a <see cref="ServiceResponse{T}"/> with the first error encountered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObjects">The response transfer objects.</param>
        /// <param name="aggregateErrors">The aggregate errors.</param>
        /// <returns>IResponseTransferObject&lt;IEnumerable&lt;T&gt;&gt;.</returns>
        public static IResponseTransferObject<IEnumerable<T>> SelectToServiceResponse<T>(this IEnumerable<IResponseTransferObject<T>> responseTransferObjects, Boolean aggregateErrors = false)
        {
            var errors = new List<Error>();
            var data = new List<T>();
            foreach (var responseTransferObject in responseTransferObjects.TakeWhile(responseTransferObject => responseTransferObject != null))
            {
                if (responseTransferObject.Error != null)
                {
                    if (!aggregateErrors)
                    {
                        return new ServiceResponse<IEnumerable<T>>(responseTransferObject.Error);
                    }

                    errors.Add(responseTransferObject.Error);
                    break;
                }

                data.Add(responseTransferObject.Data);
            }

            if (errors.Any())
            {
                return new ServiceResponse<IEnumerable<T>>(
                    new AggregateError(errors[0].HttpStatusCode, errors[0].Code, errors));
            }

            return new ServiceResponse<IEnumerable<T>>(data);
        }

        /// <summary>
        /// Projects each element of <see cref="IResponseTransferObject{T}.Data"/> for each <paramref name="responseTransferObjects"/> into an <see cref="IEnumerable{T}"/> and flattens the resulting sequences into one sequence into a new <see cref="ServiceResponse{IEnumerable{T}}" />.  
        /// If any <see cref="IResponseTransferObject{T}"/> has an error, then: 
        /// if <paramref name="aggregateErrors"/> is true, it will loop through all <paramref name="responseTransferObjects"/> 
        /// and return a <see cref="ServiceResponse{T}"/> with <see cref="AggregateError"/>, else,
        /// it will break out and return a <see cref="ServiceResponse{T}"/> with the first error encountered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTransferObjects">The response transfer objects.</param>
        /// <param name="aggregateErrors">The aggregate errors.</param>
        /// <returns>IResponseTransferObject{IEnumerable{T}}.</returns>
        public static IResponseTransferObject<IEnumerable<T>> SelectManyToServiceResponse<T>(this IEnumerable<IResponseTransferObject<IEnumerable<T>>> responseTransferObjects, Boolean aggregateErrors = false)
        {
            var errors = new List<Error>();
            var data = new List<T>();
            foreach (var responseTransferObject in responseTransferObjects.TakeWhile(responseTransferObject => responseTransferObject != null))
            {
                if (responseTransferObject.Error != null)
                {
                    if (!aggregateErrors)
                    {
                        return new ServiceResponse<IEnumerable<T>>(responseTransferObject.Error);
                    }

                    errors.Add(responseTransferObject.Error);
                    break;
                }

                data.AddRange(responseTransferObject.Data);
            }

            if (errors.Any())
            {
                return new ServiceResponse<IEnumerable<T>>(
                    new AggregateError(errors[0].HttpStatusCode, errors[0].Code, errors));
            }

            return new ServiceResponse<IEnumerable<T>>(data);
        }
    }
}