// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistenceFactory.cs">
//   Copyright (c) 2012
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
//
// <summary>
//   Provides creation for all persistence-related operations including UnitOfWork and Repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Transactions;

namespace NContext.Extensions.EntityFramework
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
        public static IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
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
        public static IEfGenericRepository<TEntity> CreateRepository<TEntity>() 
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
        public static IEfGenericRepository<TEntity> CreateRepository<TEntity, TDbContext>()
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
        public static IEfGenericRepository<TEntity> CreateRepository<TEntity>(String registeredNameForServiceLocation)
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
        public static DbContext GetOrCreateDefaultContext()
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
        public static DbContext GetOrCreateContext(String registeredNameForServiceLocation)
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
        public static TDbContext GetOrCreateContext<TDbContext>()
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