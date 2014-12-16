namespace NContext.Extensions.AutoMapper.Configuration
{
    using NContext.Configuration;

    using global::AutoMapper;

    /// <summary>
    /// Defines an AutoMapper application component.
    /// </summary>
    public interface IManageAutoMapper : IApplicationComponent
    {
        IConfigurationProvider ConfigurationProvider { get; }

        /// <summary>
        /// Gets the AutoMapper configuration.
        /// </summary>
        /// <remarks></remarks>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the AutoMapper mapping engine.
        /// </summary>
        /// <remarks></remarks>
        IMappingEngine MappingEngine { get; }
    }
}