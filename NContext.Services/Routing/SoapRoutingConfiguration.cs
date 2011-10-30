// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoapRoutingConfiguration.cs">
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
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using NContext.Application.Configuration;

namespace NContext.Application.Services.Routing
{
    /// <summary>
    /// 
    /// </summary>
    public class SoapRoutingConfiguration : RoutingConfigurationBase
    {
        public SoapRoutingConfiguration(ApplicationConfigurationBuilder applicationConfigurationBuilder, RoutingConfigurationBuilder routingConfigurationBuilder)
            : base(applicationConfigurationBuilder, routingConfigurationBuilder)
        {
        }

        protected override void Setup()
        {
            // TODO: (DG) Add better support for SOAP.
            throw new NotImplementedException();
        }
    }
}