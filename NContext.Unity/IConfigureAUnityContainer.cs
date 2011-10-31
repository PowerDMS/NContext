// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigureAUnityContainer.cs">
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
//   Defines a role-interface which encapsulates logic to configure an IUnityContainer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;

using Microsoft.Practices.Unity;

namespace NContext.Unity
{
    /// <summary>
    /// Defines a role-interface which encapsulates logic to configure an <see cref="IUnityContainer"/>.
    /// </summary>
    [InheritedExport(typeof(IConfigureAUnityContainer))]
    public interface IConfigureAUnityContainer
    {
        /// <summary>
        /// Gets the priority in which to configure the container. Implementations will be run 
        /// in ascending order based on priority, so a lower priority value will execute first.
        /// </summary>
        /// <remarks></remarks>
        Int32 Priority { get; }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/> dependency injection container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <remarks></remarks>
        void ConfigureContainer(IUnityContainer container);
    }
}