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

using System;
using System.Transactions;

namespace NContext.Data
{
    /// <summary>
    /// Defines a common abstraction for transactional unit of work persistence.
    /// </summary>
    /// <remarks></remarks>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private readonly Guid _Id;

        private readonly TransactionScopeOption _TransactionScopeOption;

        private Boolean _IsDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkBase"/> class.
        /// </summary>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <remarks></remarks>
        protected UnitOfWorkBase(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            _Id = Guid.NewGuid();
            _TransactionScopeOption = transactionScopeOption;
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
        /// Gets the transaction scope option.
        /// </summary>
        /// <remarks></remarks>
        protected TransactionScopeOption TransactionScopeOption
        {
            get
            {
                return _TransactionScopeOption;
            }
        }

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        public void Commit()
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption))
            {
                try
                {
                    CommitChanges();
                }
                catch
                {
                    // TODO: (DG) Logging?

                    Rollback();
                }
                finally
                {
                    CommitTransaction(transactionScope);
                }
            }
        }

        /// <summary>
        /// Commits the changes within the persistence context.
        /// </summary>
        /// <remarks></remarks>
        protected abstract void CommitChanges();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="transactionScope">The transaction scope.</param>
        /// <remarks></remarks>
        protected virtual void CommitTransaction(TransactionScope transactionScope)
        {
            if (transactionScope == null)
            {
                return;
            }

            transactionScope.Complete();
        }
        
        /// <summary>
        /// Rollback the transaction (if applicable).
        /// </summary>
        protected abstract void Rollback();

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
        protected abstract void Dispose(Boolean disposeManagedResources);

        #endregion
    }
}