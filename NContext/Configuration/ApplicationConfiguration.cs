// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfiguration.cs">
//   Copyright (c) 2012
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
//   Defines a class for application configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace NContext.Configuration
{
    /// <summary>
    /// Defines a class for application configuration.
    /// </summary>
    public class ApplicationConfiguration : ApplicationConfigurationBase
    {
        #region Methods
        
        /// <summary>
        /// Creates the composition container from the executing assembly.
        /// </summary>
        /// <returns>
        /// Application's <see cref="CompositionContainer"/> instance.
        /// </returns>
        protected override CompositionContainer CreateCompositionContainer(HashSet<String> compositionDirectories, HashSet<Predicate<String>> compositionFileNameConstraints)
        {
            if (IsConfigured)
            {
                return CompositionContainer;
            }

            var directoryCatalog = new SafeDirectoryCatalog(compositionDirectories, compositionFileNameConstraints);
            var compositionContainer = new CompositionContainer(directoryCatalog);
            if (compositionContainer == null)
            {
                throw new Exception("Could not create composition container. Container cannot be null.");
            }

            return compositionContainer;
        }

        #endregion
    }
}