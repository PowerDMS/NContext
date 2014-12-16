namespace NContext.Configuration
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Defines a role-interface which allows implementors to run 
    /// when <see cref="ApplicationConfigurationBase.Setup"/> has completed.
    /// </summary>
    /// <remarks></remarks>
    [InheritedExport]
    public interface IRunWhenApplicationConfigurationIsComplete
    {
        /// <summary>
        /// Gets the priority in which each implementation will run. Implementations will be run 
        /// in ascending order based on priority, so a lower priority value will execute first.
        /// </summary>
        /// <remarks></remarks>
        Int32 Priority { get; }

        /// <summary>
        /// Runs when the <see cref="ApplicationConfigurationBase.Setup"/> has completed.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        void Run(ApplicationConfigurationBase applicationConfiguration);
    }
}