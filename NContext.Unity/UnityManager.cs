// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityManager.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Implementation of IUnityManager interface for management of a Unity container.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using NContext.Application.EnterpriseLibrary;
using NContext.Application.Extensions;
using NContext.Application.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace NContext.Unity
{
    /// <summary>
    /// Implementation of <see cref="IUnityManager"/> interface for management of a <see cref="IUnityContainer"/>.
    /// </summary>
    public class UnityManager : IUnityManager
    {
        #region Fields

        private static Boolean _IsConfigured;

        private static IUnityContainer _Container;

        private readonly UnityConfiguration _UnityConfigurationConfiguration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityManager"/> class.
        /// </summary>
        /// <param name="unityConfigurationConfiguration">The dependency injection configuration.</param>
        /// <remarks></remarks>
        public UnityManager(UnityConfiguration unityConfigurationConfiguration)
        {
            if (unityConfigurationConfiguration == null)
            {
                throw new ArgumentNullException("unityConfigurationConfiguration");
            }

            _UnityConfigurationConfiguration = unityConfigurationConfiguration;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the unity container.
        /// </summary>
        public IUnityContainer Container
        {
            get { return _Container; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsConfigured
        {
            get { return _IsConfigured; }
        }

        #endregion
        
        #region Implementation of IApplicationComponent

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public void Configure(IApplicationConfiguration applicationConfiguration)
        {
            if (!_IsConfigured)
            {
                _Container = UnityContainerFactory.Create(_UnityConfigurationConfiguration.ContainerName, 
                                                          _UnityConfigurationConfiguration.ConfigurationFileName);

                SetServiceLocator();
                SetContainerConfigurator(applicationConfiguration);
                
                applicationConfiguration.CompositionContainer.ComposeExportedValue<IUnityContainer>(_Container);
                _Container.RegisterInstance<CompositionContainer>(applicationConfiguration.CompositionContainer);
                _Container.RegisterInstance<IUnityManager>(this);

                applicationConfiguration.CompositionContainer
                                        .GetExports<IConfigureAUnityContainer>()
                                        .ForEach(configurable =>
                                                 configurable.Value.ConfigureContainer(_Container));

                applicationConfiguration.CompositionContainer
                                        .GetExportTypesThatImplement<IRegisterWithAUnityContainer>()
                                        .ForEach(type => _Container.RegisterType(type));
                
                _IsConfigured = true;
            }
        }

        private void SetServiceLocator()
        {
            var serviceLocator = new UnityServiceLocator(_Container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
            EnterpriseLibraryContainer.Current = ServiceLocator.Current;
        }

        private void SetContainerConfigurator(IApplicationConfiguration applicationConfiguration)
        {
            var elManager = applicationConfiguration.GetComponent<EnterpriseLibraryManager>();
            if (elManager != null)
            {
                elManager.ContainerConfigurator = new UnityContainerConfigurator(_Container);
            }
        }

        #endregion
    }
}