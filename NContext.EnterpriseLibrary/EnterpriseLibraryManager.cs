// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnterpriseLibraryManager.cs">
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

// <summary>
//   Defines an application component for Microsoft Enterprise Library support.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

using NContext.Application.Configuration;

namespace NContext.Application.EnterpriseLibrary
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