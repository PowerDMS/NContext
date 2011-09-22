// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityConfiguration.cs">
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
//   Defines a configuration class to build the application's UnityManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Application.Configuration;

namespace NContext.Unity
{
    /// <summary>
    /// Defines a configuration class to build the application's <see cref="UnityManager"/>.
    /// </summary>
    /// <remarks></remarks>
    public class UnityConfiguration : ApplicationComponentConfigurationBase
    {
        #region Fields

        private String _ContainerName;

        private String _ConfigurationFileName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityConfiguration"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public UnityConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder)
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

        #endregion

        #region Methods

        /// <summary>
        /// Sets the name of the container.
        /// </summary>
        /// <param name="unityContainerName">Name of the unity container.</param>
        /// <returns>This <see cref="UnityConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public UnityConfiguration SetContainerName(String unityContainerName)
        {
            _ContainerName = unityContainerName;

            return this;
        }

        /// <summary>
        /// Sets the name of the configuration file.
        /// </summary>
        /// <param name="unityConfigurationFileName">Name of the unity configuration file.</param>
        /// <returns>This <see cref="UnityConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public UnityConfiguration SetConfigurationFileName(String unityConfigurationFileName)
        {
            _ConfigurationFileName = String.IsNullOrWhiteSpace(unityConfigurationFileName) 
                ? "unity.config" 
                : unityConfigurationFileName.Trim();

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
                   .RegisterComponent<IUnityManager>(() => new UnityManager(this));
        }

        #endregion
    }
}