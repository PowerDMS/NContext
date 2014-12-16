namespace NContext.Extensions.Ninject.Configuration
{
    using System;
    using System.ComponentModel.Composition;

    using global::Ninject;

    /// <summary>
    /// Defines a role-interface for Ninject's IKernel configuration.
    /// </summary>
    [InheritedExport]
    public interface IConfigureANinjectKernel
    {
        /// <summary>
        /// Configures the <see cref="IKernel"/> instance.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <remarks></remarks>
        void ConfigureKernel(IKernel kernel);

        /// <summary>
        /// Gets the priority used to configure the <see cref="IKernel"/>. Lower 
        /// priority will cause implementations to execute first.
        /// </summary>
        Int32 Priority { get; }
    }
}