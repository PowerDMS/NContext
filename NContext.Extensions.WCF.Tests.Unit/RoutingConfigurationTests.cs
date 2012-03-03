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

using System.Net.Http;
using System.Web.Http.SelfHost;

using NContext.Configuration;
using NContext.Extensions.WebApi.Formatters;
using NContext.Extensions.WebApi.Routing;

using NUnit.Framework;

namespace NContext.Extensions.WCF.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class RoutingConfigurationTests
    {
        [Ignore]
        public void ApplicationConfigurationBuilder_State_ShouldCreateHttpConfiguration()
        {
            ApplicationConfiguration applicationConfiguration =
                new ApplicationConfigurationBuilder()
                    .RegisterComponent<IManageWebApiRouting>()
                        .With<WebApiConfigurationBuilder>();
            
            Configure.Using(applicationConfiguration);
            
            Assert.That(applicationConfiguration.GetComponent<IManageWebApiRouting>(), Is.Not.Null);
            // TODO: (DG) Re-write test. This is temporary.
        }
    }
}