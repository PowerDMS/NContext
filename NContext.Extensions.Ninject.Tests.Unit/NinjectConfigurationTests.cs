using System;
using System.Linq;

using NUnit.Framework;

using Ninject;

using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace NContext.Extensions.Ninject.Tests.Unit
{
    [TestFixture]
    public class NinjectConfigurationTests
    {
        [Test]
        public void Modules_NoModulesConfigured_ReturnsEmptyEnumerableByDefault()
        {
            var ninjectConfiguration = Mock.Create<NinjectConfiguration>(Constructor.NotMocked);
            ninjectConfiguration.Arrange(c => c.Modules).CallOriginal();

            var modules = ninjectConfiguration.Modules;

            Mock.Assert(!modules.Any());
        }

        [Test]
        public void Kernel_DefaultConfiguration_ReturnsInstanceOfStandardKernel()
        {
            var ninjectConfiguration = Mock.Create<NinjectConfiguration>(Constructor.NotMocked);
            ninjectConfiguration.Arrange(c => c.NinjectSettings).CallOriginal().MustBeCalled();
            ninjectConfiguration.Arrange(c => c.Modules).CallOriginal().MustBeCalled();
            ninjectConfiguration.Arrange(c => c.Kernel).CallOriginal();

            var kernel = ninjectConfiguration.Kernel;

            Assert.That(kernel, Is.InstanceOf<StandardKernel>());
            ninjectConfiguration.AssertAll();
        }

        [Test]
        public void Kernel_CustomKernelConfigured_ReturnsCustomKernel()
        {
            var mockKernel = Mock.Create<IKernel>();
            var ninjectConfiguration = Mock.Create<NinjectConfiguration>(Constructor.NotMocked);
            ninjectConfiguration.Arrange(c => c.SetKernel(Arg.IsAny<Func<IKernel>>())).CallOriginal();
            ninjectConfiguration.Arrange(c => c.NinjectSettings).CallOriginal().MustBeCalled();
            ninjectConfiguration.Arrange(c => c.Modules).CallOriginal().MustBeCalled();
            ninjectConfiguration.Arrange(c => c.Kernel).CallOriginal();

            ninjectConfiguration.SetKernel(() => mockKernel);
            var kernel = ninjectConfiguration.Kernel;

            Assert.That(kernel, Is.EqualTo(mockKernel));
        }
    }
}