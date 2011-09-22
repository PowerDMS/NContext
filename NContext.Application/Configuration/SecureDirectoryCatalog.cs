// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecureDirectoryCatalog.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines a MEF catalog which prevents dll hijacking.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NContext.Application.Configuration
{
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