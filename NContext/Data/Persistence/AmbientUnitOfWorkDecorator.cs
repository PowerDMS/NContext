// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmbientUnitOfWorkDecorator.cs" company="Waking Venture, Inc.">
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

    using NContext.Extensions;

    /// <summary>
    /// Defines a unit of work in active use and keeps track of the number of sessions associated with it.
    /// </summary>
    /// <remarks></remarks>
    public class AmbientUnitOfWorkDecorator : IEquatable<UnitOfWorkBase>
    {
        #region Fields

        private readonly UnitOfWorkBase _UnitOfWork;

        private Int32 _ActiveSessions;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbientUnitOfWorkDecorator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <remarks></remarks>
        public AmbientUnitOfWorkDecorator(UnitOfWorkBase unitOfWork)
        {
            _ActiveSessions = 1;
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
                return _ActiveSessions == 1;
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
                return _ActiveSessions <= 1;
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
            _ActiveSessions--;
        }

        /// <summary>
        /// Increments the active sessions for this <seealso cref="UnitOfWork"/> instance.
        /// </summary>
        /// <remarks></remarks>
        protected internal void Increment()
        {
            _ActiveSessions++;
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