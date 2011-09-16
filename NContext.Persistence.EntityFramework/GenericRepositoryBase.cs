// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericRepositoryBase.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Defines a generic repository base abstraction.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

using NContext.Application.Domain;

namespace NContext.Persistence.EntityFramework
{
    /// <summary>
    /// Defines a generic repository base abstraction.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <remarks></remarks>
    public abstract class GenericRepositoryBase<TEntity> : IDisposable, IQueryable<TEntity>
        where TEntity : class, IEntity
    {
        #region Fields

        private readonly DbContext _Context;

        #endregion

        #region Constructors

        protected GenericRepositoryBase(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _Context = context;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the context associated with the repository instance.
        /// </summary>
        /// <remarks></remarks>
        protected DbContext Context
        {
            get
            {
                return _Context;
            }
        }

        #endregion

        #region Implementation of IQueryable

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="IQueryable" /> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType
        {
            get { return RepositoryQuery.ElementType; }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="IQueryable" />.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression" /> that is associated with this instance of <see cref="IQueryable" />.
        /// </returns>
        public Expression Expression
        {
            get { return RepositoryQuery.Expression; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryProvider" /> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider
        {
            get { return RepositoryQuery.Provider; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryable{T}"/> used by the <see cref="GenericRepositoryBase{TEntity}"/> 
        /// to execute Linq queries.
        /// </summary>
        /// <value>A <see cref="IQueryable{TEntity}"/> instance.</value>
        /// <remarks>
        /// Inheritors of this base class should return a valid non-null <see cref="IQueryable{TEntity}"/> instance.
        /// </remarks>
        protected abstract IQueryable<TEntity> RepositoryQuery { get; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{TEntity}" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<TEntity> GetEnumerator()
        {
            return RepositoryQuery.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return RepositoryQuery.GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to the unit of work
        /// to be persisted and inserted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// inserted into the database.</param>
        /// <remarks>Implementors of this method must handle the PersistAdd scenario.</remarks>
        public abstract void Add(TEntity entity);

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to the unit of work
        /// to be persisted and deleted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// deleted from the database.</param>
        /// <remarks>Implementors of this method must handle the PersistDelete scneario.</remarks>
        public abstract void Remove(TEntity entity);

        /// <summary>
        /// Attaches a detached entity.
        /// </summary>
        /// <param name="entity">The entity instance to attach to the repository.</param>
        public abstract void Attach(TEntity entity);

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        public abstract void Refresh(TEntity entity);

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
        public abstract IQueryable<TEntity> GetPaged<TProperty>(Int32 pageIndex, Int32 pageCount, Expression<Func<TEntity, TProperty>> orderByExpression, Boolean ascending);

        /// <summary>
        /// Queries the context using the database providers native SQL query language and returns
        /// a strongly-typed collection of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="sql">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IQueryable&lt;TEntity&gt; instance.</returns>
        /// <remarks></remarks>
        public abstract IQueryable<TEntity> SqlQuery(String sql, params Object[] parameters);

        /// <summary>
        /// Eagerly fetch associations on the entity.
        /// </summary>
        /// <param name="strategyActions">An <see cref="Action{RepositoryEagerFetchingStrategy}"/> delegate
        /// that specifies the eager fetching paths.</param>
        /// <returns>The <see cref="GenericRepositoryBase&lt;TEntity&gt;"/> instance.</returns>
        public GenericRepositoryBase<TEntity> Include(Action<EagerLoadingStrategy<TEntity>> strategyActions)
        {
            var strategy = new EagerLoadingStrategy<TEntity>();
            strategyActions(strategy);
            ApplyEagerLoadingStrategy(strategy.Paths.ToArray());

            return this;
        }

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DbEntityValidationResult Validate(TEntity entity)
        {
            return _Context.Entry<TEntity>(entity).GetValidationResult();
        }

        /// <summary>
        /// When overriden by inheriting classes, applies the fetching strategies on the repository.
        /// </summary>
        /// <param name="paths">An array of <see cref="Expression"/> containing the paths to
        /// eagerly fetch.</param>
        protected abstract void ApplyEagerLoadingStrategy(Expression[] paths);

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before 
        /// the <see cref="GenericRepositoryBase&lt;TEntity&gt;"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks></remarks>
        ~GenericRepositoryBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <remarks></remarks>
        protected Boolean IsDisposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposeManagedResources)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
            }

            IsDisposed = true;
        }

        #endregion
    }
}