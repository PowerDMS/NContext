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
//   Defines an Entity Framework 4 implementation of IUnitOfWork pattern.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;

namespace NContext.Extensions.EntityFramework
{
    /// <summary>
    /// Defines an Entity Framework 4 implementation of IUnitOfWork pattern.
    /// </summary>
    public class EfUnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly IContextContainer _ContextContianer;

        private readonly TransactionScopeOption _TransactionScopeOption;

        private Boolean _IsDisposed;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        /// <param name="contextContianer">The context contianer.</param>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <remarks></remarks>
        protected internal EfUnitOfWork(IContextContainer contextContianer, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            if (contextContianer == null)
            {
                throw new ArgumentNullException("contextContianer", "Context container cannot be null.");
            }

            _ContextContianer = contextContianer;
            _TransactionScopeOption = transactionScopeOption;

            UnitOfWorkController.AddUnitOfWork(this);
        }

        #endregion

        #region Implementation of IUnitOfWork

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
        /// Commits the changes in the context to the database.
        /// </summary>
        public void Commit()
        {
            using (var scope = new TransactionScope(_TransactionScopeOption))
            {
                try
                {
                    foreach (var context in ContextContainer.Contexts)
                    {
                        context.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    // TODO: (DG) Can we catch DbEntityValidationException? Where can we store this?
                    Rollback();
                }
                finally
                {
                    scope.Complete();
                }
            }
        }

        /// <summary>
        /// Rolls back each context within the unit of work.
        /// </summary>
        /// <remarks></remarks>
        public void Rollback()
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

        public IEnumerable<DbEntityValidationResult> Validate()
        {
            return ContextContainer.Contexts.SelectMany(c => c.GetValidationErrors());
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="EfUnitOfWork"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks></remarks>
        ~EfUnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(Boolean disposeManagedResources)
        {
            if (_IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
                if (UnitOfWorkController.DisposeUnitOfWork())
                {
                    ContextContainer.Contexts.ForEach(c => c.Dispose());
                    _IsDisposed = true;
                }
            }
        }

        #endregion
    }
}