// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkBase.cs" company="Waking Venture, Inc.">
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
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        #region Fields

        private readonly Guid _Id;

        private readonly AmbientContextManagerBase _AmbientContextManager;

        private readonly Lazy<Transaction> _ScopeTransactionFactory;

        private readonly Thread _ScopeThread;

        private readonly UnitOfWorkBase _Parent;

        private readonly PersistenceOptions _PersistenceOptions;

        private volatile TransactionStatus _Status;

        private Lazy<Transaction> _CurrentTransactionFactory;

        private Boolean _IsDisposed;

        #endregion

        protected UnitOfWorkBase(AmbientContextManagerBase ambientContextManager)
            : this(ambientContextManager, null, new PersistenceOptions())
        {
        }

        protected UnitOfWorkBase(AmbientContextManagerBase ambientContextManager, PersistenceOptions persistenceOptions)
            : this(ambientContextManager, null, persistenceOptions)
        {
        }

        protected UnitOfWorkBase(AmbientContextManagerBase ambientContextManager, UnitOfWorkBase parent)
            : this(ambientContextManager, parent, new PersistenceOptions())
        {
        }

        protected UnitOfWorkBase(AmbientContextManagerBase ambientContextManager, UnitOfWorkBase parent, PersistenceOptions persistenceOptions)
        {
            _Id = Guid.NewGuid();
            _AmbientContextManager = ambientContextManager;
            _Parent = parent;
            _PersistenceOptions = persistenceOptions;
            _ScopeThread = Thread.CurrentThread;
            _Status = TransactionStatus.InDoubt;
            _ScopeTransactionFactory =
                new Lazy<Transaction>(
                    _Parent == null
                        ? (Func<Transaction>)(() => new CommittableTransaction())
                        : (Func<Transaction>)(() => _Parent.ScopeTransaction));
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
        /// <param name="transactionScope">
        /// The transaction Scope.
        /// </param>
        /// <returns>
        /// The IResponseTransferObject{Boolean}.
        /// </returns>
        protected abstract IResponseTransferObject<Unit> CommitTransaction(TransactionScope transactionScope);

        #region Implementation of IUnitOfWork

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        /// <returns>IResponseTransferObject{Boolean}.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public IResponseTransferObject<Unit> Commit()
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
                transactionScope = new TransactionScope(CurrentTransaction, PersistenceOptions.TransactionTimeOut);
            }
            else if (ScopeThread != Thread.CurrentThread)
            {
                // UnitOfWork is being committed on a different thread.
                _CurrentTransactionFactory = new Lazy<Transaction>(() => ScopeTransaction.DependentClone(DependentCloneOption.BlockCommitUntilComplete));
                transactionScope = new TransactionScope(CurrentTransaction, PersistenceOptions.TransactionTimeOut);
            }

            if (Parent == null && Transaction.Current != null)
            {
                Transaction transaction = Transaction.Current;
                transaction.TransactionCompleted +=
                    (sender, args) =>
                        {
                            Console.WriteLine(
                                "Transaction {0} has {1}.",
                                args.Transaction.TransactionInformation.LocalIdentifier,
                                args.Transaction.TransactionInformation.Status);
                        };
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

                               return new ServiceResponse<Unit>(default(Unit));
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
    }
}