// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEfGenericRepository.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines a generic repository base abstraction.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

using NContext.Application.Domain;
using NContext.Application.Domain.Specifications;

namespace NContext.Persistence.EntityFramework
{
    /// <summary>
    /// Defines a generic repository base abstraction.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <remarks></remarks>
    public interface IEfGenericRepository<TEntity> : IDisposable, IQueryable<TEntity>
        where TEntity : class, IEntity
    {
        #region Methods

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to the unit of work
        /// to be persisted and inserted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// inserted into the database.</param>
        /// <remarks>Implementors of this method must handle the PersistAdd scenario.</remarks>
        void Add(TEntity entity);

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to the unit of work
        /// to be persisted and deleted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// deleted from the database.</param>
        /// <remarks>Implementors of this method must handle the PersistDelete scneario.</remarks>
        void Remove(TEntity entity);

        /// <summary>
        /// Attaches a detached entity.
        /// </summary>
        /// <param name="entity">The entity instance to attach to the repository.</param>
        void Attach(TEntity entity);

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        void Refresh(TEntity entity);

        /// <summary>
        /// Get all elements of type <see cref="TEntity"/> 
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        /// <remarks></remarks>
        IQueryable<TEntity> GetPaged<TProperty>(Int32 pageIndex, Int32 pageCount, Expression<Func<TEntity, TProperty>> orderByExpression, Boolean ascending = true);

        /// <summary>
        /// Queries the context using the database providers native SQL query language and returns
        /// a strongly-typed collection of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="sql">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IQueryable&lt;TEntity&gt; instance.</returns>
        /// <remarks></remarks>
        IQueryable<TEntity> SqlQuery(String sql, params Object[] parameters);

        /// <summary>
        /// Queries the context based on the provided specification and returns results that
        /// are only satisfied by the specification.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="specification">A <see cref="SpecificationBase{TEntity}"/> instance used to filter results
        /// that only satisfy the specification.</param>
        /// <returns>
        /// A <see cref="IQueryable{TEntity}"/> that can be used to enumerate over the results
        /// of the query.
        /// </returns>
        IQueryable<TEntity> AllMatching(SpecificationBase<TEntity> specification);

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        DbEntityValidationResult Validate(TEntity entity);

        #endregion
    }
}