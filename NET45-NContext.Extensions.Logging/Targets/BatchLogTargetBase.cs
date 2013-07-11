// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BatchLogTargetBase.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
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

namespace NContext.Extensions.Logging.Targets
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Defines a log target abstraction which supports batching.
    /// </summary>
    public abstract class BatchLogTargetBase : ILogTarget
    {
        private readonly IPropagatorBlock<LogEntry, IEnumerable<LogEntry>> _BatchBlock;
        private readonly ITargetBlock<IEnumerable<LogEntry>> _ActionBlock;

        private readonly Timer _FlushTimer;
        private readonly TimeSpan _FlushInterval;
        private readonly Object _FlushLock = new Object();

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchLogTargetBase"/> class.
        /// </summary>
        /// <param name="batchSize">Size of the log batch.</param>
        /// <param name="flushInterval">
        /// The interval with which to initiate a batching operation even if the 
        /// number of currently queued logs is less than the <paramref name="batchSize"/>.
        /// </param>
        protected BatchLogTargetBase(Int32 batchSize, TimeSpan flushInterval)
            : this(batchSize, flushInterval, Environment.ProcessorCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchLogTargetBase"/> class.
        /// </summary>
        /// <param name="batchSize">Size of the log batch.</param>
        /// <param name="flushInterval">The flush interval.
        /// The interval with which to initiate a batching operation even if the 
        /// number of currently queued logs is less than the <paramref name="batchSize"/>.
        /// </param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism the target instance will log batch entries (ie. <see cref="Log"/> method).</param>
        protected BatchLogTargetBase(Int32 batchSize, TimeSpan flushInterval, Int32 maxDegreeOfParallelism)
        {
            _FlushInterval = flushInterval;
            _BatchBlock = new BatchBlock<LogEntry>(batchSize);
            _ActionBlock = new ActionBlock<IEnumerable<LogEntry>>(
                logEntries =>
                    {
                        _FlushTimer.Change(_FlushInterval, TimeSpan.FromMilliseconds(-1));
                        Log(logEntries);
                    },
                new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = maxDegreeOfParallelism
                    });

            _BatchBlock.LinkTo(_ActionBlock);

            _FlushTimer = new Timer(FlushTimerCallback, null, _FlushInterval, TimeSpan.FromMilliseconds(-1));
        }

        /// <summary>
        /// Predicate which determines whether or not the target instance should log this entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        /// <returns>Boolean.</returns>
        public abstract Boolean ShouldLog(LogEntry logEntry);

        /// <summary>
        /// Logs the specified log entries.
        /// </summary>
        /// <param name="logEntries">The log entries.</param>
        protected abstract void Log(IEnumerable<LogEntry> logEntries);

        private void FlushTimerCallback(Object state)
        {
            lock (_FlushLock)
            {
                ((BatchBlock<LogEntry>) _BatchBlock).TriggerBatch();
            }
        }

        /// <summary>
        /// Offers the message.
        /// </summary>
        /// <param name="messageHeader">The message header.</param>
        /// <param name="messageValue">The message value.</param>
        /// <param name="source">The source.</param>
        /// <param name="consumeToAccept">The consume to accept.</param>
        /// <returns>DataflowMessageStatus.</returns>
        public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, LogEntry messageValue, ISourceBlock<LogEntry> source, Boolean consumeToAccept)
        {
            return _BatchBlock.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        /// <summary>
        /// Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.
        /// </summary>
        public void Complete()
        {
            _BatchBlock.Complete();
        }

        /// <summary>
        /// Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.
        /// </summary>
        /// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
        public void Fault(Exception exception)
        {
            _BatchBlock.Fault(exception);
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.
        /// </summary>
        /// <value>The completion.</value>
        /// <returns>The task.</returns>
        public Task Completion
        {
            get { return _BatchBlock.Completion; }
        }
    }
}