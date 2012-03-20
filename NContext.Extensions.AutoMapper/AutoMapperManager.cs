using System;

using AutoMapper;
using AutoMapper.Mappers;

using NContext.Configuration;

namespace NContext.Extensions.AutoMapper
{
    public class AutoMapperManager : IManageAutoMapper
    {
        #region Fields

        private static readonly Lazy<ConfigurationStore> _ConfigurationStore =
            new Lazy<ConfigurationStore>(() => new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers()));

        private static readonly Lazy<IMappingEngine> _MappingEngine =
            new Lazy<IMappingEngine>(() => new MappingEngine(_ConfigurationStore.Value));

        private Boolean _IsConfigured;

        #endregion

        public IConfigurationProvider ConfigurationProvider
        {
            get
            {
                return _ConfigurationStore.Value;
            }
        }

        public IConfiguration Configuration
        {
            get
            {
                return _ConfigurationStore.Value;
            }
        }

        public IMappingEngine MappingEngine
        {
            get
            {
                return _MappingEngine.Value;
            }
        }

        #region Implementation of IApplicationComponent

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

            var mappingConfigurations = applicationConfiguration.CompositionContainer.GetExports<IConfigureAutoMapper>();
            mappingConfigurations.ForEach(mappingConfiguration => mappingConfiguration.Value.Configure(Configuration));

            

#if DEBUG
            ConfigurationProvider.AssertConfigurationIsValid();
#endif

            _IsConfigured = true;
        }

        #endregion
    }
}