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
        IConfiguration Configuration { get; }
    }
}