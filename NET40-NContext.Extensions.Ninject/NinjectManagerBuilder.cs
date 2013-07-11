// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectManagerBuilder.cs" company="Waking Venture, Inc.">
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
    using System.Collections.Generic;

    using NContext.Configuration;

    using global::Ninject;
    using global::Ninject.Modules;

    public class NinjectManagerBuilder : ApplicationComponentConfigurationBuilderBase
    {
        private Func<IKernel> _KernelFactory; 

        private Func<IEnumerable<INinjectModule>> _ModuleFactory;

        private Func<INinjectSettings> _NinjectSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationComponentConfigurationBuilderBase"/> class.
        /// </summary>
        /// <param name="applicationConfigurationBuilder">The application configuration.</param>
        /// <remarks></remarks>
        public NinjectManagerBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
            : base(applicationConfigurationBuilder)
        {
            _KernelFactory = () => new StandardKernel();
        }

        /// <summary>
        /// Sets the <see cref="IKernel"/> instance to use.  By default, NContext will use <see cref="StandardKernel"/>.  
        /// You may use this method to supply a custom <see cref="IKernel"/>, however, you cannot 
        /// use <seealso cref="SetModules"/> or <seealso cref="SetSettings"/> in conjunction.  Therefore, the 
        /// specified <paramref name="kernelFactory"/> should supply the kernel instance with any 
        /// required <seealso cref="INinjectSettings"/> and/or <seealso cref="INinjectModule"/>s.
        /// </summary>
        /// <param name="kernelFactory">The <see cref="IKernel"/> factory.</param>
        /// <returns>This <see cref="NinjectManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public NinjectManagerBuilder SetKernel(Func<IKernel> kernelFactory)
        {
            _KernelFactory = kernelFactory;

            return this;
        }

        /// <summary>
        /// Sets the modules to load into the <see cref="IKernel"/>.
        /// </summary>
        /// <param name="moduleFactory">The module factory.</param>
        /// <returns>This <see cref="NinjectManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public NinjectManagerBuilder SetModules(Func<IEnumerable<INinjectModule>> moduleFactory)
        {
            _ModuleFactory = moduleFactory;

            return this;
        }

        /// <summary>
        /// Sets the <see cref="INinjectSettings"/> used by the <see cref="IKernel"/>.
        /// </summary>
        /// <param name="settingsFactory">The <see cref="INinjectSettings"/> instance.</param>
        /// <returns>This <see cref="NinjectManagerBuilder"/> instance.</returns>
        /// <remarks></remarks>
        public NinjectManagerBuilder SetSettings(Func<INinjectSettings> settingsFactory)
        {
            _NinjectSettings = settingsFactory;

            return this;
        }

        /// <summary>
        /// Register's an <see cref="IManageNinject"/> application component instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Setup()
        {
            Builder.ApplicationConfiguration
                   .RegisterComponent<IManageNinject>(
                   () => 
                       new NinjectManager(
                           new NinjectConfiguration(_KernelFactory, _ModuleFactory, _NinjectSettings)));
        }
    }
}