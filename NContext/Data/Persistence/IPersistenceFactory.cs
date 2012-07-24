namespace NContext.Data.Persistence
{
    using System.Transactions;

    public interface IPersistenceFactory
    {
        IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required);
    }
}