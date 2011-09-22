// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositionContainerExtensions.cs">
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
//   Defines extension methods for CompositionContainer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition.Hosting;

namespace NContext.Application.Extensions
{
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
    }
}