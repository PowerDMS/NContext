// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistenceFactory.cs">
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
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Data.Persistence
{
    using System;
    using System.Transactions;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class PersistenceFactory : PersistenceFactoryBase, IPersistenceFactory
    {
        private readonly PersistenceOptions _PersistenceOptions;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceFactory"/> class.
        /// </summary>
        /// <remarks></remarks>
        public PersistenceFactory()
            : this(null, null)
        {
        }

        public PersistenceFactory(IAmbientTransactionManagerFactory transactionManagerFactory)
            : this(transactionManagerFactory, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceFactory"/> class.
        /// </summary>
        /// <param name="persistenceOptions">The persistence options.</param>
        /// <remarks></remarks>
        public PersistenceFactory(PersistenceOptions persistenceOptions)
            : this(null, persistenceOptions)
        {
        }

        public PersistenceFactory(IAmbientTransactionManagerFactory transactionManagerFactory, PersistenceOptions persistenceOptions)
            : base(transactionManagerFactory)
        {
            _PersistenceOptions = persistenceOptions ?? PersistenceOptions.Default;
        }

        #endregion

        #region Overrides of PersistenceFactoryBase

        public override IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            UnitOfWorkBase unitOfWork;
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                    if (TransactionManager.Ambient != null)
                    {
                        if (TransactionManager.Ambient.IsTypeOf<CompositeUnitOfWork>())
                        {
                            var currentCompositeUnitOfWork = ((CompositeUnitOfWork)TransactionManager.Ambient.UnitOfWork);
                            unitOfWork = new CompositeUnitOfWork(TransactionManager, currentCompositeUnitOfWork, _PersistenceOptions);
                            currentCompositeUnitOfWork.AddUnitOfWork(unitOfWork);
                            TransactionManager.AddUnitOfWork(unitOfWork);
                        }
                        else
                        {
                            unitOfWork = TransactionManager.Ambient.UnitOfWork;
                            TransactionManager.RetainAmbient();
                        }

                        return unitOfWork;
                    }

                    unitOfWork = new CompositeUnitOfWork(TransactionManager, _PersistenceOptions);
                    TransactionManager.AddUnitOfWork(unitOfWork);

                    return unitOfWork;
                case TransactionScopeOption.RequiresNew:
                    unitOfWork = new CompositeUnitOfWork(TransactionManager, _PersistenceOptions);
                    TransactionManager.AddUnitOfWork(unitOfWork);

                    return unitOfWork;
                case TransactionScopeOption.Suppress:
                    return base.CreateUnitOfWork(transactionScopeOption);
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        #endregion
    }
}