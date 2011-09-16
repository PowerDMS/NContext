using System;
using System.Collections.Generic;
using System.Linq;

using NContext.Application.Domain;

using Omu.ValueInjecter;

namespace NContext.Application.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Queries the repository based on the provided specification and returns results that
        /// are only satisfied by the specification.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="specification">A <see cref="SpecificationBase{TEntity}"/> instance used to filter results
        /// that only satisfy the specification.</param>
        /// <returns>
        /// A <see cref="IEnumerable{TEntity}"/> that can be used to enumerate over the results
        /// of the query.
        /// </returns>
        public static IEnumerable<TEntity> AllMatching<TEntity>(this IEnumerable<TEntity> enumerable, SpecificationBase<TEntity> specification)
            where TEntity : class, IEntity
        {
            return enumerable.Where(specification.IsSatisfiedBy).AsEnumerable();
        }

        /// <summary>
        /// Projects an <see cref="IEnumerable&lt;Object&gt;"/> into an enumerable of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to inject the projection into.</typeparam>
        /// <param name="projections">The projections.</param>
        /// <returns><see cref="IEnumerable&lt;T&gt;"/> instance.</returns>
        /// <remarks>
        /// This extension method is useful when executing query projections of anonymous types into custom DTOs.
        /// </remarks>
        public static IEnumerable<T> Into<T>(this IEnumerable<Object> projections)
            where T : class, new()
        {
            return projections.Into<T, LoopValueInjection>();
        }

        /// <summary>
        /// Projects an <see cref="IEnumerable{Object}"/> into an enumerable of the specified 
        /// type <typeparamref name="T"/> using the specified <see cref="IValueInjection"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to inject the projection into.</typeparam>
        /// <typeparam name="TValueInjection">The type of the injection implementation to use.</typeparam>
        /// <param name="projections">The projections.</param>
        /// <returns><see cref="IEnumerable&lt;T&gt;"/> instance.</returns>
        /// <remarks>This extension method is useful when executing query projections of anonymous types into custom DTOs.</remarks>
        public static IEnumerable<T> Into<T, TValueInjection>(this IEnumerable<Object> projections) 
            where T : class, new()
            where TValueInjection : IValueInjection, new()
        {
            return projections.ToList().Select(o => Activator.CreateInstance<T>().InjectFrom<TValueInjection>(o)).Cast<T>();
        }
    }
}