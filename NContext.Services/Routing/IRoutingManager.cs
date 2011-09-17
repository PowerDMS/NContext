// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRoutingManager.cs">
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
//   Defines interface contract which encapsulates logic for creating WCF service routes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Application.Configuration;

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// Defines interface contract which encapsulates logic for creating WCF service routes.
    /// </summary>
    public interface IRoutingManager : IApplicationComponent
    {
        /// <summary>
        /// Registers a service route.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="routePrefix">The route prefix.</param>
        /// <remarks></remarks>
        void RegisterServiceRoute<TServiceContract, TService>(String routePrefix);
    }
}