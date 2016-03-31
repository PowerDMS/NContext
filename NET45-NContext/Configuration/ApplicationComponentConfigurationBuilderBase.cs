namespace NContext.Configuration
{
    using System;

    /// <summary>
    /// Defines an abstraction for creating application component configurations which 
    /// can in turn be used with an <see cref="ApplicationConfigurationBuilder"/>.
    /// </summary>
    /// <remarks></remarks>
    public abstract class ApplicationComponentConfigurationBuilderBase
    {
        private readonly ApplicationConfigurationBuilder _ApplicationConfigurationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        protected ApplicationComponentConfigurationBuilderBase(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            if (applicationConfigurationBuilder == null)
            {
                throw new ArgumentNullException("applicationConfigurationBuilder");
            }

            _ApplicationConfigurationBuilder = applicationConfigurationBuilder;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApplicationComponentConfigurationBuilderBase"/> 
        /// to <see cref="ApplicationConfiguration"/>.
        /// </summary>
        /// <param name="componentConfigurationBuilder">The configuration builder section.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator ApplicationConfiguration(ApplicationComponentConfigurationBuilderBase componentConfigurationBuilder)
        {
            componentConfigurationBuilder.Setup();

            return componentConfigurationBuilder.Builder;
        }

        /// <summary>
        /// Gets the builder instance.
        /// </summary>
        /// <remarks></remarks>
        protected ApplicationConfigurationBuilder Builder
        {
            get
            {
                return _ApplicationConfigurationBuilder;
            }
        }

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBase" />.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>ApplicationComponentBuilder.</returns>
        public ApplicationComponentBuilder RegisterComponent<TApplicationComponent>()
            where TApplicationComponent : class, IApplicationComponent
        {
            Setup();

            return _ApplicationConfigurationBuilder.RegisterComponent<TApplicationComponent>();
        }

        /// <summary>
        /// Registers the component with the <see cref="ApplicationConfigurationBase" />.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <returns>ApplicationConfigurationBuilder.</returns>
        public ApplicationConfigurationBuilder RegisterComponent<TApplicationComponent>(Func<TApplicationComponent> componentFactory)
            where TApplicationComponent : class, IApplicationComponent
        {
            Setup();
            
            return _ApplicationConfigurationBuilder.RegisterComponent<TApplicationComponent>(componentFactory);
        }

        /// <summary>
        /// Applies the component configuration with the <see cref="ApplicationConfigurationBase"/>.
        /// </summary>
        /// <remarks></remarks>
        protected abstract void Setup();
    }
}