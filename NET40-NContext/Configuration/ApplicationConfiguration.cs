namespace NContext.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;

    /// <summary>
    /// Defines a default implementation for application configuration.
    /// </summary>
    public class ApplicationConfiguration : ApplicationConfigurationBase
    {
        /// <summary>
        /// Creates the composition container from the executing assembly.
        /// </summary>
        /// <param name="compositionDirectories">The composition directories.</param>
        /// <param name="fileInfoConstraints">The composition file info constraints.</param>
        /// <returns>Application's <see cref="CompositionContainer" /> instance.</returns>
        /// <exception cref="System.Exception"></exception>
        protected override CompositionContainer CreateCompositionContainer(ISet<String> compositionDirectories, ISet<Predicate<FileInfo>> fileInfoConstraints)
        {
            if (IsConfigured)
            {
                return CompositionContainer;
            }

            var directoryCatalog = new SafeDirectoryCatalog(compositionDirectories, fileInfoConstraints);
            var compositionContainer = new CompositionContainer(directoryCatalog);
            if (compositionContainer == null)
            {
                throw new Exception("Could not create composition container. Container cannot be null.");
            }

            return compositionContainer;
        }
    }
}