// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfGenericRepository.cs">
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
//   Defines a entity framework implementation of the repository pattern for database communication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

using NContext.Application.Domain;
using NContext.Application.Extensions;

namespace NContext.Persistence.EntityFramework
{
    /// <summary>
    /// Defines a generic implementation of the repository pattern for database communication.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EfGenericRepository<TEntity> : GenericRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        #region Fields

        private readonly List<String> _Includes = new List<String>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EfGenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks></remarks>
        internal EfGenericRepository(DbContext context)
            : base(context)
        {
            if (UnitOfWorkController.AmbientUnitOfWork == null)
            {
                throw new Exception("A repository must be created within the scope of an existing IUnitOfWork instance.");
            }
        }

        #endregion

        #region Overrides of GenericRepositoryBase<TEntity>

        /// <summary>
        /// Gets the <see cref="IQueryable{T}"/> used by the <see cref="GenericRepositoryBase{TEntity}"/> 
        /// to execute Linq queries.
        /// </summary>
        /// <value>A <see cref="IQueryable{TEntity}"/> instance.</value>
        /// <remarks>
        /// Inheritors of this base class should return a valid non-null <see cref="IQueryable{TEntity}"/> instance.
        /// </remarks>
        protected override IQueryable<TEntity> RepositoryQuery
        {
            get
            {
                DbQuery<TEntity> query = Context.Set<TEntity>();
                if (_Includes.Count > 0)
                {
                    _Includes.ForEach(x => query = query.Include(x));
                }

                return query;
            }
        }

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to the unit of work
        /// to be persisted and inserted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// inserted into the database.</param>
        /// <remarks>Implementors of this method must handle the PersistAdd scenario.</remarks>
        public override void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Adds a transient instance of <typeparamref cref="TEntity"/> to the unit of work
        /// to be persisted and deleted by the repository.
        /// </summary>
        /// <param name="entity">An instance of <typeparamref name="TEntity"/> that should be
        /// deleted from the database.</param>
        /// <remarks>Implementors of this method must handle the PersistDelete scneario.</remarks>
        public override void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Attaches a detached entity.
        /// </summary>
        /// <param name="entity">The entity instance to attach back to the repository.</param>
        public override void Attach(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
        }

        /// <summary>
        /// Refreshes a entity instance.
        /// </summary>
        /// <param name="entity">The entity to refresh.</param>
        public override void Refresh(TEntity entity)
        {
            Context.Entry<TEntity>(entity).Reload();
        }

        /// <summary>
        /// Gets the paged.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override IQueryable<TEntity> GetPaged<TProperty>(Int32 pageIndex, Int32 pageCount, Expression<Func<TEntity, TProperty>> orderByExpression, Boolean ascending)
        {
            var set = Context.Set<TEntity>();
            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsQueryable();
            }

            return set.OrderByDescending(orderByExpression)
                      .Skip(pageCount * pageIndex)
                      .Take(pageCount)
                      .AsQueryable();
        }

        /// <summary>
        /// Queries the <see cref="DbContext"/> via SQL, using the specified parameters.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override IQueryable<TEntity> SqlQuery(String sql, params Object[] parameters)
        {
            return Context.Set<TEntity>().SqlQuery(sql, parameters).AsQueryable();
        }

        /// <summary>
        /// When overriden by inheriting classes, applies the eager loading strategies on the repository.
        /// </summary>
        /// <param name="paths">An array of <see cref="GenericRepositoryBase{TEntity}.Expression"/> containing the paths to
        /// eagerly load.</param>
        protected override void ApplyEagerLoadingStrategy(Expression[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                return;
            }

            String currentPath = String.Empty;
            paths.ForEach(path =>
            {
                var visitor = new MemberAccessPathVisitor();
                visitor.Visit(path);
                currentPath = !String.IsNullOrEmpty(currentPath)
                                  ? String.Format("{0}.{1}", currentPath, visitor.Path)
                                  : visitor.Path;

                _Includes.Add(currentPath);
            });
        }

        #endregion
    }
}