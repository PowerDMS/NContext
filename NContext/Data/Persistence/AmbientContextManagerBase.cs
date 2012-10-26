// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmbientContextManagerBase.cs" company="Waking Venture, Inc.">
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
    using System.Collections.Generic;
    using System.Transactions;

    /// <summary>
    /// Defines an abstraction for managing the ambient-context lifespan for units of work.
    /// </summary>
    /// <remarks></remarks>
    public abstract class AmbientContextManagerBase
    {
        /// <summary>
        /// Gets the ambient.
        /// </summary>
        /// <value>The ambient.</value>
        public virtual AmbientUnitOfWorkDecorator Ambient
        {
            get
            {
                return AmbientExists ? AmbientUnitsOfWork.Peek() : null;
            }
        }

        /// <summary>
        /// Gets whether the ambient unit of work is valid.
        /// </summary>
        /// <value>The ambient unit of work is valid.</value>
        public virtual Boolean AmbientUnitOfWorkIsValid
        {
            get
            {
                return AmbientExists && AmbientUnitsOfWork.Peek().UnitOfWork != null;
            }
        }

        /// <summary>
        /// Gets whether the ambient context exists.
        /// </summary>
        public abstract Boolean AmbientExists { get; }

        /// <summary>
        /// Gets whether the <see cref="AmbientContextManagerBase"/> instance supports concurrency. This is 
        /// required if you set <see cref="PersistenceOptions.MaxDegreeOfParallelism"/> greater than one.
        /// </summary>
        protected internal abstract Boolean IsThreadSafe { get; }

        /// <summary>
        /// Gets the ambient units of work.
        /// </summary>
        protected internal abstract Stack<AmbientUnitOfWorkDecorator> AmbientUnitsOfWork { get; }

        /// <summary>
        /// Adds the unit of work to the stack; thus making it the new ambient context.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public virtual void AddUnitOfWork(UnitOfWorkBase unitOfWork)
        {
            AmbientUnitsOfWork.Push(new AmbientUnitOfWorkDecorator(unitOfWork));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="unitOfWork"/> can be committed.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns><c>true</c> if the specified unit of work can be committed; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// <para>
        /// Group 1 (if the <paramref name="unitOfWork"/> is <c>not</c> part of a <see cref="CompositeUnitOfWork"/>)
        ///     <paramref name="unitOfWork"/>.Parent is NULL
        ///         <c>true</c> if the <paramref name="unitOfWork"/> does not belong to a <see cref="CompositeUnitOfWork"/>; otherwise <c>false</c>
        ///     AmbientExists -
        ///         <c>true</c> if an ambient exists within a given scope; otherwise <c>false</c>
        ///         (ie. per-thread <see cref="PerThreadAmbientContextManager"/> or per-request <see cref="PerRequestAmbientContextManager"/>)
        ///     Ambient.IsCommittable
        ///         true if there is only a single active session on the ambient context; otherwise, false
        /// </para>
        /// <para>
        /// Group 2 (if the <paramref name="unitOfWork"/> belongs to a <see cref="CompositeUnitOfWork"/> - ie. has a parent)
        ///     Ambient.UnitOfWork.IsCommitting
        ///         <c>true</c> if the ambient unit of work is currently being committed; otherwise <c>false</c>
        /// </para>
        /// </remarks>
        public virtual Boolean CanCommitUnitOfWork(UnitOfWorkBase unitOfWork)
        {
            return (unitOfWork.Parent == null &&
                       (AmbientExists &&
                        Ambient.Equals(unitOfWork) &&
                        Ambient.IsCommittable)) || 
                   Ambient.UnitOfWork.Status == TransactionStatus.Active;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="unitOfWork"/> can be disposed.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// <c>true</c> if no <see cref="AmbientExists"/>; otherwise: 
        /// <c>true</c> if the <see cref="AmbientUnitOfWorkDecorator.IsDisposable"/> and <paramref name="unitOfWork.Parent"/> is null; otherwise <c>false</c>
        /// </returns>
        /// <remarks></remarks>
        public virtual Boolean CanDisposeUnitOfWork(UnitOfWorkBase unitOfWork)
        {
            if (AmbientExists)
            {
                // Save the current disposable state of the ambient unit of work locally since further execution may affect it.
                var ambientIsDisposable = Ambient.IsDisposable;

                if (Ambient.Equals(unitOfWork))
                {
                    if (ambientIsDisposable)
                    {
                        AmbientUnitsOfWork.Pop();
                    }
                    else
                    {
                        Ambient.Decrement();
                    }
                }

                if ((unitOfWork.Status == TransactionStatus.Committed || unitOfWork.Status == TransactionStatus.Aborted) || 
                    (ambientIsDisposable && unitOfWork.Parent == null))
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Increments the active session count on the ambient unit of work.
        /// </summary>
        /// <remarks></remarks>
        public virtual void RetainAmbient()
        {
            Ambient.Increment();
        }
    }
}