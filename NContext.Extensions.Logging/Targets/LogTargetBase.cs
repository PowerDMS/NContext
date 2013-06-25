// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogTargetBase.cs" company="Waking Venture, Inc.">
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
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Defines a log target abstraction.
    /// </summary>
    public abstract class LogTargetBase : ILogTarget
    {
        private readonly ITargetBlock<LogEntry> _ActionBlock;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogTargetBase"/> class.
        /// </summary>
        protected LogTargetBase() : this(Environment.ProcessorCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogTargetBase"/> class.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">
        /// The max degree of parallelism the target instance will log entries (ie. <see cref="Log"/> method).
        /// </param>
        protected LogTargetBase(Int32 maxDegreeOfParallelism)
        {
            _ActionBlock = new ActionBlock<LogEntry>(
                logEntry => Log(logEntry),
                new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = maxDegreeOfParallelism
                    });
        }

        /// <summary>
        /// Predicate which determines whether or not the target instance should log this entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        /// <returns>Boolean.</returns>
        public abstract Boolean ShouldLog(LogEntry logEntry);

        /// <summary>
        /// Logs the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        protected abstract void Log(LogEntry logEntry);

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
            return _ActionBlock.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        /// <summary>
        /// Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.
        /// </summary>
        public void Complete()
        {
            _ActionBlock.Complete();
        }

        /// <summary>
        /// Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.
        /// </summary>
        /// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
        public void Fault(Exception exception)
        {
            _ActionBlock.Fault(exception);
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.
        /// </summary>
        /// <value>The completion.</value>
        /// <returns>The task.</returns>
        public Task Completion
        {
            get { return _ActionBlock.Completion; }
        }
    }
}