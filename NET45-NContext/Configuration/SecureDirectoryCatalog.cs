namespace NContext.Configuration
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines a MEF catalog which prevents dll hijacking.
    /// </summary>
    /// <remarks></remarks>
    // TODO: (DG) NOT-FINISHED - Create SecureDirectoryCatalog
    public class SecureDirectoryCatalog : ComposablePartCatalog
    {
        private readonly AggregateCatalog _Catalog;

        public SecureDirectoryCatalog(String directory, String searchPattern, Predicate<AssemblyName> isAuthorized)
        {
            _Catalog = new AggregateCatalog();
            var files = Directory.EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(file);

                    // Force MEF to load the plugin and figure out if there are any exports
                    // good assemblies will not throw the RTLE exception and can be added to the catalog
                    if (assemblyCatalog.Parts.ToList().Count > 0)
                    {
                        _Catalog.Catalogs.Add(assemblyCatalog);
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                }
            }
        }

        /// <summary>
        /// Gets the part definitions that are contained in the catalog.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartDefinition"/> contained in the <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartCatalog"/>.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.ComponentModel.Composition.Primitives.ComposablePartCatalog"/> object has been disposed of.</exception>
        public override IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return _Catalog.Parts;
            }
        }
    }
}