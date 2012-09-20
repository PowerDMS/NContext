// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonAtomicUnitOfWork.cs" company="Waking Venture, Inc.">
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
    using System.Diagnostics;
    using System.Transactions;

    using NContext.Common;

    internal sealed class NonAtomicUnitOfWork : UnitOfWorkBase
    {
        public NonAtomicUnitOfWork(AmbientContextManagerBase ambientContextManager)
            : base(ambientContextManager)
        {
            Debug.WriteLine(String.Format("NonAtomicUnitOfWork: {0} created.", Id));
        }

        #region Overrides of UnitOfWorkBase

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        protected override IResponseTransferObject<Boolean> CommitTransaction(TransactionScope transactionScope)
        {
            return new ServiceResponse<Boolean>(true);
        }

        /// <summary>
        /// Rollback the transaction (if applicable).
        /// </summary>
        public override void Rollback()
        {
        }

        protected override void Dispose(Boolean disposeManagedResources)
        {
            IsCommitted = true;

            base.Dispose(disposeManagedResources);
        }

        protected override void DisposeManagedResources()
        {
            Debug.WriteLine(String.Format("NonAtomicUnitOfWork: {0} disposed.", Id));
        }

        #endregion
    }
}