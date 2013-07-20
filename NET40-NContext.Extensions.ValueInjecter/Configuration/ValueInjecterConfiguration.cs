namespace NContext.Extensions.ValueInjecter.Configuration
{
    using System;
    using System.Collections.Generic;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines configuration for <see cref="ValueInjecterManager"/>
    /// </summary>
    public class ValueInjecterConfiguration
    {
        private readonly ISet<Func<IValueInjection>> _Conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueInjecterConfiguration"/> class.
        /// </summary>
        /// <param name="conventions">The conventions.</param>
        public ValueInjecterConfiguration(ISet<Func<IValueInjection>> conventions)
        {
            _Conventions = conventions;
        }

        /// <summary>
        /// Gets the application's injection conventions.
        /// </summary>
        /// <value>The conventions.</value>
        public IEnumerable<Func<IValueInjection>> Conventions
        {
            get { return _Conventions; }
        }
    }
}