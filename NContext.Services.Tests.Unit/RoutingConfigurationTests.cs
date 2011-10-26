// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingConfigurationTests.cs">
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

using Microsoft.ApplicationServer.Http;

using NContext.Application.Configuration;
using NContext.Application.Services;
using NContext.Application.Services.Formatters;
using NContext.Application.Services.Routing;

using NUnit.Framework;

namespace NContext.Services.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class RoutingConfigurationTests
    {
        [Test]
        public void ApplicationConfigurationBuilder_RoutingConfigurationWithWebApiConfiguration_ShouldCreateHttpConfiguration()
        {
            ApplicationConfiguration applicationConfiguration =
                    new ApplicationConfigurationBuilder()
                           .RegisterComponent<IRoutingManager>()
                               .With<RoutingConfiguration>()
                                   .SetEndpointBindings(EndpointBinding.Rest)
                                   .SetEndpointPostfix(null, "soap")
                                       .ConfigureWebApi()
                                           .SetEnableTestClient(true)
                                           .SetFormatters(true, new JsonNetMediaTypeFormatter());

            Assert.That(applicationConfiguration.GetComponent<IRoutingManager>(), Is.Not.Null);
            // TODO: (DG) Re-write test. This is temporary.
        }
    }
}