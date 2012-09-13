// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEfGenericRepository.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;

    using NContext.Data.Persistence;
    using NContext.Data.Specifications;

    /// <summary>
    /// Defines a generic repository base abstraction.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <remarks></remarks>
    public interface IEfGenericRepository<TEntity> : IDisposable, IQueryable<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Adds an instance of <typeparamref name="TEntity"/> to the unit of work
        /// to be persisted and inserted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// inserted into the database.</param>
        /// <remarks>Implementors of this method must handle the PersistAdd scenario.</remarks>
        void Add(TEntity entity);

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
        /// Attaches a detached entity.
        /// </summary>
        /// <param name="entity">The entity instance to attach to the repository.</param>
        void Attach(TEntity entity);

        /// <summary>
        /// Get all elements of type <typeparamref name="TEntity"/>. 
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements.</returns>
        /// <remarks></remarks>
        IQueryable<TEntity> GetPaged<TProperty>(Int32 pageIndex, Int32 pageCount, Expression<Func<TEntity, TProperty>> orderByExpression, Boolean ascending = true);

        /// <summary>
        /// Includes the specified path for eager loading.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="path">The path.</param>
        /// <returns>IQueryable{TEntity}.</returns>
        IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> path);

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        void Refresh(TEntity entity);

        /// <summary>
        /// Removes an instance of <typeparamref name="TEntity"/> to the unit of work
        /// to be persisted and deleted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// deleted from the database.</param>
        /// <remarks>Implementors of this method must handle the PersistDelete scneario.</remarks>
        void Remove(TEntity entity);

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
        /// Executes the command and returns the number of rows affected.
        /// </summary>
        /// <param name="sqlCommand">The SQL command.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>Number of affected rows.</returns>
        /// <remarks></remarks>
        Int32 SqlCommand(String sqlCommand, params Object[] parameters);

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DbEntityValidationResult.</returns>
        DbEntityValidationResult Validate(TEntity entity);
    }
}