namespace NContext.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines a MEF catalog which prevents exceptions from being thrown when an assembly cannot be added.
    /// </summary>
    public class SafeDirectoryCatalog : ComposablePartCatalog
    {
        private readonly AggregateCatalog _Catalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeDirectoryCatalog"/> class.
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="fileInfoConstraints">The file info constraints.</param>
        /// <remarks></remarks>
        public SafeDirectoryCatalog(IEnumerable<String> directories, IEnumerable<Predicate<FileInfo>> fileInfoConstraints)
        {
            var assemblyDirectories = directories.ToList();
            if (!assemblyDirectories.All(Directory.Exists))
            {
                throw new DirectoryNotFoundException("Invalid composition directory path specified. Could not create AggregateCatalog.");
            }

            _Catalog = new AggregateCatalog();
            var files = assemblyDirectories.SelectMany(
                directory => Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
                    .Select(filePath => new FileInfo(filePath))
                    .Where(fileInfo => fileInfoConstraints.Any(predicate => predicate(fileInfo)))).Distinct();

            foreach (var file in files)
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(file.FullName);
                    if (assemblyCatalog.Parts.Any())
                    {
                        _Catalog.Catalogs.Add(assemblyCatalog);
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    // Ignore: could not load the assembly.
                }
                catch (TargetInvocationException)
                {
                    // Ignore: probably tried to create an AssemblyCatalog with a non-dotNet binary.
                }
                catch (Exception)
                {
                    // Ignore exception
                }
            }
        }

        /// <summary>
        /// Gets the part definitions that are contained in the catalog.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/> 
        /// contained in the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartCatalog"/>.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartCatalog"/> object has been disposed of.
        /// </exception>
        public override IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return _Catalog.Parts;
            }
        }
    }
}