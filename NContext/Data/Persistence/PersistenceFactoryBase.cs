namespace NContext.Data.Persistence
{
    using System;
    using System.Transactions;

    public abstract class PersistenceFactoryBase
    {
        private readonly Lazy<AmbientTransactionManagerBase> _TransactionManager;

        protected PersistenceFactoryBase() : this(null)
        {
        }

        protected PersistenceFactoryBase(IAmbientTransactionManagerFactory transactionManagerFactory)
        {
            var factoryMethod = transactionManagerFactory == null
                                    ? () => CreateUnitOfWorkManager()
                                    : transactionManagerFactory.Create();

            _TransactionManager = new Lazy<AmbientTransactionManagerBase>(factoryMethod);
        }

        protected AmbientTransactionManagerBase TransactionManager
        {
            get
            {
                return _TransactionManager.Value;
            }
        }

        public virtual IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                case TransactionScopeOption.RequiresNew:
                    throw new NotSupportedException("PersistenceFactoryBase does not support TransactionScopeOption.Required or TransactionScopeOption.RequiresNew.");
                case TransactionScopeOption.Suppress:
                    TransactionManager.AddUnitOfWork(null);
                    return new NonAtomicUnitOfWork(TransactionManager);
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        protected virtual AmbientTransactionManagerBase CreateUnitOfWorkManager()
        {
            return new ThreadLocalTransactionManager();
        }
    }
}