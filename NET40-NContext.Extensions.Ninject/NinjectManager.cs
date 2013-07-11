// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectManager.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.Ninject
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using Microsoft.Practices.ServiceLocation;

    using NContext.Configuration;

    using global::Ninject;

    using NinjectAdapter;

    /// <summary>
    /// Defines a dependency injection application component using Ninject.
    /// </summary>
    public class NinjectManager : IManageNinject
    {
        private readonly NinjectConfiguration _Configuration;

        private Boolean _IsConfigured;

        private IKernel _Kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectManager"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <remarks></remarks>
        public NinjectManager(NinjectConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _Configuration = configuration;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <remarks></remarks>
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
        /// Gets the <see cref="IKernel"/> instance.
        /// </summary>
        /// <remarks></remarks>
        public IKernel Kernel
        {
            get
            {
                return _Kernel;
            }
        }

        /// <summary>
        /// Configures the component instance. This method should set <see cref="IApplicationComponent.IsConfigured"/>.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks>
        /// </remarks>
        public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            _Kernel = _Configuration.CreateKernel();

            var serviceLocator = new NinjectServiceLocator(_Kernel);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IKernel>(_Kernel);
            applicationConfiguration.CompositionContainer.ComposeExportedValue<IManageNinject>(this);
            _Kernel.Bind<CompositionContainer>().ToConstant(applicationConfiguration.CompositionContainer).InSingletonScope();

            applicationConfiguration.CompositionContainer
                                    .GetExportedValues<IConfigureANinjectKernel>()
                                    .OrderBy(configurable => configurable.Priority)
                                    .ForEach(configurable => configurable.ConfigureKernel(_Kernel));

            _IsConfigured = true;
        }
    }
}