// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RavenDBPersistenceFactory.cs">
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
//
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace NContext.Extensions.RavenDB
{
    using System.Transactions;

    using NContext.Data;

    /// <summary>
    /// 
    /// </summary>
    public class RavenDBPersistenceFactory : PersistenceFactoryBase
    {
        #region Overrides of PersistenceFactoryBase

        public override UnitOfWorkBase CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            UnitOfWorkBase unitOfWork;
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                    if (TransactionManager.AmbientExists && TransactionManager.AmbientIsValid)
                    {
                        if (TransactionManager.Current.IsTypeOf<CompositeUnitOfWork>())
                        {
                            var currentCompositeUnitOfWork = ((CompositeUnitOfWork)TransactionManager.Current.UnitOfWork);
                            unitOfWork = new EfUnitOfWork(TransactionManager, new ContextContainer(), () => currentCompositeUnitOfWork.ScopeTransaction, currentCompositeUnitOfWork);
                            currentCompositeUnitOfWork.AddUnitOfWork(unitOfWork);
                            TransactionManager.AddUnitOfWork(unitOfWork);
                        }
                        else
                        {
                            unitOfWork = TransactionManager.Current.UnitOfWork;
                            TransactionManager.Retain();
                        }
                    }
                    else
                    {
                        unitOfWork = new RavenUnitOfWork(TransactionManager, new ContextContainer(), () => new CommittableTransaction());
                        TransactionManager.AddUnitOfWork(unitOfWork);
                    }

                    return unitOfWork;
                case TransactionScopeOption.RequiresNew:
                    unitOfWork = new RavenUnitOfWork(TransactionManager, new ContextContainer(), () => new CommittableTransaction());
                    TransactionManager.AddUnitOfWork(unitOfWork);

                    return unitOfWork;
                case TransactionScopeOption.Suppress:
                    unitOfWork = new NonAtomicUnitOfWork(TransactionManager);
                    TransactionManager.AddUnitOfWork(null);

                    return unitOfWork;
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        #endregion
    }
}