// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectConfiguration.cs">
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
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using NContext.Configuration;

using Ninject;
using Ninject.Modules;

namespace NContext.Extensions.Ninject
{
    /// <summary>
    /// 
    /// </summary>
    public class NinjectConfiguration : ApplicationComponentConfigurationBase
    {
        #region Fields

        private Func<IKernel> _KernelFactory; 

        private Func<IEnumerable<INinjectModule>> _ModuleFactory;

        private Func<INinjectSettings> _NinjectSettings;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public NinjectConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="IKernel"/>.
        /// </summary>
        /// <remarks></remarks>
        public virtual IKernel Kernel
        {
            get
            {
                return _KernelFactory == null
                           ? new StandardKernel(NinjectSettings, Modules.ToArray())
                           : _KernelFactory.Invoke();
            }
        }

        /// <summary>
        /// Gets the <see cref="INinjectModule"/> collection.
        /// </summary>
        /// <remarks></remarks>
        public virtual IEnumerable<INinjectModule> Modules
        {
            get
            {
                return _ModuleFactory == null 
                           ? Enumerable.Empty<INinjectModule>() 
                           : _ModuleFactory.Invoke();
            }
        }

        /// <summary>
        /// Gets the <see cref="INinjectSettings"/> instance.
        /// </summary>
        /// <remarks></remarks>
        public virtual INinjectSettings NinjectSettings
        {
            get
            {
                return _NinjectSettings == null
                    ? new NinjectSettings()
                    : _NinjectSettings.Invoke();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the <see cref="IKernel"/> instance to use.  By default, NContext will use <see cref="StandardKernel"/>.  
        /// You may use this method to supply a custom <see cref="IKernel"/>, however, you cannot 
        /// use <see cref="SetModules"/> or <see cref="SetSettings"/> in conjunction.  Therefore, the 
        /// specified <param name="kernelFactory"></param> should supply the kernel instance with any 
        /// required <see cref="INinjectSettings"/> and/or <see cref="INinjectModule"/>s.
        /// </summary>
        /// <param name="kernelFactory">The <see cref="IKernel"/> factory.</param>
        /// <returns>This <see cref="NinjectConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public NinjectConfiguration SetKernel(Func<IKernel> kernelFactory)
        {
            _KernelFactory = kernelFactory;

            return this;
        }

        /// <summary>
        /// Sets the modules to load into the <see cref="IKernel"/>.
        /// </summary>
        /// <param name="moduleFactory">The module factory.</param>
        /// <returns>This <see cref="NinjectConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public NinjectConfiguration SetModules(Func<IEnumerable<INinjectModule>> moduleFactory)
        {
            _ModuleFactory = moduleFactory;

            return this;
        }

        /// <summary>
        /// Sets the <see cref="INinjectSettings"/> used by the <see cref="IKernel"/>.
        /// </summary>
        /// <param name="settingsFactory">The <see cref="INinjectSettings"/> instance.</param>
        /// <returns>This <see cref="NinjectConfiguration"/> instance.</returns>
        /// <remarks></remarks>
        public NinjectConfiguration SetSettings(Func<INinjectSettings> settingsFactory)
        {
            _NinjectSettings = settingsFactory;

            return this;
        }

        #endregion

        #region Overrides of ApplicationComponentConfigurationBase

        /// <summary>
        /// Register's an <see cref="IManageNinject"/> application component instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageNinject>(() => new NinjectManager(this));
        }

        #endregion
    }
}