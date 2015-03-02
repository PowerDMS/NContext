namespace NContext.Extensions.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NContext.Extensions.Logging.Targets;

    /// <summary>
    /// Defines a log entry.
    /// </summary>
    public class LogEntry
    {
        private readonly IEnumerable<String> _Categories;
        private readonly Object _Message;
        private readonly DateTimeOffset _OccurredOn;
        private readonly IDictionary<String, Object> _AuxiliaryProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="categories">The categories.</param>
        public LogEntry(Object message, IEnumerable<String> categories)
        {
            _Message = message;
            _Categories = categories;
            _OccurredOn = DateTimeOffset.UtcNow;
            _AuxiliaryProperties = new Dictionary<String, Object>();
        }

        private LogEntry(Object message, IEnumerable<String> categories, DateTimeOffset occurredOn, IDictionary<String, Object> auxiliaryProperties)
        {
            _Message = message;
            _Categories = new ReadOnlyCollection<String>(categories.ToList());
            _OccurredOn = occurredOn;
            _AuxiliaryProperties = new ReadOnlyDictionary<String, Object>(auxiliaryProperties);
        }

        /// <summary>
        /// Gets the date and time the log entry was created.
        /// </summary>
        /// <value>The occurred on.</value>
        public DateTimeOffset OccurredOn
        {
            get { return _OccurredOn; }
        }

        /// <summary>
        /// Gets the message of the log.
        /// </summary>
        /// <value>The message.</value>
        public String Message
        {
            get { return _Message.ToString(); }
        }

        /// <summary>
        /// Gets the categories associated with the log entry.
        /// </summary>
        /// <value>The categories.</value>
        public IEnumerable<String> Categories
        {
            get { return _Categories; }
        }

        /// <summary>
        /// Gets the auxiliary properties used to associate additional information with the log entry.
        /// </summary>
        /// <value>The auxiliary properties.</value>
        public IDictionary<String, Object> AuxiliaryProperties
        {
            get { return _AuxiliaryProperties; }
        }

        /// <summary>
        /// Creates a read-only clone from this instance. Used internally 
        /// for broadcasting the instance to multiple <see cref="ILogTarget"/>s.
        /// </summary>
        /// <returns>LogEntry.</returns>
        internal LogEntry Clone()
        {
            return new LogEntry(_Message, _Categories, _OccurredOn, _AuxiliaryProperties);
        }
    }
}