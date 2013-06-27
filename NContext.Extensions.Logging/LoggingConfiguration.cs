// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingConfiguration.cs" company="Waking Venture, Inc.">
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