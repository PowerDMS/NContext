// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfGenericRepository.cs" company="Waking Venture, Inc.">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;

    using NContext.Data.Persistence;
    using NContext.Data.Specifications;

    /// <summary>
    /// Defines a generic implementation of the repository pattern for database communication.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EfGenericRepository<TEntity> : IEfGenericRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfGenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks></remarks>
        protected internal EfGenericRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _Context = context;
        }

        #region Implementation of IEfGenericRepository<TEntity>

        public event EventHandler Disposing;

        public event EventHandler Disposed;

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to the unit of work
        /// to be persisted and inserted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// inserted into the database.</param>
        /// <remarks>Implementors of this method must handle the PersistAdd scenario.</remarks>
        public void Add(TEntity entity)
        {
            _Context.Set<TEntity>().Add(entity);
        }

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
        public IQueryable<TEntity> AllMatching(SpecificationBase<TEntity> specification)
        {
            return _Context.Set<TEntity>().Where(specification.IsSatisfiedBy());
        }

        /// <summary>
        /// Attaches the specified entity to the context underlying the set. That is, the entity is placed into the context in the
        /// Unchanged state, just as if it had been read from the database.
        /// </summary>
        /// <param name="entity">The entity instance to attach back to the repository.</param>
        public void Attach(TEntity entity)
        {
            _Context.Set<TEntity>().Attach(entity);
        }

        /// <summary>
        /// Detaches the specified entity from the underlying <see cref="System.Data.Objects.ObjectContext"/>.
        /// </summary>
        /// <param name="entity">The entity to detach.</param>
        public void Detach(TEntity entity)
        {
            ((IObjectContextAdapter)_Context).ObjectContext.Detach(entity);
        }

        /// <summary>
        /// Finds an entity with the given primary key values. If an entity with the given primary key values exists in the context, 
        /// then it is returned immediately without making a request to the store. Otherwise, a request is made to the store for an 
        /// entity with the given primary key values for this entity, if found, is attached to the context and returned. If no entity 
        /// is found in the context, then null is returned.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns><typeparamref name="TEntity"/> if found in the context or data store; otherwise, null.</returns>
        public TEntity Find(params Object[] keyValues)
        {
            return _Context.Set<TEntity>().Find(keyValues);
        }

        /// <summary>
        /// Gets the results for the specified paged query.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns>IQueryable{TEntity}.</returns>
        public IQueryable<TEntity> GetPaged<TProperty>(Int32 pageIndex, Int32 pageCount, Expression<Func<TEntity, TProperty>> orderByExpression, Boolean ascending = true)
        {
            var set = _Context.Set<TEntity>();
            if (@ascending)
            {
                return set.OrderBy(orderByExpression)
                    .Skip(pageCount * pageIndex)
                    .Take(pageCount);
            }

            return set.OrderByDescending(orderByExpression)
                .Skip(pageCount * pageIndex)
                .Take(pageCount);
        }

        /// <summary>
        /// Includes the specified path for eager loading.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="path">The path.</param>
        /// <returns>IQueryable{`0}.</returns>
        public IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> path)
        {
            return _Context.Set<TEntity>().Include(path);
        }

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        public void Refresh(TEntity entity)
        {
            _Context.Entry<TEntity>(entity).Reload();
        }

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to the unit of work
        /// to be persisted and deleted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// deleted from the database.</param>
        /// <remarks>Implementors of this method must handle the PersistDelete scneario.</remarks>
        public void Remove(TEntity entity)
        {
            _Context.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Queries the <see cref="DbContext" /> via SQL, using the specified parameters.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IQueryable{TEntity}.</returns>
        public IQueryable<TEntity> SqlQuery(String sql, params Object[] parameters)
        {
            return _Context.Set<TEntity>().SqlQuery(sql, parameters).AsQueryable();
        }

        /// <summary>
        /// Executes the command and returns the number of rows affected.
        /// </summary>
        /// <param name="sqlCommand">The SQL command.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>Number of affected rows.</returns>
        /// <remarks></remarks>
        public Int32 SqlCommand(String sqlCommand, params Object[] parameters)
        {
            return _Context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DbEntityValidationResult.</returns>
        public DbEntityValidationResult Validate(TEntity entity)
        {
            return _Context.Entry<TEntity>(entity).GetValidationResult();
        }

        /// <summary>
        /// Gets the <see cref="IQueryable{T}"/> used by <see cref="IEfGenericRepository{TEntity}"/> 
        /// to execute Linq queries.
        /// </summary>
        /// <value>A <see cref="IQueryable{TEntity}"/> instance.</value>
        /// <remarks>
        /// Inheritors of this base class should return a valid non-null <see cref="IQueryable{TEntity}"/> instance.
        /// </remarks>
        protected virtual IQueryable<TEntity> RepositoryQuery
        {
            get
            {
                return _Context.Set<TEntity>();
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

        #region Implementation of IDisposable

        /// <summary>
        /// Finalizes an instance of the <see cref="EfGenericRepository{TEntity}" /> class.
        /// </summary>
        ~EfGenericRepository()
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
                // TODO: (DG) If not part of a unit of work, dispose the _Context? Can the rep exist outside a UoW? Not currently.
                var disposingHandler = Disposing;
                if (disposingHandler != null)
                {
                    disposingHandler(this, EventArgs.Empty);
                }
            }

            IsDisposed = true;
            var disposedHandler = Disposed;
            if (disposedHandler != null)
            {
                disposedHandler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}