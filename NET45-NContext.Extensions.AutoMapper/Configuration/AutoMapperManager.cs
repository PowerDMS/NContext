using System.Collections.Generic;
using AutoMapper;
using static AutoMapper.Mapper;

namespace NContext.Extensions.AutoMapper.Configuration
{
    using System;
    using System.Linq;
    using System.ComponentModel.Composition;

    using NContext.Configuration;

    public class AutoMapperManager : IManageAutoMapper
    {
        private IConfigurationProvider _ConfigurationProvder;

        private IMapper _Mapper;

        private Boolean _IsConfigured;

        /// <summary>
        /// Gets the AutoMapper configuration.
        /// </summary>
        /// <remarks></remarks>
        public virtual IConfigurationProvider ConfigurationProvider
        {
            get
            {
                return _ConfigurationProvder;
            }
        }

        /// <summary>
        /// Gets the IMapper instance.
        /// </summary>
        /// <remarks></remarks>
        public virtual IMapper Mapper
        {
            get
            {
                return _Mapper;
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
                return;

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageAutoMapper>(this);
            var mappingConfigurations = applicationConfiguration.CompositionContainer.GetExportedValues<IConfigureAutoMapper>();

            _ConfigurationProvder = new MapperConfiguration(c =>
            {
                ConfigureMappings(c, mappingConfigurations);
            });
            _Mapper = _ConfigurationProvder.CreateMapper();

            Initialize(c =>
            {
                ConfigureMappings(c, mappingConfigurations);
            });

           _IsConfigured = true;
        }

        private void ConfigureMappings(
            IMapperConfigurationExpression mapperConfigurationExpression, 
            IEnumerable<IConfigureAutoMapper> mappingConfigurations)
        {
                mappingConfigurations
                    .OrderBy(mappingConfiguration => mappingConfiguration.Priority)
                    .ForEach(mappingConfiguration => mappingConfiguration.Configure(mapperConfigurationExpression));
        }
    }
}