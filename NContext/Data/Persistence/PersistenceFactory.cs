// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistenceFactory.cs" company="Waking Venture, Inc.">
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

namespace NContext.Data.Persistence
{
    using System;
    using System.Transactions;

    public class PersistenceFactory : PersistenceFactoryBase, IPersistenceFactory
    {
        private readonly PersistenceOptions _PersistenceOptions;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceFactory"/> class.
        /// </summary>
        /// <remarks></remarks>
        public PersistenceFactory()
            : this(null, null)
        {
        }

        public PersistenceFactory(IAmbientContextManagerFactory contextManagerFactory)
            : this(contextManagerFactory, null)
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

        public PersistenceFactory(IAmbientContextManagerFactory contextManagerFactory, PersistenceOptions persistenceOptions)
            : base(contextManagerFactory)
        {
            _PersistenceOptions = persistenceOptions ?? PersistenceOptions.Default;
        }

        public override IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                    return GetRequiredUnitOfWork();
                case TransactionScopeOption.RequiresNew:
                    return GetRequiredNewUnitOfWork();
                case TransactionScopeOption.Suppress:
                    return base.CreateUnitOfWork(transactionScopeOption);
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        private IUnitOfWork GetRequiredUnitOfWork()
        {
            UnitOfWorkBase unitOfWork;
            if (!AmbientContextManager.AmbientExists)
            {
                return GetRequiredNewUnitOfWork();
            }

            if (AmbientContextManager.Ambient.IsTypeOf<CompositeUnitOfWork>())
            {
                var currentCompositeUnitOfWork = (CompositeUnitOfWork)AmbientContextManager.Ambient.UnitOfWork;
                unitOfWork = new CompositeUnitOfWork(AmbientContextManager, currentCompositeUnitOfWork, _PersistenceOptions);
                currentCompositeUnitOfWork.AddUnitOfWork(unitOfWork);
                AmbientContextManager.AddUnitOfWork(unitOfWork);
            }
            else
            {
                // TODO: (DG) Revisit this! If it's not a Composite, it's either a NonAtomic or something else. Should we really retain it if something else? Throw exc?
                unitOfWork = AmbientContextManager.Ambient.UnitOfWork;
                AmbientContextManager.RetainAmbient();
            }

            return unitOfWork;
        }

        private IUnitOfWork GetRequiredNewUnitOfWork()
        {
            UnitOfWorkBase unitOfWork = new CompositeUnitOfWork(AmbientContextManager, _PersistenceOptions);
            AmbientContextManager.AddUnitOfWork(unitOfWork);

            return unitOfWork;
        }
    }
}