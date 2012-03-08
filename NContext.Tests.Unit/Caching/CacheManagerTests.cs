// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManagerTests.cs">
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
//   Defines unit tests for 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Caching;

using NContext.Caching;
using NContext.Configuration;

using NUnit.Framework;

using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace NContext.Tests.Unit.Caching
{
    /// <summary>
    /// Defines unit tests for <see cref="CacheManager"/>
    /// </summary>
    [TestFixture]
    public class CacheManagerTests
    {
        [Test]
        public void Ctor_NullCacheConfiguration_ThrowsException()
        {
            CacheConfiguration cacheConfiguration = null;

            var cacheManagerDelegate = new TestDelegate(() => new CacheManager(cacheConfiguration));

            Assert.That(cacheManagerDelegate, Throws.InstanceOf<ArgumentNullException>());
        }

        // TODO: (DG) Rewrite this test!
        [Ignore]
        public void AddOrUpdateItem_NullOrWhiteSpaceCacheEntryKey_ThrowsException()
        {
            var stubCacheProvider = Mock.Create<ObjectCache>();
            var stubConfigurationBuilder = Mock.Create<ApplicationConfigurationBuilder>();
            var stubCacheConfiguration = Mock.Create<CacheConfigurationBuilder>(stubConfigurationBuilder);
            var mockCacheManager = Mock.Create<CacheManager>(stubCacheConfiguration);

            stubCacheProvider.Arrange(p => p.Add(Arg.IsAny<String>(), Arg.IsAny<Object>(), Arg.IsAny<CacheItemPolicy>(), Arg.AnyString)).IgnoreArguments().DoNothing();
            stubCacheProvider.Arrange(p => p.Remove(Arg.IsAny<String>(), Arg.AnyString)).IgnoreArguments().DoNothing();
            stubCacheConfiguration.SetProvider(() => stubCacheProvider);

            var addOrUpdateItem = new TestDelegate(() => mockCacheManager.AddOrUpdateItem<Object>(null, null));

            Assert.That(addOrUpdateItem, Throws.InstanceOf<ArgumentNullException>());
        }
    }
}