// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistenceOptions.cs" company="Waking Venture, Inc.">
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

    /// <summary>
    /// Defines persistence-related options for transactional operations.
    /// </summary>
    public struct PersistenceOptions
    {
        private TimeSpan _TransactionTimeOut;

        private Int32 _MaxDegreeOfParallelism;

        private IsolationLevel _IsolationLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="transactionTimeOut">The transaction time out.</param>
        public PersistenceOptions(TimeSpan transactionTimeOut)
            : this(transactionTimeOut, 1, IsolationLevel.Serializable)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        public PersistenceOptions(Int32 maxDegreeOfParallelism)
            : this(TransactionManager.DefaultTimeout, maxDegreeOfParallelism, IsolationLevel.Serializable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        public PersistenceOptions(IsolationLevel isolationLevel)
            : this(TransactionManager.DefaultTimeout, 1, isolationLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="transactionTimeOut">The transaction time out.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        public PersistenceOptions(TimeSpan transactionTimeOut, Int32 maxDegreeOfParallelism)
            : this(transactionTimeOut, maxDegreeOfParallelism, IsolationLevel.Serializable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        public PersistenceOptions(Int32 maxDegreeOfParallelism, IsolationLevel isolationLevel)
            : this(TransactionManager.DefaultTimeout, maxDegreeOfParallelism, isolationLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="transactionTimeOut">The transaction time out.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        public PersistenceOptions(TimeSpan transactionTimeOut, Int32 maxDegreeOfParallelism, IsolationLevel isolationLevel)
        {
            _TransactionTimeOut = transactionTimeOut;
            _MaxDegreeOfParallelism = maxDegreeOfParallelism;
            _IsolationLevel = isolationLevel;
        }

        /// <summary>
        /// Gets or sets the max degree of parallelism.
        /// </summary>
        /// <value>The max degree of parallelism.</value>
        public Int32 MaxDegreeOfParallelism
        {
            get
            {
                return _MaxDegreeOfParallelism == 0 ? 1 : _MaxDegreeOfParallelism;
            }
            set
            {
                _MaxDegreeOfParallelism = value;
            }
        }

        /// <summary>
        /// Gets or sets the transaction time out.
        /// </summary>
        /// <value>The transaction time out.</value>
        public TimeSpan TransactionTimeOut
        {
            get
            {
                return _TransactionTimeOut;
            }
            set
            {
                _TransactionTimeOut = value;
            }
        }

        /// <summary>
        /// Gets or sets the isolation level.
        /// </summary>
        /// <value>The isolation level.</value>
        public IsolationLevel IsolationLevel
        {
            get
            {
                return _IsolationLevel;
            }
            set
            {
                _IsolationLevel = value;
            }
        }
    }
}