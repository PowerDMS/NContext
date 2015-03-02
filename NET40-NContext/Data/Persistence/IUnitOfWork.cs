namespace NContext.Data.Persistence
{
    using System;
    using System.Transactions;

    using Microsoft.FSharp.Core;

    using NContext.Common;

    /// <summary>
    /// Defines a contract for all unit of work implementations.
    /// </summary>
    /// <remarks></remarks>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the identity for the unit of work instance.
        /// </summary>
        /// <remarks></remarks>
        Guid Id { get; }

        /// <summary>
        /// Gets additional information about the transaction.
        /// </summary>
        /// <value>The transaction information.</value>
        TransactionInformation TransactionInformation { get; }

        /// <summary>
        /// Gets the additional information that specifies transaction behaviors.
        /// </summary>
        /// <value>The transaction options.</value>
        TransactionOptions TransactionOptions { get; }

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        /// <returns>IServiceResponse{Unit}.</returns>
        IServiceResponse<Unit> Commit();
    }
}