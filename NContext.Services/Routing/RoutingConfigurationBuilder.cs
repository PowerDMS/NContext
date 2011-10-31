// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingConfigurationBuilder.cs">
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
//   Defines a fluent WCF configuration builder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// Defines a fluent WCF configuration builder interface.
    /// </summary>
    public class RoutingConfigurationBuilder
    {
        private readonly RoutingConfiguration _RoutingConfiguration;

        public RoutingConfigurationBuilder(RoutingConfiguration routingConfiguration)
        {
            _RoutingConfiguration = routingConfiguration;
        }

        public RoutingConfiguration RoutingConfiguration
        {
            get
            {
                return _RoutingConfiguration;
            }
        }
    }
}