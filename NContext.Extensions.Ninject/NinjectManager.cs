// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectManager.cs">
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
//   Defines a dependency injection application component using Ninject.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

using Microsoft.Practices.ServiceLocation;

using NContext.Configuration;

using Ninject;

using NinjectAdapter;

namespace NContext.Extensions.Ninject
{
    /// <summary>
    /// Defines a dependency injection application component using Ninject.
    /// </summary>
    public class NinjectManager : IManageNinject
    {
        #region Fields

        private readonly NinjectConfigurationBuilder _Configuration;

        private Boolean _IsConfigured;

        private IKernel _Kernel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectManager"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <remarks></remarks>
        public NinjectManager(NinjectConfigurationBuilder configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _Configuration = configuration;
        }

        #endregion

        #region Properties

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

        #endregion

        #region Implementation of IApplicationComponent
        
        /// <summary>
        /// Configures the component instance. This method should set <see cref="IApplicationComponent.IsConfigured"/>.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks>
        /// </remarks>
        public virtual void Configure(IApplicationConfiguration applicationConfiguration)
        {
            if (_IsConfigured)
            {
                return;
            }

            _Kernel = _Configuration.Kernel;
            SetServiceLocator();

            applicationConfiguration.CompositionContainer.ComposeExportedValue<IKernel>(_Kernel);
            _Kernel.Bind<CompositionContainer>().ToConstant(applicationConfiguration.CompositionContainer);

            applicationConfiguration.CompositionContainer
                                    .GetExportedValues<IConfigureANinjectKernel>()
                                    .OrderBy(configurable => configurable.Priority)
                                    .ForEach(configurable => configurable.ConfigureKernel(_Kernel));

            _IsConfigured = true;
        }

        /// <summary>
        /// Sets the service locator provider.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void SetServiceLocator()
        {
            var serviceLocator = new NinjectServiceLocator(_Kernel);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        #endregion
    }
}