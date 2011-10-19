// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistenceFactory.cs">
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
//   Provides creation for all persistence-related operations including UnitOfWork and Repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Transactions;

using NContext.Application.Domain;

namespace NContext.Persistence.EntityFramework
{
    /// <summary>
    /// Provides creation for all persistence-related operations including UnitOfWork and Repositories.
    /// </summary>
    public class PersistenceFactory
    {
        #region Methods

        /// <summary>
        /// <para>
        /// If <paramref name="transactionScopeOption"/> equals <see cref="TransactionScopeOption.RequiresNew"/> or 
        /// <see cref="TransactionScopeOption.Suppress"/>, than a new instance of <see cref="EfUnitOfWork"/> is 
        /// created along with a fresh <see cref="ContextContainer"/>.
        /// </para>
        /// <para>
        /// If <paramref name="transactionScopeOption"/> equals <see cref="TransactionScopeOption.Required"/>, than 
        /// we are returned the <see cref="UnitOfWorkController.AmbientUnitOfWork"/> if one exists, else we create a
        /// new <see cref="EfUnitOfWork"/> and <see cref="ContextContainer"/> which becomes the ambient unit of work.
        /// </para>
        /// <para>The default transaction scope is <see cref="TransactionScopeOption.Required"/>.</para>
        /// </summary>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <returns>Instance of <see cref="EfUnitOfWork"/>.</returns>
        /// <remarks></remarks>
        public IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            if (transactionScopeOption == TransactionScopeOption.RequiresNew ||
                transactionScopeOption == TransactionScopeOption.Suppress)
            {
                return new EfUnitOfWork(new ContextContainer(), transactionScopeOption);
            }

            if (UnitOfWorkController.AmbientUnitOfWork != null)
            {
                UnitOfWorkController.Retain();

                return UnitOfWorkController.AmbientUnitOfWork;
            }

            return new EfUnitOfWork(new ContextContainer(), transactionScopeOption);
        }

        /// <summary>
        /// Creates an <see cref="IEfGenericRepository{TEntity}"/> instance using the application's default <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <returns>Instance of <see cref="IEfGenericRepository{TEntity}"/>.</returns>
        /// <remarks></remarks>
        public IEfGenericRepository<TEntity> CreateRepository<TEntity>() 
            where TEntity : class, IEntity
        {
            if (UnitOfWorkController.AmbientUnitOfWork == null)
            {
                throw new Exception("A repository must be created within the scope of an existing IUnitOfWork instance.");
            }

            return new EfGenericRepository<TEntity>(GetOrCreateDefaultContext());
        }

        /// <summary>
        /// Creates an <see cref="IEfGenericRepository{TEntity}"/> instance using the specified <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <typeparam name="TDbContext">The type of <see cref="DbContext"/>.</typeparam>
        /// <returns>Instance of <see cref="IEfGenericRepository{TEntity}"/>.</returns>
        /// <remarks></remarks>
        public IEfGenericRepository<TEntity> CreateRepository<TEntity, TDbContext>()
            where TEntity : class, IEntity
            where TDbContext : DbContext
        {
            if (UnitOfWorkController.AmbientUnitOfWork == null)
            {
                throw new Exception("A repository must be created within the scope of an existing IUnitOfWork instance.");
            }

            return new EfGenericRepository<TEntity>(GetOrCreateContext<TDbContext>());
        }

        /// <summary>
        /// Creates an <see cref="IEfGenericRepository{TEntity}"/> instance using the <see cref="DbContext"/> registered with the specified key.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <param name="registeredNameForServiceLocation">The context's registered name for service location.</param>
        /// <returns>Instance of <see cref="IEfGenericRepository{TEntity}"/>.</returns>
        /// <remarks></remarks>
        public IEfGenericRepository<TEntity> CreateRepository<TEntity>(String registeredNameForServiceLocation)
            where TEntity : class, IEntity
        {
            if (UnitOfWorkController.AmbientUnitOfWork == null)
            {
                throw new Exception("A repository must be created within the scope of an existing IUnitOfWork instance.");
            }

            return new EfGenericRepository<TEntity>(GetOrCreateContext(registeredNameForServiceLocation));
        }

        /// <summary>
        /// Gets the default context from the ambient <see cref="IUnitOfWork"/> if one exists, else it tries to create a new context via service location.
        /// </summary>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        public DbContext GetOrCreateDefaultContext()
        {
            if (UnitOfWorkController.AmbientUnitOfWork == null)
            {
                throw new Exception("A repository must be created within the scope of a valid IUnitOfWork instance.");
            }

            return UnitOfWorkController.AmbientUnitOfWork.ContextContainer.GetDefaultContext();
        }

        /// <summary>
        /// Gets the context from the ambient <see cref="IUnitOfWork"/> if one exists, 
        /// else it tries to create a new one via service location with the specified registered name.
        /// </summary>
        /// <param name="registeredNameForServiceLocation">The context's registered name for service location.</param>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        public DbContext GetOrCreateContext(String registeredNameForServiceLocation)
        {
            if (UnitOfWorkController.AmbientUnitOfWork == null)
            {
                throw new Exception("A repository must be created within the scope of a valid IUnitOfWork instance.");
            }

            return UnitOfWorkController.AmbientUnitOfWork.ContextContainer.GetContextFromServiceLocation(registeredNameForServiceLocation);
        }

        /// <summary>
        /// Gets the specified context from the ambient <see cref="IUnitOfWork"/> if one exists, else it tries to create a new one via service location.
        /// </summary>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        public TDbContext GetOrCreateContext<TDbContext>()
            where TDbContext : DbContext
        {
            if (UnitOfWorkController.AmbientUnitOfWork == null)
            {
                throw new Exception("A repository must be created within the scope of a valid IUnitOfWork instance.");
            }

            return UnitOfWorkController.AmbientUnitOfWork.ContextContainer.GetContext<TDbContext>();
        }

        #endregion
    }
}