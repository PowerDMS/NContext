namespace NContext.EventHandling
{
    using System;

    using NContext.Configuration;

    public class EventManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private Func<IActivationProvider> _ActivationProviderFactory;
        
        public EventManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder) 
            : base(applicationConfigurationBuilder)
        {
        }

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