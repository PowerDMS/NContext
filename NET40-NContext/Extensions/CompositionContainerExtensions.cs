// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositionContainerExtensions.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    /// <summary>
    /// Defines extension methods for <see cref="CompositionContainer"/>.
    /// </summary>
    public static class CompositionContainerExtensions
    {
        /// <summary>
        /// Gets all types within the <see cref="CompositionContainer"/>'s <see cref="CompositionContainer.Catalog"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>Enumeration of <see cref="Type"/>s.</returns>
        /// <remarks></remarks>
        public static IEnumerable<Type> GetExportTypes(this CompositionContainer container)
        {
            return container.Catalog.Parts
                            .Select(part => part.GetType().GetMethod("GetLazyPartType").Invoke(part, null))
                            .OfType<Lazy<Type>>()
                            .Select(lazyPart => lazyPart.Value);
        }

        /// <summary>
        /// Gets all types within the <see cref="CompositionContainer"/>'s <see cref="CompositionContainer.Catalog"/> 
        /// which implement the specified <typeparamref name="TExport"/>.
        /// </summary>
        /// <typeparam name="TExport">The type of the export.</typeparam>
        /// <param name="container">The container.</param>
        /// <returns>Enumeration of derived / implementing <see cref="Type"/>s.</returns>
        /// <remarks></remarks>
        public static IEnumerable<Type> GetExportTypesThatImplement<TExport>(this CompositionContainer container)
        {
            return container.GetExportTypes()
                            .Where(typePart => typePart.Implements<TExport>());
        }

        /*
        public static IEnumerable<Type> GetExportedTypes<T>(this CompositionContainer container)
        {
            return container.Catalog
                            .Parts
                            .Select(part => ComposablePartExportType<T>(part))
                            .Where(t => t != null);
        }

        private static Type ComposablePartExportType<T>(ComposablePartDefinition part)
        {
            if (part.ExportDefinitions.Any(def => def.Metadata.ContainsKey("ExportTypeIdentity") &&
                def.Metadata["ExportTypeIdentity"].Equals(typeof(T).FullName)))
            {
                return ReflectionModelServices.GetPartType(part).Value;
            }

            return null;
        }
        */
    }
}