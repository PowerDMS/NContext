namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using NContext.Common;

    /// <summary>
    /// Defines extension methods for <see cref="IQueryable{T}"/> yielding a new <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseIQueryableExtensions
    {
        /// <summary>
        /// Returns an <see cref="IServiceResponse{T}"/> with the first element of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/> to return the first element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns>IServiceResponse{T} with the first element in the sequence that passes the test in the (optional) predicate function.</returns>
        public static IServiceResponse<T> FirstResponse<T>(this IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            using (var enumerator = GetEnumerator(queryable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ErrorResponse<T>(
                        new Error(
                            (Int32)HttpStatusCode.InternalServerError,
                            "IResponseTransferObjectIQueryableExtensions_FirstResponse_NoMatch",
                            new[] { "Enumerable is empty." }));
                }

                return new DataResponse<T>(enumerator.Current);
            }
        }

        /// <summary>
        /// Returns an <see cref="IServiceResponse{T}"/> with the single, specific element of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/> to return the single element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns>IServiceResponse{T} with the single element in the sequence that passes the test in the (optional) predicate function.</returns>
        public static IServiceResponse<T> SingleResponse<T>(this IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            using (var enumerator = GetEnumerator(queryable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ErrorResponse<T>(
                        new Error(
                            (Int32)HttpStatusCode.InternalServerError,
                            "IResponseTransferObjectIQueryableExtensions_SingleResponse_NoMatch",
                            new[] { "Enumerable is empty." }));
                }

                T current = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    return new DataResponse<T>(current);
                }
            }
            
            return new ErrorResponse<T>(
                new Error(
                    (Int32)HttpStatusCode.InternalServerError,
                    "IResponseTransferObjectIQueryableExtensions_SingleResponse_MoreThanOneMatch",
                    new[] { "Enumerable has more than one matched entry." }));
        }

        private static IEnumerator<T> GetEnumerator<T>(IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            return (predicate == null) ? queryable.GetEnumerator() : queryable.Where(predicate).GetEnumerator();
        }
    }
}