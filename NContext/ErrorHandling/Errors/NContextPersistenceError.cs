// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NContextPersistenceError.cs" company="Waking Venture, Inc.">
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

namespace NContext.ErrorHandling.Errors
{
    using System;
    using System.Net;
    using System.Transactions;

    using NContext.Common;
    using NContext.Data.Persistence;
    using NContext.Extensions;

    public class NContextPersistenceError : ErrorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBase"/> class.
        /// </summary>
        /// <param name="localizationKey">The localization key.</param>
        /// <param name="errorMessageParameters">The error message parameters.</param>
        /// <remarks></remarks>
        private NContextPersistenceError(String localizationKey, params Object[] errorMessageParameters)
            : base(localizationKey, HttpStatusCode.InternalServerError, errorMessageParameters)
        {
        }

        /// <summary>
        /// ScopeTransaction cannot be null. This is due to an invalid "Func&lt;Transaction&gt; transactionFactory"
        /// parameter being supplied to the UnitOfWorkBase constructor.
        /// </summary>
        /// <returns>NContextPersistenceError.</returns>
        /// <remarks>
        ///   <para>
        /// ScopeTransaction is used for two things:
        /// 1. If IUnitOfWork.Commit() is called on the same thread with which the IUnitOfWork instance was created, then
        /// ScopeTransaction is the System.Transactions.CommittableTransaction that will be used; and
        /// 2. If IUnitOfWork.Commit() is called on a different thread from which the IUnitOfWork instance was created, then
        /// ScopeTransaction is used to create a System.Transactions.DependentTransaction - which then is used to commit
        /// all transactional work in a thread-safe manner.
        ///   </para>
        /// </remarks>
        public static NContextPersistenceError ScopeTransactionIsNull()
        {
            return new NContextPersistenceError("ScopeTransactionIsNull");
        }

        /// <summary>
        /// IUnitOfWork instance Id: {0} with transaction Id: {1} has failed to commit due to the following exception(s): {2}
        /// </summary>
        /// <param name="unitOfWorkId">The unit of work id.</param>
        /// <param name="transactionIdentifier">The transaction identifier.</param>
        /// <param name="exceptions">The exceptions.</param>
        /// <returns>NContextPersistenceError.</returns>
        public static NContextPersistenceError CommitFailed(Guid unitOfWorkId, String transactionIdentifier, AggregateException exceptions = null)
        {
            return new NContextPersistenceError("CommitFailed", unitOfWorkId, transactionIdentifier, exceptions != null ? exceptions.ToString() : String.Empty);
        }

        /// <summary>
        /// IUnitOfWork instance Id: {0} with transaction Id: {1} has failed to commit due to the following exception(s): {2}
        /// </summary>
        /// <param name="unitOfWorkId">The unit of work id.</param>
        /// <param name="transactionIdentifier">The transaction identifier.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>NContextPersistenceError.</returns>
        public static NContextPersistenceError CommitFailed(Guid unitOfWorkId, String transactionIdentifier, TransactionAbortedException exception)
        {
            return new NContextPersistenceError("CommitFailed", unitOfWorkId, transactionIdentifier, exception.ToErrors().ToMessage());
        }

        /// <summary>
        /// IUnitOfWork instance id: {0} cannot be committed.
        /// </summary>
        /// <param name="unitOfWorkId">The unit of work id.</param>
        /// <returns>NContextPersistenceError.</returns>
        public static NContextPersistenceError UnitOfWorkNonCommittable(Guid unitOfWorkId)
        {
            return new NContextPersistenceError("UnitOfWorkNonCommittable", unitOfWorkId);
        }

        /// <summary>
        /// The active ambient context manager of type {0} does not support parallel commits per transaction.
        /// To use this manager, PersistenceOptions.MaxDegreeOfParallelism cannot be more than one.
        /// </summary>
        /// <param name="ambientContextManagerType">The type of the <see cref="AmbientContextManagerBase"/>.</param>
        /// <returns>NContextPersistenceError.</returns>
        public static NContextPersistenceError ConcurrencyUnsupported(Type ambientContextManagerType)
        {
            return new NContextPersistenceError("ConcurrencyUnsupported", ambientContextManagerType.Name);
        }
    }
}