// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConfigurationTests.cs">
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
//   Defines unit tests for ApplicationConfiguration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

using NContext.Configuration;

using NUnit.Framework;

using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace NContext.Tests.Unit.Configuration
{
    /// <summary>
    /// Defines unit tests for <see cref="ApplicationConfiguration"/>.
    /// </summary>
    [TestFixture]
    public class ApplicationConfigurationTests
    {
        [Test]
        public void GetComponent_ComponentIsNotRegistered_ReturnsNull()
        {
            var configuration = Mock.Create<ApplicationConfiguration>();
            configuration.Arrange(c => c.GetComponent<FakeApplicationComponent>()).CallOriginal();

            var component = configuration.GetComponent<FakeApplicationComponent>();

            Assert.That(component, Is.Null);
        }

        [Test]
        public void GetComponent_ComponentIsRegistered_ReturnsComponentInstance()
        {
            var configuration = Mock.Create<ApplicationConfiguration>();
            configuration.Arrange(c => c.RegisterComponent<FakeApplicationComponent>(Arg.IsAny<Func<FakeApplicationComponent>>())).CallOriginal();
            configuration.Arrange(c => c.GetComponent<FakeApplicationComponent>()).CallOriginal();

            configuration.RegisterComponent<FakeApplicationComponent>(() => new FakeApplicationComponent());
            var component = configuration.GetComponent<FakeApplicationComponent>();

            Assert.That(component, Is.Not.Null);
        }

        [Test]
        public void RegisterComponent_NewComponentInstance_RegistersComponentInCollection()
        {
            var configuration = Mock.Create<ApplicationConfiguration>();
            configuration.Arrange(c => c.RegisterComponent<FakeApplicationComponent>(Arg.IsAny<Func<FakeApplicationComponent>>())).CallOriginal();
            configuration.Arrange(c => c.Components).CallOriginal();

            configuration.RegisterComponent<FakeApplicationComponent>(() => new FakeApplicationComponent());

            Assert.That(configuration.Components, Is.Not.Empty);
        }

        [Test]
        public void Setup_MultipleComponentsRegistered_ConfiguresEachComponent()
        {
            var configuration = Mock.Create<ApplicationConfiguration>();
            var stubComponent = Mock.Create<FakeApplicationComponent>();
            var stubComponent2 = Mock.Create<FakeApplicationComponent2>();

            Mock.NonPublic
                .Arrange<CompositionContainer>(configuration, "CreateCompositionContainer", ArgExpr.IsAny<HashSet<String>>(), ArgExpr.IsAny<HashSet<Predicate<String>>>())
                .Returns(new CompositionContainer());

            configuration.Arrange(c => c.Components).CallOriginal();
            configuration.Arrange(c => c.RegisterComponent<FakeApplicationComponent>(Arg.IsAny<Func<FakeApplicationComponent>>())).CallOriginal();
            configuration.Arrange(c => c.RegisterComponent<FakeApplicationComponent2>(Arg.IsAny<Func<FakeApplicationComponent2>>())).CallOriginal();
            configuration.Arrange(c => c.Setup()).CallOriginal();
            
            stubComponent.Arrange(c => c.Configure(Arg.IsAny<ApplicationConfigurationBase>())).IgnoreArguments().DoNothing().OccursOnce();
            stubComponent2.Arrange(c => c.Configure(Arg.IsAny<ApplicationConfigurationBase>())).IgnoreArguments().DoNothing().OccursOnce();

            configuration.RegisterComponent<FakeApplicationComponent>(() => stubComponent);
            configuration.RegisterComponent<FakeApplicationComponent2>(() => stubComponent2);
            configuration.Setup();

            Mock.Assert(stubComponent);
            Mock.Assert(stubComponent2);
        }

        public class FCC : CompositionContainer
        {
            
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

            public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
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

            public virtual void Configure(ApplicationConfigurationBase applicationConfiguration)
            {
                _IsConfigured = true;
            }
        }
    }
}