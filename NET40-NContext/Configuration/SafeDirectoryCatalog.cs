// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SafeDirectoryCatalog.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

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