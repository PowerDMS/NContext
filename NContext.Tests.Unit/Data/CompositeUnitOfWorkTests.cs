// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeUnitOfWorkTests.cs">
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
using System.Transactions;

using NContext.Data;
using NContext.Extensions.EntityFramework;

using NUnit.Framework;

namespace NContext.Tests.Unit.Data
{
    using NContext.Data.Persistence;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class CompositeUnitOfWorkTests
    {
        [Test]
        public void CreateUnitOfWork_RequiredTransaction_UsesSameTransactionId()
        {
            String originalTransactionId = String.Empty;
            String comparisonTransactionId = String.Empty;
            var stubPersistence = new PersistenceFactory();
            using (var uow1 = stubPersistence.CreateUnitOfWork(TransactionScopeOption.Required))
            {
                //originalTransactionId = ThreadSafeTransaction.Current.TransactionInformation.LocalIdentifier;
                using (var uow2 = stubPersistence.CreateUnitOfWork(TransactionScopeOption.Required))
                {
                    //comparisonTransactionId  = ThreadSafeTransaction.Current.TransactionInformation.LocalIdentifier;

                    uow2.Commit();
                }

                uow1.Commit();
            }

            Assert.That(originalTransactionId.Equals(comparisonTransactionId, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void CreateUnitOfWork_DifferentCompositeUnitOfWorkInstancesWithRequiredTransaction_UsesSameTransactionId()
        {
            String originalTransactionId = String.Empty;
            String comparisonTransactionId = String.Empty;
            var stubPersistence = new PersistenceFactory();
            var stubPersistence2 = new PersistenceFactory();
            using (var uow1 = stubPersistence.CreateUnitOfWork(TransactionScopeOption.Required))
            {
                originalTransactionId = Transaction.Current.TransactionInformation.LocalIdentifier;
                using (var uow2 = stubPersistence2.CreateUnitOfWork(TransactionScopeOption.Required))
                {
                    comparisonTransactionId = Transaction.Current.TransactionInformation.LocalIdentifier;

                    uow2.Commit(); // Should not commit!
                } // Should pop from stack

                uow1.Commit(); // Should commit all units of work.
            } // Should dispose uow2 and uow1

            Assert.That(originalTransactionId.Equals(comparisonTransactionId, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void CreateUnitOfWork_DifferentCompositeUnitOfWorkInstancesWithSeparateTransactions_UsesSameTransactionId()
        {
            String originalTransactionId = String.Empty;
            String comparisonTransactionId = String.Empty;
            var stubPersistence = new PersistenceFactory();
            var stubPersistence2 = new PersistenceFactory();
            using (var uow1 = stubPersistence.CreateUnitOfWork())
            {
                originalTransactionId = Transaction.Current.TransactionInformation.LocalIdentifier;
                using (var uow2 = stubPersistence2.CreateUnitOfWork(TransactionScopeOption.RequiresNew))
                {
                    comparisonTransactionId = Transaction.Current.TransactionInformation.LocalIdentifier;

                    uow2.Commit(); // Should not commit!
                } // Should pop from stack

                uow1.Commit(); // Should commit all units of work.
            } // Should dispose uow2 and uow1

            Assert.That(originalTransactionId.Equals(comparisonTransactionId, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void Bla()
        {
            var stubPersistence = new PersistenceFactory();

            using (var uow = stubPersistence.CreateUnitOfWork())
            {
                //using (efRepository)
                {

                }

                //using (rdbRepository)
                {

                }

                uow.Commit();
            }
        }

        /*public void Bla2()
        {
            var stubPersistence = new PersistenceFactory();
            var stubEfPersistence = new EfPersistenceFactory();
            using (var uow = stubPersistence.CreateUnitOfWork())
            {
                using (var efUoW = stubEfPersistence.CreateUnitOfWork(TransactionScopeOption.Required))
                {

                }

                //using (var rdbUoW = stubRdbPersistence.CreateUnitOfWork())
                {

                }

                uow.Commit();
            }
        }*/
    }
}