// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogManager.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.Logging
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks.Dataflow;

    using NContext.Configuration;
    using NContext.Extensions.Logging.Targets;

    /// <summary>
    /// Defines a log manager which leverages TPL Dataflow for efficient logging operations.
    /// </summary>
    public class LogManager : IManageLogging
    {
        private readonly LoggingConfiguration _LoggingConfiguration;
        private readonly BroadcastBlock<LogEntry> _Broadcast;

        private Boolean _IsConfigured;
        private ISet<ILogTarget> _LogTargets;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        /// <param name="loggingConfiguration">The logging configuration.</param>
        public LogManager(LoggingConfiguration loggingConfiguration)
        {
            _LoggingConfiguration = loggingConfiguration;

            _Broadcast = new BroadcastBlock<LogEntry>(
                logEntry => 
                    logEntry.Clone(),
                    new ExecutionDataflowBlockOptions
                        {
                            MaxDegreeOfParallelism = loggingConfiguration.MaxDegreeOfParallelism <= 0 
                                                         ? Environment.ProcessorCount 
                                                         : loggingConfiguration.MaxDegreeOfParallelism
                        });
        }

        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <value>The targets.</value>
        public ISet<ILogTarget> Targets
        {
            get { return _LogTargets; }
        }

        /// <summary>
        /// Logs the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        public void Log(LogEntry logEntry)
        {
            _Broadcast.Post(logEntry);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>The is configured.</value>
        public Boolean IsConfigured
        {
            get { return _IsConfigured; }
        }

        /// <summary>
        /// Configures the component instance. This method should set <see cref="IsConfigured" />.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        public void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageLogging>(this);

            _LogTargets = new HashSet<ILogTarget>(_LoggingConfiguration.LogTargetFactories.Select(logTargetFactory => logTargetFactory.Value));
            _LogTargets.ForEach(logTarget => _Broadcast.LinkTo(logTarget, new DataflowLinkOptions(), logTarget.ShouldLog));

            _IsConfigured = true;
        }
    }
}