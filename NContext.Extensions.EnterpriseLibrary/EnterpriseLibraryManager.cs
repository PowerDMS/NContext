// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnterpriseLibraryManager.cs">
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
//   Defines an application component for Microsoft Enterprise Library support.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

using NContext.Configuration;

namespace NContext.Extensions.EnterpriseLibrary
{
    /// <summary>
    /// Defines an implementation of IEnterpriseLibraryManager
    /// </summary>
    /// <remarks></remarks>
    public class EnterpriseLibraryManager : IEnterpriseLibraryManager
    {
        private Boolean _IsConfigured;

        private IContainerConfigurator _ContainerConfigurator;

        /// <summary>
        /// Gets the application's container configurator.
        /// </summary>
        /// <remarks></remarks>
        public IContainerConfigurator ContainerConfigurator
        {
            get
            {
                return _ContainerConfigurator;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsConfigured
        {
            get
            {
                return _IsConfigured;
            }
        }

        /// <summary>
        /// Sets the application's container configurator.
        /// </summary>
        /// <typeparam name="TContainerConfigurator">The type of the container configurator.</typeparam>
        /// <param name="containerConfigurator">The container configurator.</param>
        /// <remarks></remarks>
        public void SetContainerConfigurator<TContainerConfigurator>(TContainerConfigurator containerConfigurator)
            where TContainerConfigurator : IContainerConfigurator
        {
            _ContainerConfigurator = containerConfigurator;
        }

        #region Implementation of IApplicationComponent

        public void Configure(IApplicationConfiguration applicationConfiguration)
        {
            _IsConfigured = true;
        }

        #endregion
    }
}