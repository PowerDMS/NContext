using AutoMapper.Configuration;

namespace NContext.Extensions.AutoMapper.Configuration
{
    using NContext.Configuration;

    using global::AutoMapper;

    /// <summary>
    /// Defines an AutoMapper application component.
    /// </summary>
    public interface IManageAutoMapper : IApplicationComponent
    {
        /// <summary>
        /// Gets the AutoMapper configuration.
        /// </summary>
        /// <remarks></remarks>
        IConfigurationProvider ConfigurationProvider { get; }

        /// <summary>
        /// Gets the AutoMapper mapper instance.
        /// </summary>
        /// <remarks></remarks>
        IMapper Mapper { get; }
    }
}