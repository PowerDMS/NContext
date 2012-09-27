// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistenceFactoryBase.cs" company="Waking Venture, Inc.">
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
    using System.Web;

    public abstract class PersistenceFactoryBase
    {
        private readonly Lazy<AmbientContextManagerBase> _AmbientContextManager;

        protected PersistenceFactoryBase() : this(null)
        {
        }

        protected PersistenceFactoryBase(IAmbientContextManagerFactory contextManagerFactory)
        {
            var factoryMethod = contextManagerFactory == null
                                    ? new Func<AmbientContextManagerBase>(CreateDefaultAmbientContextManager)
                                    : new Func<AmbientContextManagerBase>(contextManagerFactory.Create);

            _AmbientContextManager = new Lazy<AmbientContextManagerBase>(factoryMethod);
        }

        protected internal AmbientContextManagerBase AmbientContextManager
        {
            get
            {
                return _AmbientContextManager.Value;
            }
        }

        public virtual IUnitOfWork CreateUnitOfWork(TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            switch (transactionScopeOption)
            {
                case TransactionScopeOption.Required:
                case TransactionScopeOption.RequiresNew:
                    throw new NotSupportedException("PersistenceFactoryBase does not support TransactionScopeOption.Required or TransactionScopeOption.RequiresNew.");
                case TransactionScopeOption.Suppress:
                    var unitOfWork = new NonAtomicUnitOfWork(AmbientContextManager);
                    AmbientContextManager.AddUnitOfWork(null);
                    return unitOfWork;
                default:
                    throw new ArgumentOutOfRangeException("transactionScopeOption");
            }
        }

        /// <summary>
        /// Creates the default transaction manager. Local Default:
        /// <see cref="PerRequestAmbientContextManager"/> if HttpContext exists; otherwise <see cref="ThreadLocalAmbientContextManager" />.
        /// </summary>
        /// <returns>AmbientContextManagerBase.</returns>
        protected virtual AmbientContextManagerBase CreateDefaultAmbientContextManager()
        {
            return HttpContext.Current != null
                ? (AmbientContextManagerBase)new PerRequestAmbientContextManager()
                : (AmbientContextManagerBase)new ThreadLocalAmbientContextManager();
        }
    }
}