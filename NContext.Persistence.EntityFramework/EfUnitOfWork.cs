// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfUnitOfWork.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>

// <summary>
//   Defines an Entity Framework 4 implementation of IUnitOfWork pattern.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;

using NContext.Application.Extensions;

namespace NContext.Persistence.EntityFramework
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
        internal EfUnitOfWork(IContextContainer contextContianer, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
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