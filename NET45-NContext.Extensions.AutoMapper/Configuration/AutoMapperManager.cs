namespace NContext.Extensions.AutoMapper.Configuration
{
    using System;
    using System.Linq;
    using System.ComponentModel.Composition;

    using global::AutoMapper;

    using NContext.Configuration;

    public class AutoMapperManager : IManageAutoMapper
    {
        //        private IConfiguration _Configuration; // Uncomment with AM >= v4

        private static readonly Lazy<IConfiguration> _ConfigurationStore =
                    new Lazy<IConfiguration>(() => Mapper.Configuration); // REMOVE THIS with AM >= v4

        private Boolean _IsConfigured;

        /// <summary>
        /// Gets the AutoMapper configuration.
        /// </summary>
        /// <remarks></remarks>
        public virtual IConfiguration Configuration
        {
            get
            {
                return _ConfigurationStore.Value;
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
            Mapper.Initialize(c => mappingConfigurations
                .OrderBy(mappingConfiguration => mappingConfiguration.Priority)
                .ForEach(mappingConfiguration => mappingConfiguration.Configure(Configuration)));
            

            // Uncomment the following for AutoMapper >= version 4.
            //            _Configuration = new MapperConfiguration(c =>
            //            {
            //                mappingConfigurations
            //                    .OrderBy(mappingConfiguration => mappingConfiguration.Priority)
            //                    .ForEach(mappingConfiguration => mappingConfiguration.Configure(c));
            //            });

            _IsConfigured = true;
        }
    }
}