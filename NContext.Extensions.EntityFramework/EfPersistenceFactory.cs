﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfPersistenceFactory.cs">
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

namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Transactions;

    using NContext.Data.Persistence;

    /// <summary>
    /// Provides creation for all persistence-related operations including UnitOfWork and Repositories.
    /// </summary>
    public class EfPersistenceFactory : PersistenceFactoryBase, IEfPersistenceFactory
    {
        private readonly IDbContextFactory _DbContextFactory;

        #region Constructors

        public EfPersistenceFactory()
            : this(null, null)
        {
        }

        public EfPersistenceFactory(IDbContextFactory dbContextFactory, IAmbientTransactionManagerFactory transactionManagerFactory)
            : base(transactionManagerFactory)
        {
            _DbContextFactory = dbContextFactory ?? new ServiceLocatorDbContextFactory();
        }

        #endregion

        #region Overrides of PersistenceFactoryBase

        /// <summary>
        /// <para>
        /// If <paramref name="transactionScopeOption"/> equals <see cref="TransactionScopeOption.RequiresNew"/> or 
        /// <see cref="TransactionScopeOption.Suppress"/>, than a new instance of <see cref="EfUnitOfWork"/> is 
        /// created along with a fresh <see cref="ContextContainer"/>.
        /// </para>
        /// <para>
        /// If <paramref name="transactionScopeOption"/> equals <see cref="TransactionScopeOption.Required"/>, than 
        /// we are returned the <see cref="AmbientUnitOfWork"/> if one exists, else we create a
        /// new <see cref="EfUnitOfWork"/> and <see cref="ContextContainer"/> which becomes the ambient unit of work.
        /// </para>
        /// <para>The default transaction scope is <see cref="TransactionScopeOption.Required"/>.</para>
        /// </summary>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <returns>Instance of <see cref="EfUnitOfWork"/>.</returns>
        /// <remarks></remarks>
        public override IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            UnitOfWorkBase unitOfWork;
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                    if (TransactionManager.AmbientExists && TransactionManager.AmbientIsValid)
                    {
                        if (TransactionManager.Ambient.IsTypeOf<CompositeUnitOfWork>())
                        {
                            var currentCompositeUnitOfWork = ((CompositeUnitOfWork)TransactionManager.Ambient.UnitOfWork);
                            unitOfWork = new EfUnitOfWork(TransactionManager, new ContextContainer(), currentCompositeUnitOfWork);
                            currentCompositeUnitOfWork.AddUnitOfWork(unitOfWork);
                            TransactionManager.AddUnitOfWork(unitOfWork);
                        }
                        else
                        {
                            unitOfWork = TransactionManager.Ambient.UnitOfWork;
                            TransactionManager.RetainAmbient();
                        }
                    }
                    else
                    {
                        unitOfWork = new EfUnitOfWork(TransactionManager, new ContextContainer());
                        TransactionManager.AddUnitOfWork(unitOfWork);
                    }

                    return unitOfWork;
                case TransactionScopeOption.RequiresNew:
                    unitOfWork = new EfUnitOfWork(TransactionManager, new ContextContainer());
                    TransactionManager.AddUnitOfWork(unitOfWork);

                    return unitOfWork;
                case TransactionScopeOption.Suppress:
                    return base.CreateUnitOfWork(transactionScopeOption);
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        #endregion

        #region Implementation of IEfPersistenceFactory

        /// <summary>
        /// Creates an <see cref="IEfGenericRepository{TEntity}"/> instance using the application's default <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <returns>Instance of <see cref="IEfGenericRepository{TEntity}"/>.</returns>
        /// <remarks></remarks>
        public IEfGenericRepository<TEntity> CreateRepository<TEntity>() where TEntity : class, IEntity
        {
            if (TransactionManager.Ambient == null || !TransactionManager.Ambient.IsTypeOf<IEfUnitOfWork>())
            {
                throw new Exception("A repository must be created within the scope of an existing IEfUnitOfWork instance.");
            }

            return new EfGenericRepository<TEntity>(GetOrCreateDbContext());
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
            if (TransactionManager.Ambient == null)
            {
                throw new Exception("A repository must be created within the scope of an existing IEfUnitOfWork instance.");
            }

            return new EfGenericRepository<TEntity>(GetOrCreateDbContext<TDbContext>());
        }

        /// <summary>
        /// Creates an <see cref="IEfGenericRepository{TEntity}"/> instance using the <see cref="DbContext"/> registered with the specified key.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create.</typeparam>
        /// <param name="registeredDbContextNameForServiceLocation">The context's registered name for service location.</param>
        /// <returns>Instance of <see cref="IEfGenericRepository{TEntity}"/>.</returns>
        /// <remarks></remarks>
        public IEfGenericRepository<TEntity> CreateRepository<TEntity>(String registeredDbContextNameForServiceLocation)
            where TEntity : class, IEntity
        {
            if (TransactionManager.Ambient == null)
            {
                throw new Exception("A repository must be created within the scope of an existing IEfUnitOfWork instance.");
            }

            return new EfGenericRepository<TEntity>(GetOrCreateDbContext(registeredDbContextNameForServiceLocation));
        }

        /// <summary>
        /// Gets the default context from the ambient <see cref="IEfUnitOfWork"/> if one exists, else it tries to create a new context via service location.
        /// </summary>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        public DbContext GetOrCreateDbContext()
        {
            return GetOrCreateDbContext("default");
        }

        /// <summary>
        /// Gets the context from the ambient <see cref="IEfUnitOfWork"/> if one exists, 
        /// else it tries to create a new one via service location with the specified registered name.
        /// </summary>
        /// <param name="registeredNameForServiceLocation">The context's registered name for service location.</param>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        public DbContext GetOrCreateDbContext(String registeredNameForServiceLocation)
        {
            if (TransactionManager.Ambient == null || !TransactionManager.Ambient.IsTypeOf<IEfUnitOfWork>())
            {
                throw new Exception("A DBContext must be created within the scope of a valid IEfUnitOfWork instance.");
            }

            var currentUnitOfWork = ((IEfUnitOfWork)TransactionManager.Ambient.UnitOfWork);
            return currentUnitOfWork.ContextContainer.GetContext(registeredNameForServiceLocation) ??
                   new Func<DbContext>(() =>
                       {
                           var context = _DbContextFactory.Create(registeredNameForServiceLocation);
                           currentUnitOfWork.ContextContainer.Add(registeredNameForServiceLocation, context);
                           return context;
                       }).Invoke();
        }

        /// <summary>
        /// Gets the specified context from the ambient <see cref="IEfUnitOfWork"/> if one exists, else it tries to create a new one via service location.
        /// </summary>
        /// <returns>Instance of <see cref="DbContext"/>.</returns>
        /// <remarks></remarks>
        public TDbContext GetOrCreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            if (TransactionManager.Ambient == null || !TransactionManager.Ambient.IsTypeOf<IEfUnitOfWork>())
            {
                throw new Exception("A DBContext must be created within the scope of a valid IEfUnitOfWork instance.");
            }

            var currentUnitOfWork = ((IEfUnitOfWork)TransactionManager.Ambient.UnitOfWork);
            return currentUnitOfWork.ContextContainer.GetContext<TDbContext>() ??
                new Func<TDbContext>(() =>
                {
                    var context = _DbContextFactory.Create<TDbContext>();
                    currentUnitOfWork.ContextContainer.Add(context);
                    return context;
                }).Invoke();
        }

        #endregion
    }
}