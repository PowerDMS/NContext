namespace NContext.Data.Persistence
{
    using System;
    using System.Transactions;

    using Microsoft.FSharp.Core;

    using NContext.Common;

    internal sealed class NonAtomicUnitOfWork : UnitOfWorkBase
    {
        public NonAtomicUnitOfWork(AmbientContextManagerBase ambientContextManager)
            : base(ambientContextManager)
        {
        }

        #region Overrides of UnitOfWorkBase

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        protected override IServiceResponse<Unit> CommitTransaction(TransactionScope transactionScope)
        {
            return new DataResponse<Unit>(default(Unit));
        }

        /// <summary>
        /// Rollback the transaction (if applicable).
        /// </summary>
        public override void Rollback()
        {
        }

        protected override void Dispose(Boolean disposeManagedResources)
        {
            Status = TransactionStatus.Committed;

            base.Dispose(disposeManagedResources);
        }

        protected override void DisposeManagedResources()
        {
        }

        #endregion
    }
}