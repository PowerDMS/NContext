namespace NContext.Extensions.ValueInjecter.Configuration
{
    using System;
    using System.Collections.Generic;

    using NContext.Configuration;

    using Omu.ValueInjecter;

    /// <summary>
    /// Application component builder for <see cref="ValueInjecterManager" />.
    /// </summary>
    public class ValueInjecterManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private readonly ISet<Func<IValueInjection>> _Conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase" /> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        public ValueInjecterManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder) : base(applicationConfigurationBuilder)
        {
            _Conventions = new HashSet<Func<IValueInjection>>();
        }

        /// <summary>
        /// Adds the injection convention to be invoked for all value injections.
        /// </summary>
        /// <typeparam name="TValueInjection">The type of the T value injection.</typeparam>
        /// <returns>ValueInjecterManagerBuilder.</returns>
        public ValueInjecterManagerBuilder AddInjectionConvention<TValueInjection>()
            where TValueInjection : IValueInjection, new()
        {
            _Conventions.Add(() => Activator.CreateInstance<TValueInjection>());

            return this;
        }

        /// <summary>
        /// Adds the injection convention to be invoked for all value injections.
        /// </summary>
        /// <param name="valueInjectionFactory">The value injection factory.</param>
        /// <returns>ValueInjecterManagerBuilder.</returns>
        public ValueInjecterManagerBuilder AddInjectionConvention(Func<IValueInjection> valueInjectionFactory)
        {
            _Conventions.Add(valueInjectionFactory);

            return this;
        }

        protected override void Setup()
        {
            Builder.RegisterComponent<IManageValueInjecter>(
                () => new ValueInjecterManager(
                          new ValueInjecterConfiguration(_Conventions)));
        }
    }
}