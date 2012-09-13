// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityConfiguration.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.Unity
{
    using System;

    /// <summary>
    /// Defines configuration settings for Unity.
    /// </summary>
    public class UnityConfiguration
    {
        private readonly String _ContainerName;

        private readonly String _ConfigurationFileName;

        private readonly String _ConfigurationSectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityConfiguration"/> class.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="configurationFileName">Name of the configuration file.</param>
        /// <param name="configurationSectionName">Name of the configuration section.</param>
        /// <remarks></remarks>
        public UnityConfiguration(String containerName, String configurationFileName, String configurationSectionName)
        {
            _ContainerName = containerName;
            _ConfigurationFileName = configurationFileName;
            _ConfigurationSectionName = configurationSectionName;
        }

        /// <summary>
        /// Gets the dependency injection container adapter.
        /// </summary>
        /// <remarks></remarks>
        public String ContainerName
        {
            get
            {
                return _ContainerName;
            }
        }

        /// <summary>
        /// Gets the name of the configuration file.
        /// </summary>
        /// <remarks></remarks>
        public String ConfigurationFileName
        {
            get
            {
                return _ConfigurationFileName;
            }
        }

        /// <summary>
        /// Gets the name of the configuration section.
        /// </summary>
        /// <remarks></remarks>
        public String ConfigurationSectionName
        {
            get
            {
                return _ConfigurationSectionName;
            }
        }
    }
}