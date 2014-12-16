namespace NContext.Extensions.Ninject.Configuration
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    using NContext.Configuration;

    using global::Ninject;

    /// <summary>
    /// Defines a dependency injection application component using Ninject.
    /// </summary>
    public class NinjectManager : IManageNinject
    {
        private readonly NinjectConfiguration _Configuration;

        private Boolean _IsConfigured;

        private IKernel _Kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectManager"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <remarks></remarks>
        public NinjectManager(NinjectConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _Configuration = configuration;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <remarks></remarks>
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
        /// Gets the <see cref="IKernel"/> instance.
        /// </summary>
        /// <remarks></remarks>
        public IKernel Kernel
        {
            get
            {
                return _Kernel;
            }
        }

        /// <summary>
        /// Configures the component instance. This method should set <see cref="IApplicationComponent.IsConfigured"/>.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks>
        /// </remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            _Kernel = _Configuration.CreateKernel();

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IKernel>(_Kernel);
            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageNinject>(this);
            _Kernel.Bind<CompositionContainer>().ToConstant(applicationConfiguration.CompositionContainer).InSingletonScope();

            applicationConfiguration.CompositionContainer
                                    .GetExportedValues<IConfigureANinjectKernel>()
                                    .OrderBy(configurable => configurable.Priority)
                                    .ForEach(configurable => configurable.ConfigureKernel(_Kernel));

            _IsConfigured = true;
        }
    }
}