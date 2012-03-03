// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyedHashProviderTests.cs">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

using NContext.Extensions.EnterpriseLibrary.Security.Cryptography;

using NUnit.Framework;

using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace NContext.Extensions.EnterpriseLibrary.Tests.Unit
{
    [TestFixture]
    public class KeyedHashProviderTests
    {
        private const String _SymmetricKey = "MySecretKey";

        private const String _PlainText = "MyPlainText";

        private static Byte[] _SymmetricKeyBytes = Encoding.UTF8.GetBytes(_SymmetricKey);

        private static Byte[] _PlainTextBytes = Encoding.UTF8.GetBytes(_PlainText);

        #region Constructor Tests

        [Test]
        public void Ctor_NullArgument_ThrowsArgumentNullException()
        {
            var mockHashProvider = new TestDelegate(() => Mock.Create<KeyedHashProvider>(Arg.IsNull<Type>()));

            Assert.That(mockHashProvider, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Ctor_InvalidArgument_ThrowsArgumentException()
        {
            Type defaultKeyedHashAlgorithm = GetType();

            var mockHashProvider = new TestDelegate(() => Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm));

            Assert.That(mockHashProvider, Throws.ArgumentException);
        }

        #endregion

        #region Create Hash Tests

        [Test]
        public void CreateHashAsBase64_DefaultWithNoSalt_ReturnsAValidHash()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            mockHashProvider.Arrange(p => p.CreateHashAsBase64(Arg.AnyString, Arg.AnyString, false, DataProtectionScope.LocalMachine)).CallOriginal();

            var actual = mockHashProvider.CreateHashAsBase64(_SymmetricKey, _PlainText, false);
            var expected = Convert.ToBase64String(new HMACSHA256(_SymmetricKeyBytes).ComputeHash(_PlainTextBytes));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateHashAsBase64Generic_HMACSHA1WithNoSalt_ReturnsAValidHash()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            mockHashProvider.Arrange(p => p.CreateHashAsBase64<HMACSHA1>(Arg.AnyString, Arg.AnyString, false, DataProtectionScope.LocalMachine)).CallOriginal();

            var actual = mockHashProvider.CreateHashAsBase64<HMACSHA1>(_SymmetricKey, _PlainText, false);
            var expected = Convert.ToBase64String(new HMACSHA1(_SymmetricKeyBytes).ComputeHash(_PlainTextBytes));

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Creates the hash_ with salt_ returns A verifyable salted hash.
        /// </summary>
        /// <remarks>
        /// Enterprise library uses an intricate salting procedure.By default, the first 
        /// 16 bytes of a hash result are its salt. To verify a hash, you must extract 
        /// the salt from original hash, and create a hash of the comparing plaintext.
        /// </remarks>
        [Test]
        public void CreateHash_WithSalt_ReturnsAVerifyableSaltedHash()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            mockHashProvider.Arrange(p => p.CreateHash(Arg.IsAny<Byte[]>(), Arg.IsAny<Byte[]>(), true, DataProtectionScope.LocalMachine)).CallOriginal();

            var actual = 
                mockHashProvider.CreateHash(_SymmetricKeyBytes, _PlainTextBytes)
                                .ToArray();

            var salt = CryptographyUtility.ExtractSalt(actual);
            var expected = CryptographyUtility.AddSaltToHash(salt,
                new HMACSHA256(_SymmetricKeyBytes).ComputeHash(CryptographyUtility.AddSaltToPlainText(salt, _PlainTextBytes)).ToArray());
            
            Assert.That(CryptographyUtility.CompareBytes(expected, actual));
        }

        [Test]
        public void CreateHash_NoSalt_ReturnsAVerifyableHash()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            mockHashProvider.Arrange(p => p.CreateHash(Arg.IsAny<Byte[]>(), Arg.IsAny<Byte[]>(), false, DataProtectionScope.LocalMachine)).CallOriginal();

            var actual = mockHashProvider.CreateHash(_SymmetricKeyBytes, _PlainTextBytes, false).ToArray();
            var expected = new HMACSHA256(_SymmetricKeyBytes).ComputeHash(_PlainTextBytes).ToArray();

            Assert.That(CryptographyUtility.CompareBytes(expected, actual));
        }

        [Test]
        public void CreateHash_State_Result2()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            Byte[] symmetricKey = Encoding.UTF8.GetBytes(_SymmetricKey);
            Byte[] plainText = Encoding.UTF8.GetBytes(_PlainText);
            bool saltEnabled = false;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            byte[] expected = null;
            byte[] actual;
            //actual = target.CreateHash(symmetricKey, plainText, saltEnabled, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateHash_State_Result3()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            Byte[] symmetricKey = Encoding.UTF8.GetBytes(_SymmetricKey);
            Byte[] plainText = Encoding.UTF8.GetBytes(_PlainText);
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            byte[] expected = null;
            byte[] actual;
            //actual = target.CreateHash(symmetricKey, plainText, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateHashToBase64String_State_Result()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            string expected = string.Empty;
            string actual;
            //actual = target.CreateHashAsBase64<TKeyedHashAlgorithm>(symmetricKey, plainText, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateHashToBase64String_State_Result1()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            bool saltEnabled = false;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            string expected = string.Empty;
            string actual;
            //actual = target.CreateHashAsBase64<TKeyedHashAlgorithm>(symmetricKey, plainText, saltEnabled, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateHashToBase64String_State_Result2()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            string expected = string.Empty;
            string actual;
            //actual = target.CreateHashAsBase64(symmetricKey, plainText, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateHashToBase64String_State_Result3()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            bool saltEnabled = false;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            string expected = string.Empty;
            string actual;
            //actual = target.CreateHashAsBase64(symmetricKey, plainText, saltEnabled, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Compare Hash Tests

        [Test]
        public void CompareHMAC_State_Result()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            string base64EncodedHash = string.Empty;
            bool expected = false;
            bool actual;
            //actual = target.CompareHMAC(symmetricKey, plainText, base64EncodedHash);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHMAC_State_Result1()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            string base64EncodedHash = string.Empty;
            bool expected = false;
            bool actual;
            //actual = target.CompareHMAC<TKeyedHashAlgorithm>(symmetricKey, plainText, base64EncodedHash);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHash_State_Result()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            string base64EncodedHash = string.Empty;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            bool expected = false;
            bool actual;
            //actual = target.CompareHash(symmetricKey, plainText, base64EncodedHash, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHash_State_Result1()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            string base64EncodedHash = string.Empty;
            bool saltEnabled = false;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            bool expected = false;
            bool actual;
//            actual = target.CompareHash(symmetricKey, plainText, base64EncodedHash, saltEnabled, dataProtectionScope);
//            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHash_State_Result2()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            Byte[] symmetricKey = Encoding.UTF8.GetBytes(_SymmetricKey);
            Byte[] plainText = Encoding.UTF8.GetBytes(_PlainText);
            byte[] hashedText = null;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            bool expected = false;
            bool actual;
//            actual = target.CompareHash(symmetricKey, plainText, hashedText, dataProtectionScope);
//            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHash_State_Result3()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            Byte[] symmetricKey = Encoding.UTF8.GetBytes(_SymmetricKey);
            Byte[] plainText = Encoding.UTF8.GetBytes(_PlainText);
            byte[] hashedText = null;
            bool saltEnabled = false;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            bool expected = false;
            bool actual;
//            actual = target.CompareHash(symmetricKey, plainText, hashedText, saltEnabled, dataProtectionScope);
//            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHash_State_Result4()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            Byte[] symmetricKey = Encoding.UTF8.GetBytes(_SymmetricKey);
            Byte[] plainText = Encoding.UTF8.GetBytes(_PlainText);
            byte[] hashedText = null;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            bool expected = false;
            bool actual;
            //actual = target.CompareHash<TKeyedHashAlgorithm>(symmetricKey, plainText, hashedText, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHash_State_Result5()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            string base64EncodedHash = string.Empty;
            bool saltEnabled = false;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            bool expected = false;
            bool actual;
            //actual = target.CompareHash<TKeyedHashAlgorithm>(symmetricKey, plainText, base64EncodedHash, saltEnabled, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHash_State_Result6()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            Byte[] symmetricKey = Encoding.UTF8.GetBytes(_SymmetricKey);
            Byte[] plainText = Encoding.UTF8.GetBytes(_PlainText);
            byte[] hashedText = null;
            bool saltEnabled = false;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            bool expected = false;
            bool actual;
            //actual = target.CompareHash<TKeyedHashAlgorithm>(symmetricKey, plainText, hashedText, saltEnabled, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareHash_State_Result7()
        {
            Type defaultKeyedHashAlgorithm = typeof(HMACSHA256);
            var mockHashProvider = Mock.Create<KeyedHashProvider>(defaultKeyedHashAlgorithm);
            string symmetricKey = string.Empty;
            string plainText = string.Empty;
            string base64EncodedHash = string.Empty;
            DataProtectionScope dataProtectionScope = new DataProtectionScope();
            bool expected = false;
            bool actual;
            //actual = target.CompareHash<TKeyedHashAlgorithm>(symmetricKey, plainText, base64EncodedHash, dataProtectionScope);
            //Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}