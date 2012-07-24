
namespace NContext.Extensions.EntityFramework.Tests.Unit
{
    using System;
    using System.Transactions;

    using NContext.Data;
    using NContext.Data.Persistence;

    using NUnit.Framework;

    [TestFixture]
    public class EfUnitOfWorkTests
    {
        [Test]
        public void Bla()
        {
            // Not dependent upon any specific persistence mechanism, just System.Transactions
            var persistence = new PersistenceFactory(
                new PersistenceOptions
                    {
                        MaxDegreeOfParallelism = 5
                    });

            // Domain Service -----------------------------------------
            using (var uow = persistence.CreateUnitOfWork()) // Composite
            {
                // Infrastructure - Repository 1 -----------------------------------------
                IEfPersistenceFactory efPersistence1 = new EfPersistenceFactory();
                using (efPersistence1.CreateUnitOfWork()) // Adds to composite
                {
                    // TRANSACTIONAL WORK HERE

                    // Infrastructure -Repository 2 -----------------------------------------
                    IEfPersistenceFactory efPersistence2 = new EfPersistenceFactory();
                    using (efPersistence2.CreateUnitOfWork()) // Retains the parent scope
                    {
                        // TRANSACTIONAL WORK HERE
                    }
                }

                // Repository 3
                IEfPersistenceFactory efPersistence3 = new EfPersistenceFactory();
                using (efPersistence3.CreateUnitOfWork())
                {
                    // part of composite
                    // TRANSACTIONAL WORK HERE
                }

                // Repository 4
                IEfPersistenceFactory efPersistence4 = new EfPersistenceFactory();
                using (efPersistence4.CreateUnitOfWork())
                {
                    // part of composite
                    // TRANSACTIONAL WORK HERE
                }

                // Repository 5 - Separate Transaction
                IEfPersistenceFactory efPersistence5 = new EfPersistenceFactory();
                using (var efUow = efPersistence5.CreateUnitOfWork(TransactionScopeOption.RequiresNew))
                {
                    efUow.Commit(); // New transaction, Commits
                }

                // Stupid example of doing the same thing as above via TransactionScopeOption.Suppress
                using (efPersistence5.CreateUnitOfWork(TransactionScopeOption.Suppress)) // New non-atomic, non-transactional unit.
                using (var efUow = efPersistence5.CreateUnitOfWork())
                {
                    efUow.Commit(); // New transaction, Commits
                }

                // Nested Composites Tests
                using (var uow2 = persistence.CreateUnitOfWork())
                {
                    uow2.Commit(); // Nothing...
                }

                using (var uow3 = persistence.CreateUnitOfWork(TransactionScopeOption.RequiresNew))
                {
                    uow3.Commit(); // Commits!
                }
                
                uow.Commit(); // Commits composite
            }
        }
    }
}
