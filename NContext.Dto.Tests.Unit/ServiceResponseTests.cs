// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceResponseTests.cs">
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
//   Defines unit tests for ServiceResponse.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using NUnit.Framework;

namespace NContext.Dto.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NContext.Dto;

    using Telerik.JustMock;
    using TypeMock.ArrangeActAssert;

    [TestFixture]
    public class ServiceResponseTests
    {
        [Test]
        public void Let_HasErrors_DoesNotExecuteTheSpecifiedAction()
        {
            var mockServiceResponse = Isolate.Fake.Instance<ServiceResponse<FakeObject>>();
            var dummyError = Isolate.Fake.Instance<Error>();

            Boolean evalDelegate = false;

            Isolate.WhenCalled(() => mockServiceResponse.Errors).WillReturn(new List<Error> { dummyError });
            Isolate.WhenCalled(() => mockServiceResponse.Let(null)).CallOriginal();

            mockServiceResponse.Let(data => evalDelegate = true);

            Assert.That(evalDelegate, Is.False);
        }

        [Test]
        public void Let_NoErrors_ExecutesTheSpecifiedAction()
        {
            var mockServiceResponse = Isolate.Fake.Instance<ServiceResponse<FakeObject>>();

            Boolean evalDelegate = false;

            Isolate.WhenCalled(() => mockServiceResponse.Data).WillReturn(Enumerable.Empty<FakeObject>());
            Isolate.WhenCalled(() => mockServiceResponse.Errors).WillReturn(Enumerable.Empty<Error>());
            Isolate.WhenCalled(() => mockServiceResponse.Let(null)).CallOriginal();

            mockServiceResponse.Let(data => evalDelegate = true);

            Assert.That(evalDelegate, Is.True);
        }

        [Test]
        public void Bind_HasErrors_DoesNotExecuteTheSpecifiedFunction()
        {
            var mockServiceResponse = Isolate.Fake.Instance<ServiceResponse<FakeObject>>();
            var dummyError = Isolate.Fake.Instance<Error>();

            Isolate.WhenCalled(() => mockServiceResponse.Data).WillReturn(Enumerable.Empty<FakeObject>());
            Isolate.WhenCalled(() => mockServiceResponse.Errors).WillReturn(new List<Error> { dummyError });
            Isolate.WhenCalled(() => mockServiceResponse.Bind<FakeObject2>(null)).CallOriginal();

            Boolean evalDelegate = false;

            mockServiceResponse.Bind(data =>
                {
                    evalDelegate = true;
                    return new ServiceResponse<FakeObject2>(new List<FakeObject2>());
                });

            Assert.That(evalDelegate, Is.False);
        }

        [Test]
        public void Bind_NoErrors_ExecutesTheSpecifiedFunction()
        {
            var mockServiceResponse = Isolate.Fake.Instance<ServiceResponse<FakeObject>>();

            Isolate.WhenCalled(() => mockServiceResponse.Data).WillReturn(Enumerable.Empty<FakeObject>());
            Isolate.WhenCalled(() => mockServiceResponse.Errors).WillReturn(Enumerable.Empty<Error>());
            Isolate.WhenCalled(() => mockServiceResponse.Bind<FakeObject2>(null)).CallOriginal();

            Boolean evalDelegate = false;

            mockServiceResponse.Bind(data =>
            {
                evalDelegate = true;
                return new ServiceResponse<FakeObject2>(new List<FakeObject2>());
            });

            Assert.That(evalDelegate, Is.True);
        }

        public class FakeObject
        {
        }

        public class FakeObject2
        {
        }
    }
}