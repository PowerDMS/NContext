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
    public class PersistenceOptions
    {
        private readonly IsolationLevel _IsolationLevel;

        private Int32 _MaxDegreeOfParallelism;

        private readonly TimeSpan _TransactionTimeOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceOptions" /> class.
        /// </summary>
        public PersistenceOptions()
            : this(new TimeSpan(), IsolationLevel.Serializable, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="transactionTimeOut">The transaction time out.</param>
        public PersistenceOptions(TimeSpan transactionTimeOut)
            : this(transactionTimeOut, IsolationLevel.Serializable, 1)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        public PersistenceOptions(Int32 maxDegreeOfParallelism)
            : this(new TimeSpan(), IsolationLevel.Serializable, maxDegreeOfParallelism)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        public PersistenceOptions(IsolationLevel isolationLevel)
            : this(new TimeSpan(), isolationLevel, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="transactionTimeOut">The transaction time out.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        public PersistenceOptions(TimeSpan transactionTimeOut, Int32 maxDegreeOfParallelism)
            : this(transactionTimeOut, IsolationLevel.Serializable, maxDegreeOfParallelism)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        public PersistenceOptions(IsolationLevel isolationLevel, Int32 maxDegreeOfParallelism)
            : this(new TimeSpan(), isolationLevel, maxDegreeOfParallelism)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="transactionTimeOut">The transaction time out.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        public PersistenceOptions(TimeSpan transactionTimeOut, IsolationLevel isolationLevel, Int32 maxDegreeOfParallelism)
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
        /// Gets the default transaction options.
        /// </summary>
        /// <value>The transaction options.</value>
        public TransactionOptions TransactionOptions
        {
            get
            {
                return new TransactionOptions { Timeout = _TransactionTimeOut, IsolationLevel = _IsolationLevel };
            }
        }
    }
}