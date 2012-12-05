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
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Transactions;

    using Castle.DynamicProxy;

    using NContext.Data.Persistence;

    /// <summary>
    /// Provides creation for all persistence-related operations including UnitOfWork and Repositories.
    /// </summary>
    public class EfPersistenceFactory : PersistenceFactoryBase, IEfPersistenceFactory
    {
        private readonly IDbContextFactory _DbContextFactory;

        private readonly static Lazy<ProxyGenerator> _ProxyGenerator = new Lazy<ProxyGenerator>(() => new ProxyGenerator());
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EfPersistenceFactory" /> class.
        /// </summary>
        public EfPersistenceFactory()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfPersistenceFactory" /> class.
        /// </summary>
        /// <param name="contextManagerFactory">The context manager factory.</param>
        public EfPersistenceFactory(IAmbientContextManagerFactory contextManagerFactory)
            : this(null, contextManagerFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfPersistenceFactory" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The db context factory.</param>
        public EfPersistenceFactory(IDbContextFactory dbContextFactory)
            : this(dbContextFactory, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfPersistenceFactory" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The db context factory.</param>
        /// <param name="contextManagerFactory">The context manager factory.</param>
        public EfPersistenceFactory(IDbContextFactory dbContextFactory, IAmbientContextManagerFactory contextManagerFactory)
            : base(contextManagerFactory)
        {
            _DbContextFactory = dbContextFactory ?? new ServiceLocatorDbContextFactory();
        }

        protected static ProxyGenerator ProxyGenerator
        {
            get
            {
                return _ProxyGenerator.Value;
            }
        }

        #region Overrides of PersistenceFactoryBase

        /// <summary>
        /// <para>
        /// If <paramref name="transactionScopeOption" /> equals <see cref="TransactionScopeOption.RequiresNew" />,
        /// then a new instance of <see cref="EfUnitOfWork" /> is created along with a new <see cref="DbContextContainer" />.
        /// </para>
        /// <para>
        /// If <paramref name="transactionScopeOption" /> equals <see cref="TransactionScopeOption.Required" />, then
        /// if an ambient unit of work exists <see cref="AmbientUnitOfWorkDecorator" /> if one exists, else we create a
        /// new <see cref="EfUnitOfWork" /> and <see cref="DbContextContainer" /> which becomes the ambient unit of work.
        /// </para>
        /// <para>The default transaction scope is <see cref="TransactionScopeOption.Required" />.</para>
        /// </summary>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <param name="transactionOptions">The transaction options.</param>
        /// <returns>Instance of <see cref="EfUnitOfWork" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">transactionScopeOption</exception>
        public override IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption, TransactionOptions transactionOptions)
        {
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                    return GetRequiredUnitOfWork(transactionOptions);
                case TransactionScopeOption.RequiresNew:
                    return GetRequiredNewUnitOfWork(transactionOptions);
                case TransactionScopeOption.Suppress:
                    return base.CreateUnitOfWork(transactionScopeOption);
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        private IUnitOfWork GetRequiredUnitOfWork(TransactionOptions transactionOptions)
        {
            if (!AmbientContextManager.AmbientExists || !AmbientContextManager.AmbientUnitOfWorkIsValid)
            {
                return GetRequiredNewUnitOfWork(transactionOptions);
            }

            /* First check if the ambient is a CompositeUnitOfWork and add to its collection.
             * This must come prior to checking whether it is not an instance of IEfUnitOfWork so we can 
             * add it as a leaf to a composite.
             * */
            var ambient = AmbientContextManager.Ambient;
            if (ambient.IsTypeOf<CompositeUnitOfWork>())
            {
                UnitOfWorkBase unitOfWork;
                var currentCompositeUnitOfWork = (CompositeUnitOfWork)AmbientContextManager.Ambient.UnitOfWork;
                if (currentCompositeUnitOfWork.ContainsType<IEfUnitOfWork>())
                {
                    unitOfWork = currentCompositeUnitOfWork.Single(uow => uow.GetType().Implements<IEfUnitOfWork>());
                }
                else
                {
                    unitOfWork = new EfUnitOfWork(AmbientContextManager, new DbContextContainer(), currentCompositeUnitOfWork);
                    currentCompositeUnitOfWork.Add(unitOfWork);
                }

                AmbientContextManager.AddUnitOfWork(unitOfWork);

                return unitOfWork;
            }
            
            // Current ambient is not Entity Framework.
            if (!AmbientContextManager.Ambient.IsTypeOf<IEfUnitOfWork>())
            {
                return GetRequiredNewUnitOfWork(transactionOptions);
            }
            
            // Current ambient implements IEfUnitOfWork so let's simply retain it by incrementing the ambient session count.
            AmbientContextManager.RetainAmbient();

            return AmbientContextManager.Ambient.UnitOfWork;
        }

        private IUnitOfWork GetRequiredNewUnitOfWork(TransactionOptions transactionOptions)
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
            if (!AmbientEfUnitOfWorkExists())
            {
                return _DbContextFactory.Create(registeredNameForServiceLocation);
            }

            return GetOrCreateDbContextForUnitOfWork(registeredNameForServiceLocation);
        }

        /// <summary>
        /// Gets the specified context from the ambient <see cref="IEfUnitOfWork" /> if one exists, else it tries to create a new one via service location.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the <see cref="DbContext"/>.</typeparam>
        /// <returns>Instance of <see cref="DbContext" />.</returns>
        public TDbContext GetOrCreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            if (!AmbientEfUnitOfWorkExists())
            {
                return _DbContextFactory.Create<TDbContext>();
            }

            return GetOrCreateDbContextForUnitOfWork<TDbContext>();
        }

        private DbContext GetOrCreateDbContextForUnitOfWork(String registeredNameForServiceLocation)
        {
            var unitOfWork = (IEfUnitOfWork)AmbientContextManager.Ambient.UnitOfWork;
            return unitOfWork.DbContextContainer.GetContext(registeredNameForServiceLocation) ??
                   new Func<DbContext>(
                       () =>
                           {
                               var context = CreateDbContextProxy(_DbContextFactory.Create(registeredNameForServiceLocation));
                               unitOfWork.DbContextContainer.Add(registeredNameForServiceLocation, context);

                               return context;
                           }).Invoke();
        }

        private TDbContext GetOrCreateDbContextForUnitOfWork<TDbContext>() where TDbContext : DbContext
        {
            var unitOfWork = (IEfUnitOfWork)AmbientContextManager.Ambient.UnitOfWork;
            return unitOfWork.DbContextContainer.GetContext<TDbContext>() ?? 
                new Func<TDbContext>(
                    () =>
                        {
                            var context = CreateDbContextProxy(_DbContextFactory.Create<TDbContext>());
                            unitOfWork.DbContextContainer.Add(context);

                            return context;
                        }).Invoke();
        }

        private TDbContext CreateDbContextProxy<TDbContext>(TDbContext context) where TDbContext : DbContext
        {
            Contract.Requires(context != null);

            var contextProxy = ProxyGenerator.CreateClassProxyWithTarget(
                context.GetType(),
                context, 
                new ProxyGenerationOptions(new DbContextProxyGenerationHook()), 
                new Object[] { context.Database.Connection.ConnectionString },
                new DisposeInterceptor()) as TDbContext;

            var disposableMixin =
                (((IProxyTargetAccessor)contextProxy)
                    .GetInterceptors()
                    .Single(interceptor => interceptor is DisposeInterceptor)) as IDisposableMixin;

            var unitOfWork = (IEfUnitOfWork)AmbientContextManager.Ambient.UnitOfWork;
            disposableMixin.SetDisposePredicate(() => unitOfWork.DbContextContainer.IsDisposing);

            return contextProxy;
        }

        private Boolean AmbientEfUnitOfWorkExists()
        {
            if (!AmbientContextManager.AmbientExists ||
                !AmbientContextManager.AmbientUnitOfWorkIsValid ||
                !AmbientContextManager.Ambient.IsTypeOf<IEfUnitOfWork>())
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}