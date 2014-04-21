// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfiguration.cs" company="Waking Venture, Inc.">
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