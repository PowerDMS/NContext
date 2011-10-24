// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityContainerFactory.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines a simple factory for creating a <see
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.IO;
using System.Reflection;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace NContext.Unity
{
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
        /// <param name="configurationFileName">Name of the configuration file.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>Instance of <see cref="IUnityContainer"/>.</returns>
        /// <remarks></remarks>
        public static IUnityContainer Create(String configurationFileName, String containerName)
        {
            return Create(configurationFileName, "unity", containerName);
        }

        /// <summary>
        /// Creates an instance of <see cref="IUnityContainer"/> used for dependency injection.
        /// </summary>
        /// <param name="configurationFileName">Name of the configuration file.</param>
        /// <param name="configurationSectionName">Name of the configuration section.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>Instance of <see cref="IUnityContainer"/>.</returns>
        /// <remarks></remarks>
        public static IUnityContainer Create(String configurationFileName = null, String configurationSectionName = "unity", String containerName = "")
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

            container.AddNewExtension<EnterpriseLibraryCoreExtension>()
                     .AddNewExtension<Interception>()
                     .RegisterType<InterfaceInterceptor>()
                     .RegisterType<TransparentProxyInterceptor>()
                     .RegisterType<VirtualMethodInterceptor>()
                     .RegisterType<IAuthorizationProviderInstrumentationProvider, NullAuthorizationProviderInstrumentationProvider>(new ContainerControlledLifetimeManager());


            return container;
        }
    }
}