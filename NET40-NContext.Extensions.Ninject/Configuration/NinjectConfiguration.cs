// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectConfiguration.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.Ninject.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Ninject;
    using global::Ninject.Modules;

    /// <summary>
    /// Defines configuration settings for Ninject.
    /// </summary>
    public class NinjectConfiguration
    {
        private readonly Func<IKernel> _KernelFactory;

        private readonly Func<IEnumerable<INinjectModule>> _ModuleFactory;

        private readonly Func<INinjectSettings> _NinjectSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectConfiguration"/> class.
        /// </summary>
        /// <param name="kernelFactory">The kernel factory.</param>
        /// <param name="moduleFactory">The module factory.</param>
        /// <param name="ninjectSettings">The ninject settings.</param>
        /// <remarks></remarks>
        public NinjectConfiguration(Func<IKernel> kernelFactory, Func<IEnumerable<INinjectModule>> moduleFactory, Func<INinjectSettings> ninjectSettings)
        {
            _KernelFactory = kernelFactory;
            _ModuleFactory = moduleFactory;
            _NinjectSettings = ninjectSettings;
        }

        /// <summary>
        /// Creates the <see cref="IKernel"/> instance.
        /// </summary>
        /// <returns>Instance of <see cref="IKernel"/>.</returns>
        /// <remarks></remarks>
        public virtual IKernel CreateKernel()
        {
            return _KernelFactory == null
                              ? new StandardKernel(GetSettings(), GetModules().ToArray())
                              : _KernelFactory.Invoke();
        }

        private IEnumerable<INinjectModule> GetModules()
        {
            return _ModuleFactory == null
                           ? Enumerable.Empty<INinjectModule>()
                           : _ModuleFactory.Invoke();
        }

        private INinjectSettings GetSettings()
        {
            return _NinjectSettings == null
                    ? new NinjectSettings()
                    : _NinjectSettings.Invoke();
        }
    }
}