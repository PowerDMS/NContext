// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfigurationTests.cs">
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
//   Defines unit tests for ApplicationConfiguration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Application.Configuration;

using NUnit.Framework;

using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace NContext.Application.Tests.Unit.Configuration
{
    /// <summary>
    /// Defines unit tests for <see cref="ApplicationConfiguration"/>.
    /// </summary>
    [TestFixture]
    public class ApplicationConfigurationTests
    {
        private readonly ApplicationConfiguration _Configuration = new ApplicationConfiguration();

        [TearDown]
        protected void ResetApplicationConfiguration()
        {
            _Configuration.Reset();
        }

        [Test]
        public void Should_return_null_when_component_is_not_registered()
        {
            var component = _Configuration.GetComponent<FakeApplicationComponent>();

            Assert.That(component, Is.Null);
        }

        [Test]
        public void Should_return_component_instance_when_component_is_registered()
        {
            _Configuration.RegisterComponent<FakeApplicationComponent>(() => new FakeApplicationComponent());
            var component = _Configuration.GetComponent<FakeApplicationComponent>();

            Assert.That(component, Is.Not.Null);
        }

        [Test]
        public void Should_add_component_to_application_configuration_when_registering_component()
        {
            _Configuration.RegisterComponent<FakeApplicationComponent>(() => new FakeApplicationComponent());

            Assert.That(_Configuration.Components, Is.Not.Empty);
        }

        [Test]
        public void Should_configure_each_component_when_calling_setup()
        {
            var stubComponent = Mock.Create<FakeApplicationComponent>();
            var stubComponent2 = Mock.Create<FakeApplicationComponent2>();
            stubComponent.Arrange(c => c.Configure(Arg.IsAny<IApplicationConfiguration>())).IgnoreArguments().DoNothing().OccursOnce();
            stubComponent2.Arrange(c => c.Configure(Arg.IsAny<IApplicationConfiguration>())).IgnoreArguments().DoNothing().OccursOnce();

            _Configuration.RegisterComponent<FakeApplicationComponent>(() => stubComponent);
            _Configuration.RegisterComponent<FakeApplicationComponent2>(() => stubComponent2);
            _Configuration.Setup();

            Mock.Assert(stubComponent);
            Mock.Assert(stubComponent2);
        }
        
        public class FakeApplicationComponent : IApplicationComponent
        {
            private Boolean _IsConfigured;

            public Boolean IsConfigured
            {
                get
                {
                    return _IsConfigured;
                }
            }

            public virtual void Configure(IApplicationConfiguration applicationConfiguration)
            {
                _IsConfigured = true;
            }
        }

        public class FakeApplicationComponent2 : IApplicationComponent
        {
            private Boolean _IsConfigured;

            public Boolean IsConfigured
            {
                get
                {
                    return _IsConfigured;
                }
            }

            public virtual void Configure(IApplicationConfiguration applicationConfiguration)
            {
                _IsConfigured = true;
            }
        }
    }
}