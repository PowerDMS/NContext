namespace NContext.Data.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Defines a transaction lifetime-management abstraction for data persistence.
    /// </summary>
    /// <remarks></remarks>
    public abstract class AmbientTransactionManagerBase
    {
        public virtual AmbientUnitOfWork Ambient
        {
            get
            {
                return AmbientExists ? AmbientUnitsOfWork.Peek() : null;
            }
        }

        public virtual Boolean AmbientIsValid
        {
            get
            {
                return AmbientExists && AmbientUnitsOfWork.Peek().UnitOfWork != null;
            }
        }

        public abstract Boolean AmbientExists { get; }

        protected abstract Stack<AmbientUnitOfWork> AmbientUnitsOfWork { get; }

        /// <summary>
        /// Adds the unit of work.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <remarks></remarks>
        public virtual void AddUnitOfWork(UnitOfWorkBase unitOfWork)
        {
            AmbientUnitsOfWork.Push(new AmbientUnitOfWork(unitOfWork));
            Debug.WriteLine("Ambient Pushed");
        }

        public virtual Boolean CanCommitUnitOfWork(UnitOfWorkBase unitOfWork)
        {
            if (unitOfWork.ScopeThread == Thread.CurrentThread)
            {
                return (AmbientExists && Ambient.Equals(unitOfWork) && Ambient.IsCommittable && unitOfWork.Parent == null) || Ambient.UnitOfWork.IsCommitted;
            }

            return true;
        }

        public virtual Boolean CanDisposeUnitOfWork(UnitOfWorkBase unitOfWork)
        {
            if (AmbientExists)
            {
                var isDisposable = Ambient.IsDisposable;

                if (unitOfWork.IsCommitted || (Ambient.Equals(unitOfWork) && isDisposable))
                {
                    AmbientUnitsOfWork.Pop();
                    Debug.WriteLine("Ambient Popped");
                }
                else if (!isDisposable && unitOfWork.Parent != null)
                {
                    Ambient.Decrement();
                    Debug.WriteLine("Ambient Decremented");
                }

                if (isDisposable && unitOfWork.Parent == null)
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Augments this instance.
        /// </summary>
        /// <remarks></remarks>
        public virtual void RetainAmbient()
        {
            Ambient.Increment();
            Debug.WriteLine("Ambient Incremented");
        }
    }
}