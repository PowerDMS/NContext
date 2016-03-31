namespace NContext.Configuration
{
    using System;

    /// <summary>
    /// Defines a fluent-interface builder for application components.
    /// </summary>
    public class ApplicationComponentBuilder
    {
        private readonly ApplicationConfigurationBuilder _ApplicationConfigurationBuilder;

        private ApplicationComponentConfigurationBuilderBase _ApplicationComponentConfigurationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration builder.</param>
        /// <remarks></remarks>
        public ApplicationComponentBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            _ApplicationConfigurationBuilder = applicationConfigurationBuilder;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApplicationComponentConfigurationBuilderBase"/>
        /// to <see cref="ApplicationConfiguration"/>.
        /// </summary>
        /// <param name="applicationComponentBuilder">The component configuration builder.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator ApplicationConfiguration(ApplicationComponentBuilder applicationComponentBuilder)
        {
            return applicationComponentBuilder._ApplicationConfigurationBuilder;
        }

        /// <summary>
        /// Configure's the <see cref="IApplicationComponent"/> with the specified <typeparamref name="TComponentConfigurationBuilder"/> instance.
        /// </summary>
        /// <typeparam name="TComponentConfigurationBuilder">The type of the component configuration.</typeparam>
        /// <returns><typeparamref name="TComponentConfigurationBuilder"/> instance to configure.</returns>
        /// <remarks></remarks>
        public TComponentConfigurationBuilder With<TComponentConfigurationBuilder>() 
            where TComponentConfigurationBuilder : ApplicationComponentConfigurationBuilderBase
        {
            if (_ApplicationComponentConfigurationBuilder != null)
            {
                return _ApplicationComponentConfigurationBuilder as TComponentConfigurationBuilder;
            }

            var applicationComponentConfiguration = 
                (TComponentConfigurationBuilder)Activator.CreateInstance(typeof(TComponentConfigurationBuilder), _ApplicationConfigurationBuilder);

            _ApplicationComponentConfigurationBuilder = applicationComponentConfiguration;

            return applicationComponentConfiguration;
        }
    }
}