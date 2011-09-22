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
        /// Creates an instance of <see cref="IUnityContainer"/> container used for dependency injection.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="configurationFileName">Name of the configuration file.</param>
        /// <param name="configurationSectionName">Name of the configuration section.</param>
        /// <returns>Instance of <see cref="IUnityContainer"/>.</returns>
        /// <remarks></remarks>
         public static IUnityContainer Create(String containerName, String configurationFileName, String configurationSectionName = "unity")
         {
             var fileMap = new ExeConfigurationFileMap
             {
                 ExeConfigFilename =
                     new Uri(
                        String.Format(
                            @"{0}\{1}",
                            Path.GetDirectoryName(
                                new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath), 
                                configurationFileName)).LocalPath
             };

             try
             {
                 var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                 var unitySection = configuration.GetSection(configurationSectionName) as UnityConfigurationSection;
                 if (unitySection == null)
                 {
                     throw new Exception("Unity container could not be configured.");
                 }

                 var container = new UnityContainer().LoadConfiguration(unitySection, containerName ?? String.Empty);
                 container.AddNewExtension<EnterpriseLibraryCoreExtension>()
                          .AddNewExtension<Interception>()
                          .RegisterType<InterfaceInterceptor>()
                          .RegisterType<TransparentProxyInterceptor>()
                          .RegisterType<VirtualMethodInterceptor>()
                          .RegisterType<IAuthorizationProviderInstrumentationProvider, NullAuthorizationProviderInstrumentationProvider>(new ContainerControlledLifetimeManager());


                 return container;
             }
             catch
             {
                 // TODO: (DG) Logging could not load unity configuration
                 throw;
             }
         }
    }
}