namespace NContext.Data.Persistence
{
    using System.Transactions;

    /// <summary>
    /// Defines a general abstraction for creating instances of <see cref="IUnitOfWork"/>.
    /// </summary>
    public interface IPersistenceFactory
    {
        /// <summary>
        /// Creates an <see cref="IUnitOfWork"/> instance.
        /// </summary>
        /// <returns>IUnitOfWork instance.</returns>
        IUnitOfWork CreateUnitOfWork();

        /// <summary>
        /// Creates an <see cref="IUnitOfWork"/> instance.
        /// </summary>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <returns>IUnitOfWork instance.</returns>
        IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption);

        /// <summary>
        /// Creates an <see cref="IUnitOfWork" /> instance.
        /// </summary>
        /// <param name="transactionScopeOption">The transaction scope option.</param>
        /// <param name="transactionOptions">The transaction options.</param>
        /// <returns>IUnitOfWork instance.</returns>
        IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption, TransactionOptions transactionOptions);
    }
}