// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfUnitOfWork.cs">
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
//   Defines an Entity Framework 4 implementation of IEfUnitOfWork pattern.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;

using NContext.Data;

namespace NContext.Extensions.EntityFramework
{
    using System.Diagnostics;
    using System.Threading;

    using NContext.Data.Persistence;

    /// <summary>
    /// Defines an Entity Framework 4 implementation of IEfUnitOfWork.
    /// </summary>
    public class EfUnitOfWork : UnitOfWorkBase, IEfUnitOfWork
    {
        #region Fields

        private readonly IContextContainer _ContextContianer;

        #endregion

        #region Constructors
        
        protected internal EfUnitOfWork(AmbientTransactionManagerBase transactionManager, IContextContainer contextContainer)
            : this(transactionManager, () => new CommittableTransaction(), contextContainer, null)
        {
        }

        protected internal EfUnitOfWork(AmbientTransactionManagerBase transactionManager, IContextContainer contextContainer, UnitOfWorkBase parent)
            : this(transactionManager, () => parent.ScopeTransaction, contextContainer, parent)
        {
        }

        private EfUnitOfWork(AmbientTransactionManagerBase transactionManager, Func<Transaction> transactionFactory, IContextContainer contextContainer, UnitOfWorkBase parent)
            : base(transactionManager, transactionFactory, parent)
        {
            if (contextContainer == null)
            {
                throw new ArgumentNullException("contextContainer");
            }

            _ContextContianer = contextContainer;

            Debug.WriteLine(String.Format("EfUnitOfWork: {0} created.", Id));
        }
        
        #endregion

        #region Overrides of UnitOfWorkBase

        /// <summary>
        /// Commits the changes within the persistence context.
        /// </summary>
        /// <remarks></remarks>
        protected override void CommitTransaction(TransactionScope transactionScope)
        {
            if (Parent == null)
            {
                try
                {
                    CommitTransactionInternal(transactionScope);
                }
                catch (InvalidOperationException)
                {
                    Rollback();
                }
            }
            else
            {
                CommitTransactionInternal(transactionScope);
            }
        }

        private void CommitTransactionInternal(TransactionScope transactionScope)
        {
            using (transactionScope)
            {
                Debug.WriteLine(
                    "EfUnitOfWork: {0}; Type: {1}; Origin Thread: {2}; Commit Thread: {3}; Transaction: {4}",
                    Id,
                    ThreadSafeTransaction.GetType(),
                    ScopeThread.ManagedThreadId,
                    Thread.CurrentThread.ManagedThreadId,
                    Transaction.Current.TransactionInformation.LocalIdentifier);

                foreach (var context in ContextContainer.Contexts)
                {
                    context.SaveChanges();
                }

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Rolls back each context within the unit of work.
        /// </summary>
        /// <remarks></remarks>
        public override void Rollback()
        {
            // TODO: (DG) Should we be re-loading each enity marked as changed?
            Debug.WriteLine(String.Format("EfUoW: {0} is rolling back.", Id));
        }

        #endregion

        #region Implementation of IEfUnitOfWork

        /// <summary>
        /// Gets the context container.
        /// </summary>
        /// <remarks></remarks>
        public IContextContainer ContextContainer
        {
            get
            {
                return _ContextContianer;
            }
        }

        /// <summary>
        /// Validates each context in the container.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public IEnumerable<DbEntityValidationResult> Validate()
        {
            return ContextContainer.Contexts.SelectMany(c => c.GetValidationErrors());
        }

        #endregion

        #region Implementation of IDisposable

        protected override void DisposeManagedResources()
        {
            ContextContainer.Contexts.ForEach(c => c.Dispose());
            IsDisposed = true;

            Debug.WriteLine(String.Format("EfUnitOfWork: {0} disposed.", Id));
        }

        #endregion
    }
}