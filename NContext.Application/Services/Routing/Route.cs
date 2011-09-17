// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRoute.cs">
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
//   Defines a class which represents a WCF service route.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// Defines a class which represents a WCF service route.
    /// </summary>
    public class Route
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="routePrefix">The route prefix.</param>
        /// <param name="serviceContractType">Type of the service contract.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="binding">The binding.</param>
        /// <remarks></remarks>
        public Route(String routePrefix, Type serviceContractType, Type serviceType, EndpointBinding binding)
        {
            RoutePrefix = routePrefix;
            ServiceContractType = serviceContractType;
            ServiceType = serviceType;
            Binding = binding;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the binding.
        /// </summary>
        /// <remarks></remarks>
        public EndpointBinding Binding { get; private set; }

        /// <summary>
        /// Gets the route prefix.
        /// </summary>
        /// <remarks></remarks>
        public String RoutePrefix { get; private set; }

        /// <summary>
        /// Gets the type of the service contract.
        /// </summary>
        /// <remarks></remarks>
        public Type ServiceContractType { get; private set; }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <remarks></remarks>
        public Type ServiceType { get; private set; }

        #endregion
    }
}