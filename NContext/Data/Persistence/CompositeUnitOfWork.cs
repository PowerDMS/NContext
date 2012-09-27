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

    using Microsoft.FSharp.Core;

    using NContext.Common;
    using NContext.ErrorHandling.Errors;
    using NContext.Extensions;

    public class CompositeUnitOfWork : UnitOfWorkBase
    {
        private readonly HashSet<UnitOfWorkBase> _UnitsOfWork = new HashSet<UnitOfWorkBase>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeUnitOfWork" /> class.
        /// </summary>
        /// <param name="ambientContextManager">The ambient context manager.</param>
        public CompositeUnitOfWork(AmbientContextManagerBase ambientContextManager)
            : this(ambientContextManager, null, new PersistenceOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeUnitOfWork" /> class.
        /// </summary>
        /// <param name="ambientContextManager">The ambient context manager.</param>
        /// <param name="persistenceOptions">The persistence options.</param>
        public CompositeUnitOfWork(AmbientContextManagerBase ambientContextManager, PersistenceOptions persistenceOptions)
            : this(ambientContextManager, null, persistenceOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeUnitOfWork" /> class.
        /// </summary>
        /// <param name="ambientContextManager">The ambient context manager.</param>
        /// <param name="parent">The parent.</param>
        public CompositeUnitOfWork(AmbientContextManagerBase ambientContextManager, UnitOfWorkBase parent)
            : this(ambientContextManager, parent, new PersistenceOptions())
        {
        }

        protected internal CompositeUnitOfWork(AmbientContextManagerBase ambientContextManager, UnitOfWorkBase parent, PersistenceOptions persistenceOptions)
            : base(ambientContextManager, parent, persistenceOptions)
        {
            Debug.WriteLine(String.Format("CompositeUnitOfWork: {0} created.", Id));
        }

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

        /// <summary>
        /// Rollback the transaction (if applicable).
        /// </summary>
        public override void Rollback()
        {
            if (Parent == null)
                Debug.WriteLine(String.Format("----- Transaction: {0} is Rolling Back -----", Transaction.Current != null ? Transaction.Current.TransactionInformation.LocalIdentifier : String.Empty));
            else
                Debug.WriteLine(String.Format("CompositeUoW: {0} is rolling back.", Id));

            UnitsOfWork.AsParallel()
                       .WithDegreeOfParallelism(PersistenceOptions.MaxDegreeOfParallelism)
                       .ForAll(uow => uow.Rollback());
        }

        protected override IResponseTransferObject<Unit> CommitTransaction(TransactionScope transactionScope)
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
                return (PersistenceOptions.MaxDegreeOfParallelism == 1 ? CommitChildren() : CommitChildrenParallel())
                    .Catch(errors =>
                        {
                            Rollback();
                            transactionScope.Dispose();
                            // throw new AggregateException(commitExceptions); // TODO: (DG) NContext Exception vs ErrorHandling Support
                        })
                    .Let(_ =>
                        {
#if DEBUG
                            Console.WriteLine(
                                "CompositeUnitOfWork: {0}; Type: {1}; Origin Thread: {2}; Commit Thread: {3}; Transaction: {4}",
                                Id,
                                CommittableTransaction.GetType(),
                                ScopeThread.ManagedThreadId,
                                System.Threading.Thread.CurrentThread.ManagedThreadId,
                                Transaction.Current.TransactionInformation.LocalIdentifier);
                            
                            Console.WriteLine(
                                "Transaction {0} - Complete() is about to be called.",
                                Transaction.Current.TransactionInformation.LocalIdentifier);

                            if (Parent == null)
                                Debug.WriteLine("----- Transaction End -----");
#endif
                            transactionScope.Complete();
                        });
            }
        }

        private IResponseTransferObject<Unit> CommitChildren()
        {
            foreach (var unitOfWork in UnitsOfWork)
            {
                try
                {
                    IEnumerable<Error> errors;
                    if ((errors = unitOfWork.Commit().Errors).Any())
                    {
                        return new ServiceResponse<Unit>(errors);
                    }
                }
                catch (Exception exception)
                {
                    return new ServiceResponse<Unit>(exception.ToErrors());
                }
            }

            return new ServiceResponse<Unit>(default(Unit));
        }

        private IResponseTransferObject<Unit> CommitChildrenParallel()
        {
            if (!AmbientContextManager.SupportsConcurrency)
            {
                return NContextPersistenceError.ConcurrencyUnsupported(AmbientContextManager.GetType()).ToServiceResponse();
            }

            var commitExceptions = new ConcurrentQueue<Error>();

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
                                            errors.ForEach(commitExceptions.Enqueue);
                                        });
                            }
                            catch (Exception exception)
                            {
                                state.Break();
                                exception.ToErrors().ForEach(commitExceptions.Enqueue);

                                // TODO: (DG) Logging ...
                            }
                        });

            return commitExceptions.Any()
                       ? new ServiceResponse<Unit>(commitExceptions.Select(error => error))
                       : new ServiceResponse<Unit>(default(Unit));
        }

        protected override void DisposeManagedResources()
        {
            UnitsOfWork.AsParallel()
                       .WithDegreeOfParallelism(PersistenceOptions.MaxDegreeOfParallelism)
                       .ForAll(uow => uow.Dispose());

            Console.WriteLine("CompositeUnitOfWork: {0} disposed.", Id);
        }
    }
}