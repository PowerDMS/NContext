namespace NContext.Extensions.Ninject.Configuration
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