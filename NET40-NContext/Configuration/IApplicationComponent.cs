namespace NContext.Configuration
{
    using System;

    /// <summary>
    /// Defines a contract for all application-level components.
    /// </summary>
    public interface IApplicationComponent
    {
        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsConfigured { get; }

        /// <summary>
        /// Configures the component instance. This method should set <see cref="IsConfigured"/>.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks>
        /// </remarks>
        void Configure(ApplicationConfigurationBase applicationConfiguration);
    }
}