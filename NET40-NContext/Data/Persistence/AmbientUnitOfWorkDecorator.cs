namespace NContext.Data.Persistence
{
    using System;

    using NContext.Extensions;

    /// <summary>
    /// Defines a unit of work in active use and keeps track of the number of sessions associated with it.
    /// </summary>
    /// <remarks></remarks>
    public class AmbientUnitOfWorkDecorator : IEquatable<UnitOfWorkBase>
    {
        #region Fields

        private readonly UnitOfWorkBase _UnitOfWork;

        private Int32 _SessionCount;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbientUnitOfWorkDecorator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <remarks></remarks>
        public AmbientUnitOfWorkDecorator(UnitOfWorkBase unitOfWork)
        {
            _SessionCount = 1;
            _UnitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <remarks></remarks>
        public UnitOfWorkBase UnitOfWork
        {
            get
            {
                return _UnitOfWork;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is committable. Returns true if there is only a single active session for the <see cref="UnitOfWork"/> instance.
        /// </summary>
        /// <returns><c>true</c> if there is only a single active session for the <see cref="UnitOfWork"/> instance; otherwise <c>false</c></returns>
        /// <remarks></remarks>
        internal Boolean IsCommittable
        {
            get
            {
                return SessionCount == 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposable. Returns true if there is at most, one active session for the <see cref="UnitOfWork"/> instance.
        /// </summary>
        /// <returns><c>true</c> if there is at most, one active session for the <see cref="UnitOfWork"/> instance; otherwise <c>false</c></returns>
        /// <remarks></remarks>
        internal Boolean IsDisposable
        {
            get
            {
                return SessionCount <= 1;
            }
        }

        internal Int32 SessionCount
        {
            get
            {
                return _SessionCount;
            }
        }

        public Boolean IsTypeOf<TUnitOfWork>() where TUnitOfWork : class, IUnitOfWork
        {
            return UnitOfWork != null && UnitOfWork.GetType().Implements(typeof(TUnitOfWork));
        }

        /// <summary>
        /// Decrements the active sessions for this <seealso cref="UnitOfWork"/> instance.
        /// </summary>
        /// <remarks></remarks>
        protected internal void Decrement()
        {
            _SessionCount = SessionCount - 1;
        }

        /// <summary>
        /// Increments the active sessions for this <seealso cref="UnitOfWork"/> instance.
        /// </summary>
        /// <remarks></remarks>
        protected internal void Increment()
        {
            _SessionCount = SessionCount + 1;
        }

        #region Implementation of IEquatable<IUnitOfWork>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current unit of work is equal to the <paramref name="unitOfWork"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="unitOfWork">An <see cref="UnitOfWorkBase"/> to compare with this object.</param>
        public Boolean Equals(UnitOfWorkBase unitOfWork)
        {
            return unitOfWork != null && UnitOfWork != null && UnitOfWork.Id.Equals(unitOfWork.Id);
        }

        #endregion
    }
}