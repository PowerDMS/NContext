namespace NContext.Data.Persistence
{
    using System;
    using System.Transactions;

    /// <summary>
    /// Defines a general abstraction for creating further datastore-specific implementations - 
    /// responsible for managing the creation and retainment of existing ambient <see cref="IUnitOfWork"/> instances.
    /// </summary>
    public abstract class PersistenceFactoryBase
    {
        private readonly Lazy<AmbientContextManagerBase> _AmbientContextManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceFactoryBase" /> class.
        /// </summary>
        /// <param name="contextManagerFactory">The context manager factory.</param>
        protected PersistenceFactoryBase(IAmbientContextManagerFactory contextManagerFactory)
        {
            var factoryMethod = contextManagerFactory == null
                                    ? new Func<AmbientContextManagerBase>(CreateDefaultAmbientContextManager)
                                    : new Func<AmbientContextManagerBase>(contextManagerFactory.Create);

            _AmbientContextManager = new Lazy<AmbientContextManagerBase>(factoryMethod);
        }

        /// <summary>
        /// Gets the ambient context manager.
        /// </summary>
        /// <value>The ambient context manager.</value>
        protected internal AmbientContextManagerBase AmbientContextManager
        {
            get
            {
                return _AmbientContextManager.Value;
            }
        }

        /// <summary>
        /// Creates the unit of work.
        /// </summary>
        /// <returns>IUnitOfWork instance.</returns>
        public virtual IUnitOfWork CreateUnitOfWork()
        {
            return CreateUnitOfWork(TransactionScopeOption.Required);
        }

        /// <summary>
        /// Creates the unit of work.
        /// </summary>
        /// <param name="transactionScopeOption">The <see cref="TransactionScopeOption"/>.</param>
        /// <returns>IUnitOfWork instance.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public virtual IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption)
        {
            return CreateUnitOfWork(transactionScopeOption, new TransactionOptions());
        }

        /// <summary>
        /// Creates the unit of work.
        /// </summary>
        /// <param name="transactionScopeOption">The <see cref="TransactionScopeOption" />.</param>
        /// <param name="transactionOptions">The transaction options.</param>
        /// <returns>IUnitOfWork instance.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public abstract IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption, TransactionOptions transactionOptions);

        /// <summary>
        /// Creates the default transaction manager. Local Default: <see cref="PerThreadAmbientContextManager" />.
        /// </summary>
        /// <returns>AmbientContextManagerBase.</returns>
        protected virtual AmbientContextManagerBase CreateDefaultAmbientContextManager()
        {
            return new PerThreadAmbientContextManager();
        }
    }
}