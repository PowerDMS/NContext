// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigureServiceRoutes.cs">
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
//   Defines a role-interface used by the routing manger to register service routes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// Defines a role-interface used by the <see cref="IRoutingManager"/> to register service routes.
    /// </summary>
    [InheritedExport(typeof(IConfigureServiceRoutes))]
    public interface IConfigureServiceRoutes
    {
        /// <summary>
        /// Creates the WCF service routes.
        /// </summary>
        /// <param name="routingManager">The routing manager.</param>
        void RegisterServiceRoutes(IRoutingManager routingManager);
    }
}