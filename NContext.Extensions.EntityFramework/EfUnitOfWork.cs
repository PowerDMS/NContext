// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfUnitOfWork.cs" company="Waking Venture, Inc.">
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
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Transactions;

    using NContext.Common;
    using NContext.Data.Persistence;
    using NContext.ErrorHandling.Errors;

    /// <summary>
    /// Defines an Entity Framework 5 implementation of <see cref="UnitOfWorkBase"/>.
    /// </summary>
    public class EfUnitOfWork : UnitOfWorkBase, IEfUnitOfWork
    {
        private readonly IDbContextContainer _DbContextContianer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        /// <param name="ambientContextManager">The transaction manager.</param>
        /// <param name="dbContextContainer">The context container.</param>
        /// <remarks></remarks>
        protected internal EfUnitOfWork(AmbientContextManagerBase ambientContextManager, IDbContextContainer dbContextContainer)
            : this(ambientContextManager, dbContextContainer, () => new CommittableTransaction(), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        /// <param name="ambientContextManager">The transaction manager.</param>
        /// <param name="dbContextContainer">The context container.</param>
        /// <param name="parent">The parent.</param>
        /// <remarks></remarks>
        protected internal EfUnitOfWork(AmbientContextManagerBase ambientContextManager, IDbContextContainer dbContextContainer, UnitOfWorkBase parent)
            : this(ambientContextManager, dbContextContainer, () => parent.ScopeTransaction, parent)
        {
        }

        private EfUnitOfWork(AmbientContextManagerBase ambientContextManager, IDbContextContainer dbContextContainer, Func<Transaction> transactionFactory, UnitOfWorkBase parent)
            : base(ambientContextManager, transactionFactory, parent)
        {
            if (dbContextContainer == null)
            {
                throw new ArgumentNullException("dbContextContainer");
            }

            _DbContextContianer = dbContextContainer;

            Debug.WriteLine(String.Format("EfUnitOfWork: {0} created.", Id));
        }

        #region Overrides of UnitOfWorkBase

        /// <summary>
        /// Commits the changes within the persistence context.
        /// </summary>
        /// <param name="transactionScope">The transaction scope.</param>
        /// <returns>IResponseTransferObject{Boolean}.</returns>
        protected override IResponseTransferObject<Boolean> CommitTransaction(TransactionScope transactionScope)
        {
            if (Parent == null)
            {
                using (transactionScope)
                {
                    try
                    {
                        return CommitTransactionInternal().Let(_ => transactionScope.Complete());
                    }
                    catch (Exception exception)
                    {
                        Rollback();

                        // TODO: (DG) NContext Exception vs ErrorHandling
                        // throw new AggregateException(commitExceptions);
                        return
                            new ServiceResponse<Boolean>(
                                NContextPersistenceError.CommitFailed(
                                    Id,
                                    Transaction.Current.TransactionInformation.LocalIdentifier,
                                    new AggregateException(exception)));
                    }
                }
            }

            using (transactionScope)
            {
                return CommitTransactionInternal().Let(_ => transactionScope.Complete());
            }
        }

        /// <summary>
        /// Rolls-back each DbContext within the unit of work.
        /// </summary>
        /// <remarks>
        /// This does not dispose the contexts. Instead we leave that for when the instance gets disposed.
        /// </remarks>
        public override void Rollback()
        {
            Debug.WriteLine(String.Format("EfUoW: {0} is rolling back.", Id));
        }

        private IResponseTransferObject<Boolean> CommitTransactionInternal()
        {
            Debug.WriteLine(
                    "EfUnitOfWork: {0}; Type: {1}; Origin Thread: {2}; Commit Thread: {3}; Transaction LocalId: {4}; Transaction GlobalId: {5}",
                    Id,
                    ThreadSafeTransaction.GetType(),
                    ScopeThread.ManagedThreadId,
                    Thread.CurrentThread.ManagedThreadId,
                    Transaction.Current.TransactionInformation.LocalIdentifier,
                    Transaction.Current.TransactionInformation.DistributedIdentifier);

            foreach (var context in DbContextContainer.Contexts)
            {
                context.SaveChanges();
            }

            return new ServiceResponse<Boolean>(true);
        }

        #endregion

        #region Implementation of IEfUnitOfWork

        /// <summary>
        /// Gets the context container.
        /// </summary>
        /// <remarks></remarks>
        public IDbContextContainer DbContextContainer
        {
            get
            {
                return _DbContextContianer;
            }
        }

        /// <summary>
        /// Validates each context in the container.
        /// </summary>
        /// <returns>IEnumerable{DbEntityValidationResult}.</returns>
        public IEnumerable<DbEntityValidationResult> Validate()
        {
            return DbContextContainer.Contexts.SelectMany(c => c.GetValidationErrors());
        }

        #endregion

        #region Implementation of IDisposable

        protected override void DisposeManagedResources()
        {
            DbContextContainer.Dispose();
            
            Debug.WriteLine(String.Format("EfUnitOfWork: {0} disposed.", Id));
        }

        #endregion
    }
}