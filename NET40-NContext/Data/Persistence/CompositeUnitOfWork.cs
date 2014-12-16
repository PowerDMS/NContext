namespace NContext.Data.Persistence
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
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
    public class CompositeUnitOfWork : UnitOfWorkBase, ICollection<UnitOfWorkBase>
    {
        private readonly PersistenceOptions _PersistenceOptions;

        private readonly ISet<UnitOfWorkBase> _UnitsOfWork = new HashSet<UnitOfWorkBase>();

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

        protected ISet<UnitOfWorkBase> UnitsOfWork
        {
            get
            {
                return _UnitsOfWork;
            }
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

        /// <summary>
        /// Determines whether this instance contains a specific type of <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public Boolean ContainsType<T>() where T : IUnitOfWork
        {
            return UnitsOfWork.Any(uow => uow.GetType().Implements<T>());
        }

        protected override IServiceResponse<Unit> CommitTransaction(TransactionScope transactionScope)
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

        private IServiceResponse<Unit> CommitChildren()
        {
            foreach (var unitOfWork in UnitsOfWork)
            {
                try
                {
                    Error error;
                    if ((error = unitOfWork.Commit().Error) != null)
                    {
                        return new ErrorResponse<Unit>(error);
                    }
                }
                catch (Exception exception)
                {
                    return new ErrorResponse<Unit>(exception.ToError());
                }
            }

            return new DataResponse<Unit>(default(Unit));
        }

        private IServiceResponse<Unit> CommitChildrenParallel()
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
                                    .Catch(error =>
                                        {
                                            state.Break();
                                            commitExceptions.Enqueue(error);
                                        });
                            }
                            catch (Exception exception)
                            {
                                state.Break();
                                commitExceptions.Enqueue(exception.ToError());

                                // TODO: (DG) Logging ...
                            }
                        });

            return commitExceptions.Any()
                       ? (IServiceResponse<Unit>)new ErrorResponse<Unit>(new AggregateError((Int32)HttpStatusCode.InternalServerError, GetType().Name, commitExceptions.Select(error => error)))
                       : (IServiceResponse<Unit>)new DataResponse<Unit>(default(Unit));
        }

        protected override void DisposeManagedResources()
        {
            UnitsOfWork.AsParallel()
                       .WithDegreeOfParallelism(_PersistenceOptions.MaxDegreeOfParallelism)
                       .ForAll(uow => uow.Dispose());
        }

        #region Implementation of ICollection

        public IEnumerator<UnitOfWorkBase> GetEnumerator()
        {
            return _UnitsOfWork.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(UnitOfWorkBase unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (UnitsOfWork.Contains(unitOfWork))
            {
                return;
            }

            UnitsOfWork.Add(unitOfWork);
        }

        public void Clear()
        {
            UnitsOfWork.Clear();
        }

        public Boolean Contains(UnitOfWorkBase unitOfWork)
        {
            return UnitsOfWork.Contains(unitOfWork);
        }

        public void CopyTo(UnitOfWorkBase[] array, Int32 arrayIndex)
        {
            UnitsOfWork.CopyTo(array, arrayIndex);
        }

        public Boolean Remove(UnitOfWorkBase unitOfWork)
        {
            return UnitsOfWork.Remove(unitOfWork);
        }

        public Int32 Count
        {
            get
            {
                return UnitsOfWork.Count;
            }
        }

        public Boolean IsReadOnly
        {
            get
            {
                return UnitsOfWork.IsReadOnly;
            }
        }

        #endregion
    }
}