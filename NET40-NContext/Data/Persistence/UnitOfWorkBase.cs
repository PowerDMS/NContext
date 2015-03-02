namespace NContext.Data.Persistence
{
    using System;
    using System.Threading;
    using System.Transactions;

    using Microsoft.FSharp.Core;

    using NContext.Common;
    using NContext.ErrorHandling.Errors;
    using NContext.Extensions;

    using LocalizedPersistenceError = NContext.ErrorHandling.Errors.Localization.NContextPersistenceError;

    /// <summary>
    /// Defines a common abstraction for transactional unit of work persistence.
    /// </summary>
    /// <remarks></remarks>
    public abstract class UnitOfWorkBase : IUnitOfWork, IEquatable<UnitOfWorkBase>
    {
        private readonly Guid _Id;

        private readonly AmbientContextManagerBase _AmbientContextManager;

        private readonly Lazy<Transaction> _ScopeTransactionFactory;

        private readonly Thread _ScopeThread;

        private readonly UnitOfWorkBase _Parent;

        private volatile TransactionStatus _Status;

        private Lazy<Transaction> _CurrentTransactionFactory;

        private Boolean _IsDisposed;

        private TransactionOptions _TransactionOptions;

        protected UnitOfWorkBase(AmbientContextManagerBase ambientContextManager)
            : this(ambientContextManager, null, new TransactionOptions())
        {
        }

        protected UnitOfWorkBase(AmbientContextManagerBase ambientContextManager, TransactionOptions transactionOptions)
            : this(ambientContextManager, null, transactionOptions)
        {
        }

        protected UnitOfWorkBase(AmbientContextManagerBase ambientContextManager, UnitOfWorkBase parent)
            : this(ambientContextManager, parent, new TransactionOptions())
        {
        }

        protected UnitOfWorkBase(AmbientContextManagerBase ambientContextManager, UnitOfWorkBase parent, TransactionOptions transactionOptions)
        {
            ValidateTransactionOptions(parent, transactionOptions);

            _Id = Guid.NewGuid();
            _AmbientContextManager = ambientContextManager;
            _Parent = parent;
            _TransactionOptions = transactionOptions;
            _ScopeThread = Thread.CurrentThread;
            _Status = TransactionStatus.InDoubt;
            _ScopeTransactionFactory =
                new Lazy<Transaction>(
                    _Parent == null
                        ? (Func<Transaction>)(() => new CommittableTransaction(TransactionOptions))
                        : (Func<Transaction>)(() => _Parent.ScopeTransaction));
        }

        /// <summary>
        /// Gets the parent <see cref="UnitOfWorkBase"/>. Usually a <see cref="CompositeUnitOfWork"/>.
        /// </summary>
        /// <remarks></remarks>
        public UnitOfWorkBase Parent
        {
            get
            {
                return _Parent;
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public TransactionStatus Status
        {
            get
            {
                return _Status;
            }
            protected set
            {
                _Status = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        protected Boolean IsDisposed
        {
            get
            {
                return _IsDisposed;
            }
            set
            {
                _IsDisposed = value;
            }
        }

        /// <summary>
        /// Gets the thread in which the <see cref="UnitOfWorkBase"/> instance was created.
        /// </summary>
        /// <value>The scope thread.</value>
        protected Thread ScopeThread
        {
            get
            {
                return _ScopeThread;
            }
        }

        protected internal Transaction ScopeTransaction
        {
            get
            {
                return _ScopeTransactionFactory.Value;
            }
        }

        /// <summary>
        /// Gets the thread-safe transaction to use during <see cref="Commit"/>.
        /// </summary>
        protected Transaction CurrentTransaction
        {
            get
            {
                return _CurrentTransactionFactory == null ? null : _CurrentTransactionFactory.Value;
            }
        }

        protected AmbientContextManagerBase AmbientContextManager
        {
            get
            {
                return _AmbientContextManager;
            }
        }

        /// <summary>
        /// Rollback the transaction (if applicable).
        /// </summary>
        public abstract void Rollback();

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        /// <param name="transactionScope">
        /// The transaction Scope.
        /// </param>
        /// <returns>
        /// The IServiceResponse{Boolean}.
        /// </returns>
        protected abstract IServiceResponse<Unit> CommitTransaction(TransactionScope transactionScope);

        private void ValidateTransactionOptions(UnitOfWorkBase parent, TransactionOptions transactionOptions)
        {
            if (transactionOptions.Timeout < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("transactionOptions.Timeout");
            }

            if (parent != null && !transactionOptions.IsolationLevel.Equals(parent.TransactionOptions.IsolationLevel))
            {
                throw new ArgumentException("When using nested units of work, all nested scopes must be configured to use exactly the same isolation level if they want to join the ambient transaction.");
            }

            if (parent != null && transactionOptions.Timeout < parent.TransactionOptions.Timeout)
            {
                ResetTransactionOptions(parent, transactionOptions);
            }
        }

        private void ResetTransactionOptions(UnitOfWorkBase parent, TransactionOptions transactionOptions)
        {
            var parentScope = parent;
            while (parentScope != null)
            {
                parentScope.TransactionOptions = transactionOptions;
                parentScope = parent.Parent;
            }
        }

        #region Implementation of IUnitOfWork

        /// <summary>
        /// Gets the identity for the unit of work instance.
        /// </summary>
        /// <remarks></remarks>
        public Guid Id
        {
            get
            {
                return _Id;
            }
        }

        /// <summary>
        /// Gets the additional information about the transaction.
        /// </summary>
        /// <value>The transaction information.</value>
        public TransactionInformation TransactionInformation
        {
            get
            {
                if (_IsDisposed)
                {
                    throw new ObjectDisposedException("UnitOfWorkBase");
                }

                if (Status != TransactionStatus.Active)
                {
                    return null;
                }

                return CurrentTransaction.TransactionInformation;
            }
        }

        public TransactionOptions TransactionOptions
        {
            get
            {
                return _TransactionOptions;
            }
            private set
            {
                _TransactionOptions = value;
            }
        }

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        /// <returns>IServiceResponse{Boolean}.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public IServiceResponse<Unit> Commit()
        {
            if (Status == TransactionStatus.Active)
            {
                throw new InvalidOperationException(String.Format(LocalizedPersistenceError.UnitOfWorkCommitting, Id));
            }

            if (Status == TransactionStatus.Committed)
            {
                throw new InvalidOperationException(String.Format(LocalizedPersistenceError.UnitOfWorkCommitted, Id));
            }

            if (!AmbientContextManager.CanCommitUnitOfWork(this))
            {
                return NContextPersistenceError.UnitOfWorkNonCommittable(Id).ToServiceResponse();
            }

            if (ScopeTransaction == null)
            {
                return NContextPersistenceError.ScopeTransactionIsNull().ToServiceResponse();
            }

            TransactionScope transactionScope = null;
            if (Parent == null && ScopeThread == Thread.CurrentThread)
            {
                // Use the existing CurrentTransaction.
                _CurrentTransactionFactory = new Lazy<Transaction>(() => ScopeTransaction);
                transactionScope = new TransactionScope(CurrentTransaction, TransactionOptions.Timeout);
            }
            else if (ScopeThread != Thread.CurrentThread)
            {
                // UnitOfWork is being committed on a different thread.
                _CurrentTransactionFactory = new Lazy<Transaction>(() => ScopeTransaction.DependentClone(DependentCloneOption.BlockCommitUntilComplete));
                transactionScope = new TransactionScope(CurrentTransaction, TransactionOptions.Timeout);
            }

            _Status = TransactionStatus.Active;

            return CommitTransaction(transactionScope)
                       .Catch(_ => { _Status = TransactionStatus.Aborted; })
                       .Bind(_ =>
                           {
                               _Status = TransactionStatus.Committed;
                               if (CurrentTransaction is DependentTransaction)
                               {
                                   if (transactionScope != null)
                                   {
                                       transactionScope.Complete();
                                       transactionScope.Dispose();
                                   }

                                   var dependentTransaction = CurrentTransaction as DependentTransaction;
                                   dependentTransaction.Complete();
                                   dependentTransaction.Dispose();
                               }
                               else if (CurrentTransaction is CommittableTransaction)
                               {
                                   try
                                   {
                                       (CurrentTransaction as CommittableTransaction).Commit();
                                   }
                                   catch (TransactionAbortedException abortedException)
                                   {
                                       return NContextPersistenceError.CommitFailed(Id, CurrentTransaction.TransactionInformation.LocalIdentifier, abortedException)
                                                                      .ToServiceResponse();
                                   }
                               }

                               return new DataResponse<Unit>(default(Unit));
                           });
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Finalizes an instance of the <see cref="UnitOfWorkBase" /> class.
        /// </summary>
        ~UnitOfWorkBase()
        {
            Dispose(false);
        }

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

            if (disposeManagedResources && AmbientContextManager.CanDisposeUnitOfWork(this))
            {
                DisposeManagedResources();
                IsDisposed = true;
            }
        }

        protected abstract void DisposeManagedResources();

        #endregion

        #region Implementation of IEquatable

        public Boolean Equals(UnitOfWorkBase other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _Id.Equals(other._Id);
        }

        public override Boolean Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((UnitOfWorkBase)obj);
        }

        public override Int32 GetHashCode()
        {
            return _Id.GetHashCode();
        }

        #endregion
    }
}