namespace NContext.Extensions.Logging
{
    using System.Collections.Generic;
    using NContext.Configuration;
    using NContext.Extensions.Logging.Targets;

    /// <summary>
    /// Defines a log manager application component.
    /// </summary>
    public interface IManageLogging : IApplicationComponent
    {
        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <value>The targets.</value>
        ISet<ILogTarget> Targets { get; }

        /// <summary>
        /// Logs the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        void Log(LogEntry logEntry);
    }
}