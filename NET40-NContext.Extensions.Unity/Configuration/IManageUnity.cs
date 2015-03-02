namespace NContext.Extensions.Unity.Configuration
{
    using Microsoft.Practices.Unity;

    using NContext.Configuration;

    /// <summary>
    /// Defines a dependency injection application component using Unity.
    /// </summary>
    public interface IManageUnity : IApplicationComponent
    {
        /// <summary>
        /// Gets the <see cref="IUnityContainer"/> instance.
        /// </summary>
        IUnityContainer Container { get; }
    }
}