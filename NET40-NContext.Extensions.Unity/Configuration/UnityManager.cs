namespace NContext.Extensions.Unity.Configuration
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    using NContext.Configuration;

    /// <summary>
    /// Implementation of <see cref="IManageUnity"/> interface for management of a <see cref="IUnityContainer"/>.
    /// </summary>
    public class UnityManager : IManageUnity
    {
        private readonly UnityConfiguration _UnityConfiguration;

        private Boolean _IsConfigured;

        private IUnityContainer _Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityManager"/> class.
        /// </summary>
        /// <param name="unityConfiguration">The dependency injection configuration.</param>
        /// <remarks></remarks>
        public UnityManager(UnityConfiguration unityConfiguration)
        {
            if (unityConfiguration == null)
            {
                throw new ArgumentNullException("unityConfiguration");
            }

            _UnityConfiguration = unityConfiguration;
        }

        /// <summary>
        /// Gets the unity container.
        /// </summary>
        public IUnityContainer Container
        {
            get
            {
                return _Container;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
            protected set
            {
                _IsConfigured = value;
            }
        }

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            _Container = UnityContainerFactory.Create(
                _UnityConfiguration.ConfigurationFileName,
                _UnityConfiguration.ConfigurationSectionName,
                _UnityConfiguration.ContainerName);

            SetServiceLocator();
                
            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageUnity>(this);
            applicationConfiguration.CompositionContainer.ComposeExportedValue<IUnityContainer>(_Container);
            _Container.RegisterInstance<CompositionContainer>(applicationConfiguration.CompositionContainer);

            applicationConfiguration.CompositionContainer
                .GetExportedValues<IConfigureAUnityContainer>()
                .OrderBy(configurable => configurable.Priority)
                .ForEach(configurable => configurable.ConfigureContainer(_Container));
                
            _IsConfigured = true;
        }

        protected void SetServiceLocator()
        {
            var serviceLocator = new UnityServiceLocator(_Container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }
    }
}