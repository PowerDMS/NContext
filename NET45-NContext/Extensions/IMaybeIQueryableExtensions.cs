namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NContext.Common;

    /// <summary>
    /// Defines extension methods for <see cref="IMaybe{T}"/>.
    /// </summary>
    public static class IMaybeIQueryableExtensions
    {
        /// <summary>
        /// Returns the first element in an <see cref="IQueryable{T}" /> as a
        /// <see cref="Just{T}" />, or, if the sequence contains no elements, returns
        /// a <see cref="Nothing{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the object in the <see cref="IQueryable{T}" /></typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/> to return the first element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns><see cref="IMaybe{T}" /></returns>
        public static IMaybe<T> MaybeFirst<T>(this IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            using (var enumerator = GetEnumerator(queryable, predicate))
            {
                return enumerator.MoveNext() ? enumerator.Current.ToMaybe() : new Nothing<T>();
            }
        }

        /// <summary>
        /// Returns the first element in an <see cref="IQueryable{T}" /> as a <see cref="Just{T}" />, or, 
        /// if the sequence contains no elements, returns a <see cref="Nothing{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the object in the <see cref="IQueryable{T}" /></typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/> to return the single element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns><see cref="IMaybe{T}" /></returns>
        public static IMaybe<T> MaybeSingle<T>(this IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            using (var enumerator = GetEnumerator(queryable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new Nothing<T>();
                }

                T current = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    return current.ToMaybe();
                }
            }

            return new Nothing<T>();
        }

        private static IEnumerator<T> GetEnumerator<T>(IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            return (predicate == null) ? queryable.GetEnumerator() : queryable.Where(predicate).GetEnumerator();
        }
    }
}