namespace NContext.Data.Persistence
{
    using System;
    using System.Transactions;

    using NContext.ErrorHandling.Errors;
    using NContext.Extensions;

    /// <summary>
    /// Defines a <see cref="System.Transactions"/> general persistence abstraction for <see cref="CompositeUnitOfWork"/>.
    /// </summary>
    public class PersistenceFactory : PersistenceFactoryBase, IPersistenceFactory
    {
        private readonly PersistenceOptions _PersistenceOptions;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceFactory"/> class.
        /// </summary>
        /// <remarks></remarks>
        public PersistenceFactory()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceFactory" /> class.
        /// </summary>
        /// <param name="contextManagerFactory">The context manager factory.</param>
        public PersistenceFactory(IAmbientContextManagerFactory contextManagerFactory)
            : this(contextManagerFactory, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceFactory"/> class.
        /// </summary>
        /// <param name="persistenceOptions">The persistence options.</param>
        /// <remarks></remarks>
        public PersistenceFactory(PersistenceOptions persistenceOptions)
            : this(null, persistenceOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceFactory" /> class.
        /// </summary>
        /// <param name="contextManagerFactory">The context manager factory.</param>
        /// <param name="persistenceOptions">The persistence options.</param>
        public PersistenceFactory(IAmbientContextManagerFactory contextManagerFactory, PersistenceOptions persistenceOptions)
            : base(contextManagerFactory)
        {
            _PersistenceOptions = persistenceOptions ?? new PersistenceOptions();
        }

        /// <summary>
        /// Gets the transactional persistence options that will govern any <see cref="IUnitOfWork"/> created by this instance.
        /// </summary>
        /// <value>The <see cref="PersistenceOptions"/>.</value>
        public PersistenceOptions Options
        {
            get
            {
                return _PersistenceOptions;
            }
        }

        /// <summary>
        /// Creates the unit of work.
        /// </summary>
        /// <returns>IUnitOfWork instance.</returns>
        public override IUnitOfWork CreateUnitOfWork()
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
        public override IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption)
        {
            return CreateUnitOfWork(transactionScopeOption, Options.TransactionOptions);
        }
        
        /// <summary>
        /// Creates an <see cref="IUnitOfWork" /> instance.
        /// </summary>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <param name="transactionOptions">The transaction options.</param>
        /// <returns>IUnitOfWork instance.</returns>
        public override IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption, TransactionOptions transactionOptions)
        {
            var persistenceOptions = GetUnitOfWorkPersistenceOptions(transactionOptions);
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                    return GetRequiredUnitOfWork(persistenceOptions);
                case TransactionScopeOption.RequiresNew:
                    return GetRequiredNewUnitOfWork(persistenceOptions);
                case TransactionScopeOption.Suppress:
                    throw new NotImplementedException("Suppress is currently not supported and in development.");
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        private IUnitOfWork GetRequiredUnitOfWork(PersistenceOptions persistenceOptions)
        {
            if (!AmbientContextManager.AmbientExists)
            {
                return GetRequiredNewUnitOfWork(persistenceOptions);
            }

            if (!AmbientContextManager.Ambient.IsTypeOf<CompositeUnitOfWork>())
            {
                throw NContextPersistenceError.CompositeUnitOfWorkWithinDifferentAmbientType(AmbientContextManager.Ambient.UnitOfWork.GetType())
                                              .ToException<InvalidOperationException>();
            }

            // TODO: (DG) If ambient is a Composite do we retain or add to it?
            //if ()
            //{
            //    var currentCompositeUnitOfWork = (CompositeUnitOfWork)AmbientContextManager.Ambient.UnitOfWork;
            //    unitOfWork = new CompositeUnitOfWork(AmbientContextManager, currentCompositeUnitOfWork, _PersistenceOptions);
            //    currentCompositeUnitOfWork.AddUnitOfWork(unitOfWork);
            //    AmbientContextManager.AddUnitOfWork(unitOfWork);
            //}

            AmbientContextManager.RetainAmbient();

            return AmbientContextManager.Ambient.UnitOfWork;
        }

        private IUnitOfWork GetRequiredNewUnitOfWork(PersistenceOptions persistenceOptions)
        {
            UnitOfWorkBase unitOfWork = new CompositeUnitOfWork(AmbientContextManager, persistenceOptions);
            AmbientContextManager.AddUnitOfWork(unitOfWork);

            return unitOfWork;
        }

        private PersistenceOptions GetUnitOfWorkPersistenceOptions(TransactionOptions transactionOptions)
        {
            if (!transactionOptions.Equals(Options.TransactionOptions))
            {
                return new PersistenceOptions(transactionOptions.Timeout, transactionOptions.IsolationLevel, Options.MaxDegreeOfParallelism);
            }

            return Options;
        }
    }
}