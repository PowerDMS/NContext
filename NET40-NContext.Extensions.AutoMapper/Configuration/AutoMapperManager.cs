namespace NContext.Extensions.AutoMapper.Configuration
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;

    using NContext.Configuration;

    using global::AutoMapper;

    /// <summary>
    /// Defines an AutoMapper application component.
    /// </summary>
    /// <remarks></remarks>
    public class AutoMapperManager : IManageAutoMapper
    {
        private static readonly Lazy<IConfiguration> _ConfigurationStore =
            new Lazy<IConfiguration>(() => Mapper.Configuration);

        private static readonly Lazy<IMappingEngine> _MappingEngine =
            new Lazy<IMappingEngine>(() => Mapper.Engine);

        private Boolean _IsConfigured;

        /// <summary>
        /// Gets the configuration provider.
        /// </summary>
        /// <remarks></remarks>
        public IConfigurationProvider ConfigurationProvider
        {
            get
            {
                return (IConfigurationProvider)_ConfigurationStore.Value;
            }
        }

        /// <summary>
        /// Gets the AutoMapper configuration.
        /// </summary>
        /// <remarks></remarks>
        public IConfiguration Configuration
        {
            get
            {
                return _ConfigurationStore.Value;
            }
        }

        /// <summary>
        /// Gets the AutoMapper mapping engine.
        /// </summary>
        /// <remarks></remarks>
        public IMappingEngine MappingEngine
        {
            get
            {
                return _MappingEngine.Value;
            }
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
        /// Configures the component instance. This method should set <see cref="IApplicationComponent.IsConfigured"/>.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks>
        /// </remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (IsConfigured)
            {
                return;
            }

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageAutoMapper>(this);

            var mappingConfigurations = applicationConfiguration.CompositionContainer.GetExportedValues<IConfigureAutoMapper>();
            mappingConfigurations.OrderBy(mappingConfiguration => mappingConfiguration.Priority)
                                 .ForEach(mappingConfiguration => mappingConfiguration.Configure(Configuration));

            _IsConfigured = true;
        }
    }
}