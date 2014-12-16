namespace NContext.Extensions.ValueInjecter.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NContext.Configuration;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines an application component for ValueInjecter. This component is used for 
    /// mapping objects based upon specified conventions.
    /// </summary>
    public class ValueInjecterManager : IManageValueInjecter
    {
        private static IEnumerable<Func<IValueInjection>> _ApplicationInjectionConventions;

        private Boolean _IsConfigured;

        static ValueInjecterManager()
        {
            _ApplicationInjectionConventions = Enumerable.Empty<Func<IValueInjection>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueInjecterManager"/> class.
        /// </summary>
        /// <param name="valueInjecterConfiguration">The value injecter configuration.</param>
        public ValueInjecterManager(ValueInjecterConfiguration valueInjecterConfiguration)
        {
            if (valueInjecterConfiguration.Conventions != null)
            {
                _ApplicationInjectionConventions = valueInjecterConfiguration.Conventions;
            }
        }

        /// <summary>
        /// Gets the application's injection conventions.
        /// </summary>
        /// <value>The conventions.</value>
        public static IEnumerable<Func<IValueInjection>> Conventions
        {
            get { return _ApplicationInjectionConventions; }
        }

        /// <summary>
        /// Returns a new <see cref="IFluentValueInjecter{T}" /> instance using <paramref name="source" />
        /// as the source object for value injection.
        /// </summary>
        /// <typeparam name="TSource">The type of the T source.</typeparam>
        /// <param name="source">The source intance.</param>
        /// <returns>IFluentValueInjecter{TSource}.</returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public IFluentValueInjecter<TSource> Inject<TSource>(TSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return new FluentValueInjecter<TSource>(source, Conventions);
        }

        public Boolean IsConfigured
        {
            get { return _IsConfigured; }
        }

        public void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (IsConfigured)
            {
                return;
            }

            _IsConfigured = true;
        }
    }
}