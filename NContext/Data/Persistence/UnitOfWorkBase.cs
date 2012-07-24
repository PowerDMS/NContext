// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkBase.cs">
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
//   Defines a common abstraction for transactional unit of work persistence.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Data.Persistence
{
    using System;
    using System.Transactions;
    using System.Threading;

    /// <summary>
    /// Defines a common abstraction for transactional unit of work persistence.
    /// </summary>
    /// <remarks></remarks>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        #region Fields

        private readonly Guid _Id;

        private readonly AmbientTransactionManagerBase _TransactionManager;

        private readonly Lazy<Transaction> _TransactionFactory;

        private readonly Thread _ScopeThread;

        private readonly UnitOfWorkBase _Parent;

        private readonly PersistenceOptions _PersistenceOptions;

        private Boolean _IsDisposed;

        private volatile Boolean _IsCommitted;

        private Lazy<Transaction> _ThreadSafeTransaction;

        #endregion

        protected UnitOfWorkBase(AmbientTransactionManagerBase transactionManager)
            : this(transactionManager, () => null, null, null)
        {
        }

        protected UnitOfWorkBase(AmbientTransactionManagerBase transactionManager, PersistenceOptions persistenceOptions)
            : this(transactionManager, () => null, null, persistenceOptions)
        {
        }

        protected UnitOfWorkBase(AmbientTransactionManagerBase transactionManager, Func<Transaction> transactionFactory, UnitOfWorkBase parent)
            : this(transactionManager, transactionFactory, parent, null)
        {
        }

        protected UnitOfWorkBase(AmbientTransactionManagerBase transactionManager, Func<Transaction> transactionFactory, PersistenceOptions persistenceOptions)
            : this(transactionManager, transactionFactory, null, persistenceOptions)
        {
        }

        protected UnitOfWorkBase(AmbientTransactionManagerBase transactionManager, Func<Transaction> transactionFactory, UnitOfWorkBase parent, PersistenceOptions persistenceOptions)
        {
            if (transactionFactory == null)
            {
                throw new ArgumentNullException("transactionManager");
            }

            _Id = Guid.NewGuid();
            _ScopeThread = Thread.CurrentThread;
            _TransactionManager = transactionManager;
            _TransactionFactory = new Lazy<Transaction>(transactionFactory);
            _Parent = parent;
            _PersistenceOptions = persistenceOptions ?? PersistenceOptions.Default;
        }

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

        public Thread ScopeThread
        {
            get
            {
                return _ScopeThread;
            }
        }

        public Transaction ScopeTransaction
        {
            get
            {
                return _TransactionFactory.Value;
            }
        }

        public UnitOfWorkBase Parent
        {
            get
            {
                return _Parent;
            }
        }

        protected Transaction ThreadSafeTransaction
        {
            get
            {
                return _ThreadSafeTransaction == null ? null : _ThreadSafeTransaction.Value;
            }
        }

        protected AmbientTransactionManagerBase TransactionManager
        {
            get
            {
                return _TransactionManager;
            }
        }

        public Boolean IsCommitted
        {
            get
            {
                return _IsCommitted;
            }
            protected set
            {
                _IsCommitted = value;
            }
        }

        protected PersistenceOptions PersistenceOptions
        {
            get
            {
                return _PersistenceOptions;
            }
        }

        /// <summary>
        /// Rollback the transaction (if applicable).
        /// </summary>
        public abstract void Rollback();

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        protected abstract void CommitTransaction(TransactionScope transactionScope);

        #region Implementation of IUnitOfWork

        public void Commit()
        {
            if (IsCommitted)
            {
                throw new InvalidOperationException(String.Format("Unit of work instance id: {0} has already been committed.", Id));
            }
            
            if (!TransactionManager.CanCommitUnitOfWork(this))
            {
                return;
            }

            if (ScopeTransaction != null && ScopeThread != Thread.CurrentThread)
            {
                _ThreadSafeTransaction = new Lazy<Transaction>(() => ScopeTransaction.DependentClone(DependentCloneOption.BlockCommitUntilComplete));
            }
            else if (ScopeTransaction != null)
            {
                _ThreadSafeTransaction = new Lazy<Transaction>(() => ScopeTransaction);
            }

            _IsCommitted = true;
            
            var transactionScope = ThreadSafeTransaction == null
                                       ? new TransactionScope(TransactionScopeOption.Suppress)
                                       : new TransactionScope(ThreadSafeTransaction, PersistenceOptions.TransactionTimeOut);

            CommitTransaction(transactionScope);

            var threadSafeTransaction = ThreadSafeTransaction as DependentTransaction;
            if (threadSafeTransaction != null)
            {
                threadSafeTransaction.Complete();
            }
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="UnitOfWorkBase"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks></remarks>
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

            if (disposeManagedResources)
            {
                if (TransactionManager.CanDisposeUnitOfWork(this))
                {
                    DisposeManagedResources();
                }
            }
        }

        protected abstract void DisposeManagedResources();

        #endregion
    }
}