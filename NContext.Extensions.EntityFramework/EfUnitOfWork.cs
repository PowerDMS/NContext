// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfUnitOfWork.cs">
//   Copyright (c) 2012
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
//   Defines an Entity Framework 4 implementation of IEfUnitOfWork pattern.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;

using NContext.Data;

namespace NContext.Extensions.EntityFramework
{
    /// <summary>
    /// Defines an Entity Framework 4 implementation of IEfUnitOfWork.
    /// </summary>
    public class EfUnitOfWork : UnitOfWorkBase, IEfUnitOfWork
    {
        #region Fields

        private readonly IContextContainer _ContextContianer;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        /// <param name="contextContianer">The context contianer.</param>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <remarks></remarks>
        protected internal EfUnitOfWork(IContextContainer contextContianer, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
            : base(transactionScopeOption)
        {
            _ContextContianer = contextContianer;
        }

        #endregion

        #region Overrides of UnitOfWorkBase

        /// <summary>
        /// Commits the changes within the persistence context.
        /// </summary>
        /// <remarks></remarks>
        protected override void CommitChanges()
        {
            foreach (var context in ContextContainer.Contexts)
            {
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Rolls back each context within the unit of work.
        /// </summary>
        /// <remarks></remarks>
        protected override void Rollback()
        {
            foreach (var context in ContextContainer.Contexts)
            {
                context.ChangeTracker
                    .Entries()
                    .Where(e => e != null && e.State != EntityState.Unchanged)
                    .Select(e => e.Entity)
                    .ForEach(e => context.Entry(e).Reload());
            }
        }

        #endregion

        #region Implementation of IEfUnitOfWork

        /// <summary>
        /// Gets the context container.
        /// </summary>
        /// <remarks></remarks>
        public IContextContainer ContextContainer
        {
            get
            {
                return _ContextContianer;
            }
        }

        /// <summary>
        /// Validates each context in the container.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public IEnumerable<DbEntityValidationResult> Validate()
        {
            return ContextContainer.Contexts.SelectMany(c => c.GetValidationErrors());
        }

        #endregion

        #region Implementation of IDisposable

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
                if (EfUnitOfWorkController.DisposeUnitOfWork())
                {
                    ContextContainer.Contexts.ForEach(c => c.Dispose());

                    IsDisposed = true;
                }
            }
        }

        #endregion
    }
}