// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecureDirectoryCatalog.cs">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
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