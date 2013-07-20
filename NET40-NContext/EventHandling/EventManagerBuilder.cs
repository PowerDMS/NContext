namespace NContext.EventHandling
{
    using System;

    using NContext.Configuration;

    /// <summary>
    /// Defines an application component builder for <see cref="EventManager"/>.
    /// </summary>
    public class EventManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private Func<IActivationProvider> _ActivationProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase" /> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        public EventManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder) 
            : base(applicationConfigurationBuilder)
        {
        }

        /// <summary>
        /// Sets the activation provider used to resolve <see cref="IHandleEvent{T}"/> and <see cref="IGracefullyHandleEvent{T}"/> instances.
        /// </summary>
        /// <param name="activationProviderFactory">The activation provider factory.</param>
        /// <returns>EventManagerBuilder.</returns>
        public EventManagerBuilder SetActivationProvider(Func<IActivationProvider> activationProviderFactory)
        {
            _ActivationProviderFactory = activationProviderFactory;

            return this;
        }

        protected override void Setup()
        {
            Builder.RegisterComponent<IManageEvents>(
                () =>
                new EventManager(_ActivationProviderFactory.Invoke()));
        }
    }
}