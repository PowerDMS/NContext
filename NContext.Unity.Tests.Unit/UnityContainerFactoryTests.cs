// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityContainerFactoryTests.cs">
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

using NUnit.Framework;

namespace NContext.Unity.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class UnityContainerFactoryTests
    {
        [Test]
        public void Create_DefaultCtor_ShouldCreateNewIUnityContainer()
        {
            var unityContainer = UnityContainerFactory.Create();

            Assert.That(unityContainer, Is.Not.Null);
        }

        [Test]
        public void Create_InvalidConfigurationFile_ShouldThrowException()
        {
            TestDelegate createUnityContainer = () => UnityContainerFactory.Create("invalidFile.config");

            Assert.That(createUnityContainer, Throws.Exception);
        }

        [Test]
        public void Create_ConfigurationFileWithDefaultContainer_ShouldCreateNewIUnityContainer()
        {
            TestDelegate createUnityContainer = () => UnityContainerFactory.Create("unity.config");

            Assert.That(createUnityContainer, Throws.Exception);
        }

        [Test]
        public void Create_ConfigurationFileInvalidContainerName_ShouldThrowException()
        {
            TestDelegate createUnityContainer = () => UnityContainerFactory.Create("unity.config", "unity", "InvalidContainer");

            Assert.That(createUnityContainer, Throws.Exception);
        }

        [Test]
        public void Create_ConfigurationFileWithContainerName_ShouldCreateNewIUnityContainer()
        {
            TestDelegate createUnityContainer = () => UnityContainerFactory.Create("unity.config", "unity", "UnityTestContainer");

            Assert.That(createUnityContainer, Throws.Exception);
        }
    }
}