namespace NContext.Extensions.Unity.Configuration
{
    using System;

    using NContext.Configuration;

    /// <summary>
    /// Defines a configuration class to build the application's <see cref="UnityManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class UnityManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private String _ContainerName;

        private String _ConfigurationFileName;

        private String _ConfigurationSectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityManagerBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public UnityManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        /// <summary>
        /// Sets the name of the container.
        /// </summary>
        /// <param name="unityContainerName">Name of the unity container.</param>
        /// <returns>This <see cref="UnityManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public UnityManagerBuilder SetContainerName(String unityContainerName)
        {
            _ContainerName = unityContainerName;

            return this;
        }

        /// <summary>
        /// Sets the unity configuration file.
        /// </summary>
        /// <param name="configurationFileName">Name of the unity configuration file.</param>
        /// <param name="configurationSectionName">Name of the configuration section. Default is 'unity'.</param>
        /// <returns>This <see cref="UnityManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public UnityManagerBuilder SetConfigurationFile(String configurationFileName, String configurationSectionName = "unity")
        {
            _ConfigurationFileName = String.IsNullOrWhiteSpace(configurationFileName)
                                         ? String.Empty
                                         : configurationFileName.Trim();

            _ConfigurationSectionName = String.IsNullOrWhiteSpace(configurationSectionName)
                                            ? "unity"
                                            : configurationSectionName.Trim();

            return this;
        }

        /// <summary>
        /// Sets the application's dependency injection manager using 
        /// the configuration created from this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageUnity>(
                   () => 
                       new UnityManager(
                           new UnityConfiguration(_ContainerName, _ConfigurationFileName, _ConfigurationSectionName)));
        }
    }
}