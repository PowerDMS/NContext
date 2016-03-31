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