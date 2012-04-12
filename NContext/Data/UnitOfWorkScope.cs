// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkScope.cs">
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
using System.Linq;

using NContext.Extensions;

namespace NContext.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitOfWorkScope : UnitOfWorkBase
    {
        private readonly HashSet<IUnitOfWork> _UnitsOfWork = new HashSet<IUnitOfWork>();

        private volatile Boolean _IsDisposing;

        protected internal Boolean IsDisposing
        {
            get
            {
                return _IsDisposing;
            }
            protected set
            {
                _IsDisposing = value;
            }
        }
        
        protected HashSet<IUnitOfWork> UnitsOfWork
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
        public void AddUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (UnitsOfWork.Any(uow => uow.Id == unitOfWork.Id))
            {
                return;
            }

            UnitsOfWork.Add(unitOfWork);
        }

        #region Overrides of UnitOfWorkBase

        /// <summary>
        /// Commits the changes within the persistence context.
        /// </summary>
        /// <remarks></remarks>
        protected override void CommitChanges()
        {
            UnitsOfWork.ForEach(uow => uow.Commit());
        }

        /// <summary>
        /// Rollback the transaction (if applicable).
        /// </summary>
        protected override void Rollback()
        {
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposeManagedResources)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
                IsDisposing = true;

                UnitsOfWork.ForEach(uow => uow.Dispose());

                IsDisposing = false;
                IsDisposed = true;
            }
        }

        #endregion
    }
}