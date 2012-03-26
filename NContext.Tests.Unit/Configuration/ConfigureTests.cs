// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureTests.cs">
//   Copyright (c) 2012
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
//   Defines unit tests for application configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Configuration;

using NUnit.Framework;

using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace NContext.Tests.Unit.Configuration
{
    /// <summary>
    /// Defines unit tests for application configuration.
    /// </summary>
    [TestFixture]
    public class ConfigureTests
    {
        [Test]
        public void Using_NullConfiguration_ThrowsException()
        {
            ApplicationConfigurationBase configuration = null;

            var configureUsing = new TestDelegate(() => Configure.Using(configuration));

            Assert.That(configureUsing, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Using_ConfigurationInstance_CallsSetupOnlyOnce()
        {
            var configuration = Mock.Create<ApplicationConfigurationBase>();
            configuration.Arrange(c => c.Setup()).DoNothing();

            Configure.Using(configuration);

            Mock.Assert(() => configuration.Setup(), Occurs.Once());
        }
    }
}