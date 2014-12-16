namespace NContext.Extensions.Logging.Targets
{
    using System;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Defines a log target which handles the actual logging procedure.
    /// </summary>
    public interface ILogTarget : ITargetBlock<LogEntry>
    {
        /// <summary>
        /// Predicate which determines whether or not the target instance should log this entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        /// <returns>Boolean.</returns>
        Boolean ShouldLog(LogEntry logEntry);
    }
}