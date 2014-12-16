namespace NContext.Extensions.Ninject.Configuration
{
    using NContext.Configuration;

    using global::Ninject;

    /// <summary>
    /// Defines a dependency injection application component using Ninject.
    /// </summary>
    public interface IManageNinject : IApplicationComponent
    {
        /// <summary>
        /// Gets the <see cref="IKernel"/> instance.
        /// </summary>
        /// <remarks></remarks>
        IKernel Kernel { get; }
    }
}