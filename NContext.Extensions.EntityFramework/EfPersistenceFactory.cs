// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfPersistenceFactory.cs" company="Waking Venture, Inc.">
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
    using System.Data.Entity;
    using System.Transactions;

    using NContext.Data.Persistence;

    /// <summary>
    /// Provides creation for all persistence-related operations including UnitOfWork and Repositories.
    /// </summary>
    public class EfPersistenceFactory : PersistenceFactoryBase, IEfPersistenceFactory
    {
        private readonly IDbContextFactory _DbContextFactory;

        public EfPersistenceFactory()
            : this(null, null)
        {
        }

        public EfPersistenceFactory(IAmbientContextManagerFactory contextManagerFactory)
            : this(null, contextManagerFactory)
        {
        }

        public EfPersistenceFactory(IDbContextFactory dbContextFactory)
            : this(dbContextFactory, null)
        {
        }

        public EfPersistenceFactory(IDbContextFactory dbContextFactory, IAmbientContextManagerFactory contextManagerFactory)
            : base(contextManagerFactory)
        {
            _DbContextFactory = dbContextFactory ?? new ServiceLocatorDbContextFactory();
        }

        #region Overrides of PersistenceFactoryBase

        /// <summary>
        /// <para>
        /// If <paramref name="transactionScopeOption"/> equals <see cref="TransactionScopeOption.RequiresNew"/>, 
        /// then a new instance of <see cref="EfUnitOfWork"/> is created along with a new <see cref="DbContextContainer"/>.
        /// </para>
        /// <para>
        /// If <paramref name="transactionScopeOption"/> equals <see cref="TransactionScopeOption.Required"/>, then 
        /// if an ambient unit of work exists <see cref="AmbientUnitOfWorkDecorator"/> if one exists, else we create a
        /// new <see cref="EfUnitOfWork"/> and <see cref="DbContextContainer"/> which becomes the ambient unit of work.
        /// </para>
        /// <para>The default transaction scope is <see cref="TransactionScopeOption.Required"/>.</para>
        /// </summary>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <returns>Instance of <see cref="EfUnitOfWork"/>.</returns>
        /// <remarks></remarks>
        public override IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                    return GetRequiredUnitOfWork();
                case TransactionScopeOption.RequiresNew:
                    return GetRequiredNewUnitOfWork();
                case TransactionScopeOption.Suppress:
                    return base.CreateUnitOfWork(transactionScopeOption);
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        private IUnitOfWork GetRequiredUnitOfWork()
        {
            if (!AmbientContextManager.AmbientExists || !AmbientContextManager.AmbientUnitOfWorkIsValid)
            {
                return GetRequiredNewUnitOfWork();
            }

            var ambient = AmbientContextManager.Ambient;
            if (ambient.IsTypeOf<CompositeUnitOfWork>())
            {
                var currentCompositeUnitOfWork = (CompositeUnitOfWork)AmbientContextManager.Ambient.UnitOfWork;
                var unitOfWork = new EfUnitOfWork(AmbientContextManager, new DbContextContainer(), currentCompositeUnitOfWork);
                currentCompositeUnitOfWork.AddUnitOfWork(unitOfWork);
                AmbientContextManager.AddUnitOfWork(unitOfWork);

                return unitOfWork;
            }

            if (ambient.IsTypeOf<IEfUnitOfWork>())
            {
                var unitOfWork = AmbientContextManager.Ambient.UnitOfWork;
                AmbientContextManager.RetainAmbient();

                return unitOfWork;
            }

            return GetRequiredNewUnitOfWork();
        }

        private IUnitOfWork GetRequiredNewUnitOfWork()
        {
            var unitOfWork = new EfUnitOfWork(AmbientContextManager, new DbContextContainer());
            AmbientContextManager.AddUnitOfWork(unitOfWork);

            return unitOfWork;
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
            EnsureEfUnitOfWorkExists();

            var currentUnitOfWork = (IEfUnitOfWork)AmbientContextManager.Ambient.UnitOfWork;
            return currentUnitOfWork.DbContextContainer.GetContext(registeredNameForServiceLocation) ??
                   new Func<DbContext>(() =>
                       {
                           var context = _DbContextFactory.Create(registeredNameForServiceLocation);
                           currentUnitOfWork.DbContextContainer.Add(registeredNameForServiceLocation, context);
                           return context;
                       }).Invoke();
        }

        /// <summary>
        /// Gets the specified context from the ambient <see cref="IEfUnitOfWork" /> if one exists, else it tries to create a new one via service location.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the <see cref="DbContext"/>.</typeparam>
        /// <returns>Instance of <see cref="DbContext" />.</returns>
        public TDbContext GetOrCreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            EnsureEfUnitOfWorkExists();

            var currentUnitOfWork = ((IEfUnitOfWork)AmbientContextManager.Ambient.UnitOfWork);
            return currentUnitOfWork.DbContextContainer.GetContext<TDbContext>() ??
                new Func<TDbContext>(() =>
                {
                    var context = _DbContextFactory.Create<TDbContext>();
                    currentUnitOfWork.DbContextContainer.Add(context);
                    return context;
                }).Invoke();
        }

        private void EnsureEfUnitOfWorkExists()
        {
            if (!AmbientContextManager.AmbientExists || 
                !AmbientContextManager.AmbientUnitOfWorkIsValid || 
                !AmbientContextManager.Ambient.IsTypeOf<IEfUnitOfWork>())
            {
                CreateUnitOfWork();
            }
        }

        #endregion
    }
}