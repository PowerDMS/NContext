// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityContainerFactory.cs">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
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

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace NContext.Extensions.Unity
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
        /// <param name="configurationSectionName">Name of the configuration section.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>Instance of <see cref="IUnityContainer"/>.</returns>
        /// <remarks></remarks>
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

            container.AddNewExtension<Interception>()
                     .RegisterType<InterfaceInterceptor>()
                     .RegisterType<TransparentProxyInterceptor>()
                     .RegisterType<VirtualMethodInterceptor>();
            
            return container;
        }
    }
}