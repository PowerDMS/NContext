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
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;

    using Microsoft.FSharp.Core;

    using NContext.Common;
    using NContext.ErrorHandling.Errors;
    using NContext.Extensions;

    /// <summary>
    /// Defines a persistence-agnostic unit of work container for transactional polyglot persistence 
    /// support. All units of work must simply work against <see cref="System.Transactions"/>.
    /// </summary>
    public class CompositeUnitOfWork : UnitOfWorkBase
    {
        private readonly PersistenceOptions _PersistenceOptions;

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
            : base(ambientContextManager, parent, persistenceOptions.TransactionOptions)
        {
            _PersistenceOptions = persistenceOptions;
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
            UnitsOfWork.AsParallel()
                       .WithDegreeOfParallelism(_PersistenceOptions.MaxDegreeOfParallelism)
                       .ForAll(uow => uow.Rollback());
        }

        protected override IResponseTransferObject<Unit> CommitTransaction(TransactionScope transactionScope)
        {
            using (transactionScope)
            {
                return (_PersistenceOptions.MaxDegreeOfParallelism == 1 ? CommitChildren() : CommitChildrenParallel())
                    .Catch(errors =>
                        {
                            Rollback();
                            transactionScope.Dispose();
                            // throw new AggregateException(commitExceptions); // TODO: (DG) NContext Exception vs ErrorHandling Support
                        })
                    .Let(_ =>
                        {
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
            if (!AmbientContextManager.IsThreadSafe)
            {
                return NContextPersistenceError.ConcurrencyUnsupported(AmbientContextManager.GetType()).ToServiceResponse();
            }

            var commitExceptions = new ConcurrentQueue<Error>();

            Parallel.ForEach(
                    UnitsOfWork,
                    new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _PersistenceOptions.MaxDegreeOfParallelism
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
                       .WithDegreeOfParallelism(_PersistenceOptions.MaxDegreeOfParallelism)
                       .ForAll(uow => uow.Dispose());
        }
    }
}