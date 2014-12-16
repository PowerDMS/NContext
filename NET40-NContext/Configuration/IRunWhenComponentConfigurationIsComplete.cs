namespace NContext.Configuration
{
    using System.ComponentModel.Composition;

    /// <summary>
    /// Defines a role-interface which allows implementors to run 
    /// when <see cref="IApplicationComponent.Configure"/> has completed.
    /// </summary>
    /// <remarks></remarks>
    [InheritedExport]
    public interface IRunWhenComponentConfigurationIsComplete
    {
        /// <summary>
        /// Runs when a <see cref="IApplicationComponent.Configure"/> has completed.
        /// </summary>
        /// <param name="applicationComponent">The application component.</param>
        /// <remarks></remarks>
        void Run(IApplicationComponent applicationComponent);
    }
}