// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityConfigurationBuilder.cs">
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
//   Defines a configuration class to build the application's UnityManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Configuration;

namespace NContext.Extensions.Unity
{
    /// <summary>
    /// Defines a configuration class to build the application's <see cref="UnityManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class UnityConfigurationBuilder : ApplicationComponentConfigurationBase
    {
        #region Fields

        private String _ContainerName;

        private String _ConfigurationFileName;

        private String _ConfigurationSectionName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public UnityConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        /// <summary>
        /// Sets the name of the container.
        /// </summary>
        /// <param name="unityContainerName">Name of the unity container.</param>
        /// <returns>This <see cref="UnityConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public UnityConfigurationBuilder SetContainerName(String unityContainerName)
        {
            _ContainerName = unityContainerName;

            return this;
        }

        /// <summary>
        /// Sets the unity configuration file.
        /// </summary>
        /// <param name="configurationFileName">Name of the unity configuration file.</param>
        /// <param name="configurationSectionName">Name of the configuration section. Default is 'unity'.</param>
        /// <returns>This <see cref="UnityConfigurationBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public UnityConfigurationBuilder SetConfigurationFile(String configurationFileName, String configurationSectionName = "unity")
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
                   .RegisterComponent<IManageUnity>(() => new UnityManager(this));
        }

        #endregion
    }
}