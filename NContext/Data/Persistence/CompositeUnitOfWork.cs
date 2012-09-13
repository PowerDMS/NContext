// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeUnitOfWork.cs" company="Waking Venture, Inc.">
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;

    using NContext.Dto;
    using NContext.ErrorHandling.Errors;

    public class CompositeUnitOfWork : UnitOfWorkBase
    {
        #region Fields

        private readonly HashSet<UnitOfWorkBase> _UnitsOfWork = new HashSet<UnitOfWorkBase>();

        #endregion

        #region Constructors

        public CompositeUnitOfWork(AmbientContextManagerBase ambientContextManager)
            : this(ambientContextManager, () => new CommittableTransaction(), null, null)
        {
        }

        public CompositeUnitOfWork(AmbientContextManagerBase ambientContextManager, PersistenceOptions persistenceOptions)
            : this(ambientContextManager, () => new CommittableTransaction(), null, persistenceOptions)
        {
        }

        public CompositeUnitOfWork(AmbientContextManagerBase ambientContextManager, UnitOfWorkBase parent, PersistenceOptions persistenceOptions)
            : this(ambientContextManager, () => parent.ScopeTransaction, parent, persistenceOptions)
        {
        }

        private CompositeUnitOfWork(AmbientContextManagerBase ambientContextManager, Func<Transaction> transactionFactory, UnitOfWorkBase parent, PersistenceOptions persistenceOptions)
            : base(ambientContextManager, transactionFactory, parent, persistenceOptions)
        {
            Debug.WriteLine(String.Format("CompositeUnitOfWork: {0} created.", Id));
        }

        #endregion

        protected HashSet<UnitOfWorkBase> UnitsOfWork
        {
            get
            {
                return _UnitsOfWork;
            }
        }

        /// <summary>
        /// Adds the unit of work.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <remarks></remarks>
        public void AddUnitOfWork(UnitOfWorkBase unitOfWork)
        {
            if (UnitsOfWork.Any(uow => uow.Id == unitOfWork.Id))
            {
                return;
            }

            UnitsOfWork.Add(unitOfWork);
        }

        #region Overrides of UnitOfWorkBase

        protected override IResponseTransferObject<Boolean> CommitTransaction(TransactionScope transactionScope)
        {
            using (transactionScope)
            {
#if DEBUG
                // TODO: (DG) Remove this block
                if (Parent == null)
                {
                    Debug.WriteLine(String.Format("-----  Transaction: {0}  -----", Transaction.Current.TransactionInformation.LocalIdentifier));
                }
#endif

                var commitExceptions = new ConcurrentQueue<Exception>();

                Parallel.ForEach(
                    UnitsOfWork,
                    new ParallelOptions
                        {
                            MaxDegreeOfParallelism = PersistenceOptions.MaxDegreeOfParallelism
                        },
                    (unitOfWork, state) =>
                        {
                            try
                            {
                                if (state.IsExceptional || state.ShouldExitCurrentIteration) return;

                                unitOfWork.Commit()
                                    .Catch(errors =>
                                        {
                                            state.Break();
                                            commitExceptions.Enqueue(
                                                new AggregateException(
                                                    errors.Select(error => new Exception(error.Messages.ToString()))));
                                        });
                            }
                            catch (Exception exception)
                            {
                                state.Break();
                                commitExceptions.Enqueue(exception);

                                // TODO: (DG) Logging ...
                            }
                        });

                if (!commitExceptions.Any())
                {
                    Debug.WriteLine(
                        "CompositeUnitOfWork: {0}; Type: {1}; Origin Thread: {2}; Commit Thread: {3}; Transaction: {4}",
                        Id,
                        ThreadSafeTransaction.GetType(),
                        ScopeThread.ManagedThreadId,
                        System.Threading.Thread.CurrentThread.ManagedThreadId,
                        Transaction.Current.TransactionInformation.LocalIdentifier);

                    transactionScope.Complete();

#if DEBUG
                    if (Parent == null) Debug.WriteLine("----- Transaction End -----");
#endif

                    return new ServiceResponse<Boolean>(true);
                }

                Rollback();

#if DEBUG
                if (Parent == null) Debug.WriteLine("----- Transaction End -----");
#endif

                //throw new AggregateException(commitExceptions); // TODO: (DG) NContext Exception vs ErrorHandling
                return
                    new ServiceResponse<Boolean>(
                        NContextPersistenceError.CommitFailed(
                            Id,
                            Transaction.Current.TransactionInformation.LocalIdentifier,
                            new AggregateException(commitExceptions)));
            }
        }

        /// <summary>
        /// Rollback the transaction (if applicable).
        /// </summary>
        public override void Rollback()
        {
            if (Parent == null)
                Debug.WriteLine(String.Format("----- Transaction: {0} is Rolling Back -----", Transaction.Current.TransactionInformation.LocalIdentifier));
            else
                Debug.WriteLine(String.Format("CompositeUoW: {0} is rolling back.", Id));

            UnitsOfWork.AsParallel()
                       .WithDegreeOfParallelism(PersistenceOptions.MaxDegreeOfParallelism)
                       .ForAll(uow => uow.Rollback());
        }

        protected override void DisposeManagedResources()
        {
            UnitsOfWork.AsParallel()
                       .WithDegreeOfParallelism(PersistenceOptions.MaxDegreeOfParallelism)
                       .ForAll(uow => uow.Dispose());

            Debug.WriteLine(String.Format("CompositeUnitOfWork: {0} disposed.", Id));
        }

        #endregion
    }
}