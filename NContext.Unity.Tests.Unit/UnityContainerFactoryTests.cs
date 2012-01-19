// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityContainerFactoryTests.cs">
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