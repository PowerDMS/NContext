namespace NContext.Extensions.Unity.Configuration
{
    using System;

    /// <summary>
    /// Defines configuration settings for Unity.
    /// </summary>
    public class UnityConfiguration
    {
        private readonly String _ContainerName;

        private readonly String _ConfigurationFileName;

        private readonly String _ConfigurationSectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityConfiguration"/> class.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="configurationFileName">Name of the configuration file.</param>
        /// <param name="configurationSectionName">Name of the configuration section.</param>
        /// <remarks></remarks>
        public UnityConfiguration(String containerName, String configurationFileName, String configurationSectionName)
        {
            _ContainerName = containerName;
            _ConfigurationFileName = configurationFileName;
            _ConfigurationSectionName = configurationSectionName;
        }

        /// <summary>
        /// Gets the dependency injection container adapter.
        /// </summary>
        /// <remarks></remarks>
        public String ContainerName
        {
            get
            {
                return _ContainerName;
            }
        }

        /// <summary>
        /// Gets the name of the configuration file.
        /// </summary>
        /// <remarks></remarks>
        public String ConfigurationFileName
        {
            get
            {
                return _ConfigurationFileName;
            }
        }

        /// <summary>
        /// Gets the name of the configuration section.
        /// </summary>
        /// <remarks></remarks>
        public String ConfigurationSectionName
        {
            get
            {
                return _ConfigurationSectionName;
            }
        }
    }
}