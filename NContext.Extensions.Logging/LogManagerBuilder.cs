// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogManagerBuilder.cs" company="Waking Venture, Inc.">
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

    using NContext.Configuration;
    using NContext.Extensions.Logging.Targets;

    /// <summary>
    /// Defines a fluent builder for configuring a logging application component.
    /// </summary>
    public class LogManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private readonly ISet<Lazy<ILogTarget>> _LogTargets;

        private Int32 _MaxDegreeOfParallelism;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase" /> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        public LogManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder) 
            : base(applicationConfigurationBuilder)
        {
            _LogTargets = new HashSet<Lazy<ILogTarget>>();
        }

        /// <summary>
        /// Sets the max degree of parallelism which the <see cref="LogManager"/> will use to broadcast log 
        /// entries to it's <see cref="ILogTarget"/> collection. Defaults to <see cref="Environment.ProcessorCount"/>.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        /// <returns>LogManagerBuilder.</returns>
        public LogManagerBuilder SetMaxDegreeOfParallelism(Int32 maxDegreeOfParallelism)
        {
            _MaxDegreeOfParallelism = maxDegreeOfParallelism;

            return this;
        }

        /// <summary>
        /// Adds the log target.
        /// </summary>
        /// <param name="logTargetFactory">The log target factory.</param>
        /// <returns>LogManagerBuilder.</returns>
        public LogManagerBuilder AddLogTarget(Func<ILogTarget> logTargetFactory)
        {
            _LogTargets.Add(new Lazy<ILogTarget>(logTargetFactory));

            return this;
        }

        /// <summary>
        /// Applies the component configuration with the <see cref="ApplicationConfigurationBase" />.
        /// </summary>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageLogging>(
                   () =>
                       new LogManager(
                           new LoggingConfiguration(_LogTargets, _MaxDegreeOfParallelism)));
        }
    }
}