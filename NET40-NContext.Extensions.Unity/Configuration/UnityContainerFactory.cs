namespace NContext.Extensions.Unity.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// Defines a simple factory for creating a <see cref="IUnityContainer"/>
    /// </summary>
    public static class UnityContainerFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IUnityContainer"/> used for dependency injection.
        /// </summary>
        /// <returns>Instance of <see cref="IUnityContainer"/>.</returns>
        /// <remarks></remarks>
        public static IUnityContainer Create()
        {
            return Create(null, null);
        }

        /// <summary>
        /// Creates an instance of <see cref="IUnityContainer"/> used for dependency injection.
        /// </summary>
        /// <param name="configurationFileName">
        /// Name of the configuration file.
        /// </param>
        /// <param name="containerName">
        /// Name of the container.
        /// </param>
        /// <param name="configurationSectionName">
        /// Name of the configuration section.
        /// </param>
        /// <returns>
        /// Instance of <see cref="IUnityContainer"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static IUnityContainer Create(String configurationFileName, String containerName = "", String configurationSectionName = "unity")
        {
            IUnityContainer container = null;
            if (!String.IsNullOrWhiteSpace(configurationFileName))
            {
                var filePath = String.Format(
                                @"{0}\{1}",
                                Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath),
                                configurationFileName);

                var fileMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = new Uri(filePath).LocalPath
                };

                try
                {
                    var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    var unitySection = configuration.GetSection(configurationSectionName) as UnityConfigurationSection;
                    if (unitySection == null)
                    {
                        throw new ConfigurationErrorsException();
                    }

                    container = new UnityContainer().LoadConfiguration(unitySection, containerName ?? String.Empty);
                }
                catch (ConfigurationErrorsException errorsException)
                {
                    throw new Exception("Unity container configuration section could not be loaded.", errorsException);
                }
            }

            if (container == null)
            {
                container = new UnityContainer();
            }
            
            return container;
        }
    }
}