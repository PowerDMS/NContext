namespace NContext.Extensions.Logging
{
    using System;
    using System.Collections.Generic;
    using NContext.Extensions.Logging.Targets;

    public class LoggingConfiguration
    {
        private readonly ISet<Lazy<ILogTarget>> _LogTargetFactories;

        private readonly Int32 _MaxDegreeOfParallelism;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConfiguration"/> class.
        /// </summary>
        /// <param name="logTargets">The log targets.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        public LoggingConfiguration(ISet<Lazy<ILogTarget>> logTargets, Int32 maxDegreeOfParallelism)
        {
            _LogTargetFactories = logTargets;
            _MaxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// Gets the log target factories.
        /// </summary>
        /// <value>The log target factories.</value>
        public ISet<Lazy<ILogTarget>> LogTargetFactories
        {
            get { return _LogTargetFactories; }
        }

        /// <summary>
        /// Gets the max degree of parallelism.
        /// </summary>
        /// <value>The max degree of parallelism.</value>
        public Int32 MaxDegreeOfParallelism
        {
            get { return _MaxDegreeOfParallelism; }
        }
    }
}