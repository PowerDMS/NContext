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