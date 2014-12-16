namespace NContext.Extensions.Unity.Configuration
{
    using System;
    using System.ComponentModel.Composition;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Defines a role-interface which encapsulates logic to configure an <see cref="IUnityContainer"/>.
    /// </summary>
    [InheritedExport(typeof(IConfigureAUnityContainer))]
    public interface IConfigureAUnityContainer
    {
        /// <summary>
        /// Gets the priority in which to configure the container. Implementations will be run 
        /// in ascending order based on priority, so a lower priority value will execute first.
        /// </summary>
        /// <remarks></remarks>
        Int32 Priority { get; }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/> dependency injection container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <remarks></remarks>
        void ConfigureContainer(IUnityContainer container);
    }
}