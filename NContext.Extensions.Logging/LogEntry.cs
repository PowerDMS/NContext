// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="Waking Venture, Inc.">
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