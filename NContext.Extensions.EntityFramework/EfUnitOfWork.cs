// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfUnitOfWork.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Transactions;

    using Microsoft.FSharp.Core;

    using NContext.Common;
    using NContext.Data.Persistence;
    using NContext.ErrorHandling.Errors;
    using NContext.Extensions;

    /// <summary>
    /// Defines an Entity Framework 5 implementation of <see cref="UnitOfWorkBase"/>.
    /// </summary>
    public class EfUnitOfWork : UnitOfWorkBase, IEfUnitOfWork
    {
        private readonly IDbContextContainer _DbContextContianer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        /// <param name="ambientContextManager">The transaction manager.</param>
        /// <param name="dbContextContainer">The context container.</param>
        /// <remarks></remarks>
        protected internal EfUnitOfWork(AmbientContextManagerBase ambientContextManager, IDbContextContainer dbContextContainer)
            : this(ambientContextManager, dbContextContainer, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        /// <param name="ambientContextManager">The transaction manager.</param>
        /// <param name="dbContextContainer">The context container.</param>
        /// <param name="parent">The parent.</param>
        /// <remarks></remarks>
        protected internal EfUnitOfWork(AmbientContextManagerBase ambientContextManager, IDbContextContainer dbContextContainer, UnitOfWorkBase parent)
            : base(ambientContextManager, parent)
        {
            if (dbContextContainer == null)
            {
                throw new ArgumentNullException("dbContextContainer");
            }

            _DbContextContianer = dbContextContainer;

            Debug.WriteLine(String.Format("EfUnitOfWork: {0} created.", Id));
        }

        #region Overrides of UnitOfWorkBase

        /// <summary>
        /// Commits the changes within the persistence context.
        /// </summary>
        /// <param name="transactionScope">The transaction scope.</param>
        /// <returns>IResponseTransferObject{Boolean}.</returns>
        protected override IResponseTransferObject<Unit> CommitTransaction(TransactionScope transactionScope)
        {
            if (Parent == null)
            {
                try
                {
                    using (transactionScope)
                    {
#if DEBUG
                        // TODO: (DG) Remove this block
                        Debug.WriteLine(String.Format("-----  Transaction: {0}  -----", Transaction.Current.TransactionInformation.LocalIdentifier));
#endif
                        return CommitTransactionInternal()
                                   .Catch(errors =>
                                       {
                                           // TODO: (DG) Support exceptions!
                                           // If NContext exception vs error handling IS Exception-based, don't rollback here; just throw;
                                           Rollback();
                                           // throw new NContextPersistenceException(errors);
#if DEBUG
                                           Console.WriteLine(String.Format("----- Transaction: {0} is Rolling Back -----", Transaction.Current != null ? Transaction.Current.TransactionInformation.LocalIdentifier : String.Empty));
#endif
                                       })
                                   .Let(_ =>
                                       {
#if DEBUG
                                           Console.WriteLine("Transaction {0} - Complete() is about to be called.", Transaction.Current.TransactionInformation.LocalIdentifier);
                                           Console.WriteLine("----- Transaction End -----");
#endif
                                           transactionScope.Complete();
                                       });
                    }
                }
                catch (Exception exception)
                {
                    Rollback();

                    // TODO: (DG) NContext Exception vs ErrorHandling
                    return ErrorBaseExtensions.ToServiceResponse(NContextPersistenceError.CommitFailed(Id, "tranId", new AggregateException(exception)));
                }
            }

            return CommitTransactionInternal();
        }

        /// <summary>
        /// Rolls-back each DbContext within the unit of work.
        /// </summary>
        /// <remarks>
        /// This does not dispose the contexts. Instead we leave that for when the instance gets disposed.
        /// </remarks>
        public override void Rollback()
        {
            Debug.WriteLine(String.Format("EfUoW: {0} is rolling back.", Id));
        }

        private IResponseTransferObject<Unit> CommitTransactionInternal()
        {
            Console.Write(
                new StringBuilder()
                    .AppendFormat("EfUnitOfWork: {0}", Id).AppendLine()
                    .AppendFormat("\tType: {0}", CurrentTransaction == null ? String.Empty : CurrentTransaction.GetType().ToString()).AppendLine()
                    .AppendFormat("\tScope Thread: {0}", ScopeThread.ManagedThreadId).AppendLine()
                    .AppendFormat("\tCommit Thread: {0}", Thread.CurrentThread.ManagedThreadId).AppendLine()
                    .AppendFormat("\tTransaction LocalId: {0}", Transaction.Current.TransactionInformation.LocalIdentifier).AppendLine()
                    .AppendFormat("\tTransaction GlobalId: {0}", Transaction.Current.TransactionInformation.DistributedIdentifier).AppendLine());

            foreach (var context in DbContextContainer.Contexts)
            {
                if (context.Configuration.ValidateOnSaveEnabled)
                {
                    try
                    {
                        context.SaveChanges();
                        continue;
                    }
                    catch (DbEntityValidationException exception)
                    {
                        return new EfServiceResponse<Unit>(exception.EntityValidationErrors);
                    }
                }

                context.SaveChanges();
            }

            return new ServiceResponse<Unit>(default(Unit));
        }

        #endregion

        #region Implementation of IEfUnitOfWork

        /// <summary>
        /// Gets the context container.
        /// </summary>
        /// <remarks></remarks>
        public IDbContextContainer DbContextContainer
        {
            get
            {
                return _DbContextContianer;
            }
        }

        /// <summary>
        /// Validates each context in the container.
        /// </summary>
        /// <returns>IEnumerable{DbEntityValidationResult}.</returns>
        public IEnumerable<DbEntityValidationResult> Validate()
        {
            return DbContextContainer.Contexts.SelectMany(c => c.GetValidationErrors());
        }

        #endregion

        #region Implementation of IDisposable

        protected override void DisposeManagedResources()
        {
            DbContextContainer.Dispose();
            
            Debug.WriteLine(String.Format("EfUnitOfWork: {0} disposed.", Id));
        }

        #endregion
    }
}