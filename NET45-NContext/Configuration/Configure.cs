namespace NContext.Configuration
{
    using System;

    /// <summary>
    /// Defines a static class for fluid-interface application configuration.
    /// </summary>
    /// <remarks></remarks>
    public static class Configure
    {
        /// <summary>
        /// Configures the application using the specified <see cref="ApplicationConfigurationBase"/> instance.
        /// </summary>
        /// <typeparam name="TApplicationConfiguration">The type of the application configuration.</typeparam>
        /// <param name="applicationConfiguration">The application configuration instance.</param>
        /// <remarks></remarks>
        public static void Using<TApplicationConfiguration>(TApplicationConfiguration applicationConfiguration) 
            where TApplicationConfiguration : ApplicationConfigurationBase
        {
            if (applicationConfiguration == null)
            {
                throw new ArgumentNullException("applicationConfiguration");
            }

            applicationConfiguration.Setup();
        }
    }
}