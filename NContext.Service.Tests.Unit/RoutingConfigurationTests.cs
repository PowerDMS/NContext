// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoutingConfigurationTests.cs">
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
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using NContext.Application.Configuration;
using NContext.Service.Routing;
using NContext.Service.Soap.Routing;
using NContext.Service.WebApi.Formatters;
using NContext.Service.WebApi.Routing;

using NUnit.Framework;

namespace NContext.Service.Tests.Unit
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
                                       .ConfigureRouting<WebApiRoutingConfiguration>()
                                           .SetFormatters(true, new JsonNetMediaTypeFormatter(), new XmlDataContractMediaTypeFormatter())
                                           .SetEnableHelpPage(true)
                                           .SetEnableTestClient(true)
                                       .ConfigureRouting<SoapRoutingConfiguration>()
                                       ;

            Configure.Using(applicationConfiguration);
            
            Assert.That(applicationConfiguration.GetComponent<IRoutingManager>(), Is.Not.Null);
            // TODO: (DG) Re-write test. This is temporary.
        }
    }
}