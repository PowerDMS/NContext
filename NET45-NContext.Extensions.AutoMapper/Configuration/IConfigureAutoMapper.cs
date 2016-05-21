namespace NContext.Extensions.AutoMapper.Configuration
{
    using System;
    using System.ComponentModel.Composition;

    using global::AutoMapper;

    /// <summary>
    /// Defines a role-interface for application composition and configuration of AutoMapper.
    /// </summary>
    [InheritedExport]
    public interface IConfigureAutoMapper
    {
        /// <summary>
        /// Configures the specified AutoMapper <see cref="IMapperConfiguration"/>.
        /// </summary>
        /// <param name="mapperConfiguration">The mapper configuration.</param>
        /// <remarks></remarks>
        void Configure(IConfiguration mapperConfiguration);

        /// <summary>
        /// Gets the priority used to configure the <see cref="IMapperConfiguration"/>. Lower 
        /// priority will cause implementations to execute first.
        /// </summary>
        Int32 Priority { get; }
    }
}