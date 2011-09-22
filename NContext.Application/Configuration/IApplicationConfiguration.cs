// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationConfiguration.cs">
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
//   Defines an application configuration contract.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace NContext.Application.Configuration
{
    /// <summary>
    /// Defines an application configuration contract.
    /// </summary>
    /// <remarks></remarks>
    public interface IApplicationConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        Boolean IsConfigured { get; }

        /// <summary>
        /// Gets the application composition container.
        /// </summary>
        CompositionContainer CompositionContainer { get; }

        /// <summary>
        /// Gets the application components.
        /// </summary>
        /// <remarks></remarks>
        IEnumerable<IApplicationComponent> Components { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the application component by type registered.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <returns>Instance of <typeparamref name="TApplicationComponent"/> if it exists, else null.</returns>
        /// <remarks></remarks>
        TApplicationComponent GetComponent<TApplicationComponent>()
            where TApplicationComponent : IApplicationComponent;

        /// <summary>
        /// Registers an application component.
        /// </summary>
        /// <typeparam name="TApplicationComponent">The type of the application component.</typeparam>
        /// <param name="componentFactory">The component factory.</param>
        /// <remarks></remarks>
        void RegisterComponent<TApplicationComponent>(Func<IApplicationComponent> componentFactory)
            where TApplicationComponent : IApplicationComponent;

        /// <summary>
        /// Creates all application components and configures them.
        /// </summary>
        /// <remarks>Should only be called once from application startup.</remarks>
        void Setup();

        #endregion
    }
}