﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityManager.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.Unity
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    using NContext.Configuration;

    /// <summary>
    /// Implementation of <see cref="IManageUnity"/> interface for management of a <see cref="IUnityContainer"/>.
    /// </summary>
    public class UnityManager : IManageUnity
    {
        private readonly UnityConfiguration _UnityConfiguration;

        private Boolean _IsConfigured;

        private IUnityContainer _Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityManager"/> class.
        /// </summary>
        /// <param name="unityConfiguration">The dependency injection configuration.</param>
        /// <remarks></remarks>
        public UnityManager(UnityConfiguration unityConfiguration)
        {
            if (unityConfiguration == null)
            {
                throw new ArgumentNullException("unityConfiguration");
            }

            _UnityConfiguration = unityConfiguration;
        }

        /// <summary>
        /// Gets the unity container.
        /// </summary>
        public IUnityContainer Container
        {
            get
            {
                return _Container;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
            protected set
            {
                _IsConfigured = value;
            }
        }

        /// <summary>
        /// Configures the component instance.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            _Container = UnityContainerFactory.Create(
                _UnityConfiguration.ConfigurationFileName,
                _UnityConfiguration.ConfigurationSectionName,
                _UnityConfiguration.ContainerName);

            SetServiceLocator();
                
            applicationConfiguration.CompositionContainer.ComposeExportedValue<IUnityContainer>(_Container);
            _Container.RegisterInstance<CompositionContainer>(applicationConfiguration.CompositionContainer);

            applicationConfiguration.CompositionContainer
                .GetExportedValues<IConfigureAUnityContainer>()
                .OrderBy(configurable => configurable.Priority)
                .ForEach(configurable => configurable.ConfigureContainer(_Container));
                
            _IsConfigured = true;
        }

        protected void SetServiceLocator()
        {
            var serviceLocator = new UnityServiceLocator(_Container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }
    }
}