namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NContext.Common;

    /// <summary>
    /// Defines extension methods for <see cref="IEnumerable{T}"/> yielding a new <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectIEnumerableExtensions
    {
        /// <summary>
        /// Returns an <see cref="IResponseTransferObject{T}"/> with the first element of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns>IResponseTransferObject{T} with the first element in the sequence that passes the test in the (optional) predicate function.</returns>
        public static IResponseTransferObject<T> FirstResponse<T>(this IEnumerable<T> queryable, Func<T, Boolean> predicate = null)
        {
            // TODO: (DG) Re-write this error!
            using (var enumerator = GetEnumerator(queryable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ServiceResponse<T>(new Error("NoMatch", new[] { "No match" }));
                }

                return new ServiceResponse<T>(enumerator.Current);
            }
        }

        /// <summary>
        /// Returns an <see cref="IResponseTransferObject{T}"/> with the single, specific element of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querable">The <see cref="IEnumerable{T}"/> to return the single element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns>IResponseTransferObject{T} with the single element in the sequence that passes the test in the (optional) predicate function.</returns>
        public static IResponseTransferObject<T> SingleResponse<T>(this IEnumerable<T> querable, Func<T, Boolean> predicate = null)
        {
            // TODO: (DG) Re-write these errors!
            using (var enumerator = GetEnumerator(querable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ServiceResponse<T>(new Error("NoMatch", new[] { "No match" }));
                }

                T current = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    return new ServiceResponse<T>(current);
                }
            }

            return new ServiceResponse<T>(new Error("MoreThanOneMatch", new[] { "More than one match!" }));
        }

        private static IEnumerator<T> GetEnumerator<T>(IEnumerable<T> queryable, Func<T, Boolean> predicate = null)
        {
            return (predicate == null) ? queryable.GetEnumerator() : queryable.Where(predicate).GetEnumerator();
        }
    }
}