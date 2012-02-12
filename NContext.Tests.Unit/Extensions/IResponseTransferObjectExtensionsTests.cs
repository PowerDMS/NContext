// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensionsTests.cs">
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
//   Defines unit tests for IResponseTransferObject.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NContext.Dto;
using NContext.Extensions;

using NUnit.Framework;

using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace NContext.Tests.Unit.Extensions
{
    /// <summary>
    /// Defines unit tests for <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    [TestFixture]
    public class IResponseTransferObjectExtensionsTests
    {
        #region BindAsync Tests

        [Test]
        public void BindAsync_ResponseTransferObjectHasData_ExecutesTheTaskOnASeparateThread()
        {
            Int32 activeThreadId = Thread.CurrentThread.ManagedThreadId;
            Int32 bindingThreadId = -1;

            var bindTaskStatus = TaskStatus.Created;
            var fakeData = Mock.Create<FakeObject>();
            var fakeData2 = Mock.Create<FakeObject2>();
            var mockResponse = Mock.Create<IResponseTransferObject<FakeObject>>();
            mockResponse.Arrange(response => response.Data).Returns(new[] { fakeData });

            mockResponse.BindAsync(
                            fakes =>
                                {
                                    bindingThreadId = Thread.CurrentThread.ManagedThreadId;
                                    return new ServiceResponse<FakeObject2>(fakeData2);
                                })
                        .ContinueWith(
                            task =>
                                {
                                    bindTaskStatus = task.Status;
                                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                        .Wait();

            Assert.That(activeThreadId, Is.Not.EqualTo(bindingThreadId));
            Assert.That(bindTaskStatus == TaskStatus.RanToCompletion);
        }

        [Test]
        public void BindAsync_ResponseTransferObjectHasErrors_ExecutesTheTaskSynchronously()
        {
            Int32 activeThreadId = Thread.CurrentThread.ManagedThreadId;
            Int32 bindingThreadId = activeThreadId;

            var fakeData = Mock.Create<FakeError>(Constructor.Mocked);
            var fakeData2 = Mock.Create<FakeObject2>();
            var mockResponse = Mock.Create<IResponseTransferObject<FakeObject>>();
            mockResponse.Arrange(response => response.Errors).Returns(new[] { fakeData });

            var bindTask = 
                mockResponse.BindAsync(
                    fakes =>
                        {
                            // Should never get here when errors exist.
                            bindingThreadId = Thread.CurrentThread.ManagedThreadId;
                            return new ServiceResponse<FakeObject2>(fakeData2);
                        });

            Assert.That(activeThreadId == bindingThreadId);
            Assert.That(bindTask.Status == TaskStatus.RanToCompletion);
        }

        [Test]
        public void BindAsync_BindingFunctionThrowsException_SetsTheTaskStatusAsFaulted()
        {
            var bindTaskStatus = TaskStatus.Created;
            var fakeData = Mock.Create<FakeObject>();
            var mockResponse = Mock.Create<IResponseTransferObject<FakeObject>>();
            mockResponse.Arrange(response => response.Data).Returns(new[] { fakeData });

            mockResponse.BindAsync<FakeObject, FakeObject2>(
                            fakes =>
                                {
                                    throw new Exception("Test exception.");
                                })
                        .ContinueWith(
                            task =>
                                {
                                    bindTaskStatus = task.Status;
                                }, TaskContinuationOptions.OnlyOnFaulted)
                        .Wait();

            Assert.That(bindTaskStatus == TaskStatus.Faulted);
        }

        [Test]
        public void BindAsync_ResponseTransferObjectHasNoErrorsOrData_SetsTheTaskStatusAsFaulted()
        {
            var bindTaskStatus = TaskStatus.Created;
            var fakeData = Mock.Create<FakeObject>();
            var mockResponse = Mock.Create<IResponseTransferObject<FakeObject>>();

            mockResponse.BindAsync<FakeObject, FakeObject2>(
                            fakes =>
                            {
                                // Should never get here. IResponseTransferObject is in an invalid state for binding.
                                return null;
                            })
                        .ContinueWith(
                            task =>
                            {
                                bindTaskStatus = task.Status;
                            }, TaskContinuationOptions.OnlyOnFaulted)
                        .Wait();

            Assert.That(bindTaskStatus == TaskStatus.Faulted);
        }

        #endregion

        #region LetAsync Tests

        [Test]
        public void LetAsync_ResponseTransferObjectHasData_ExecutesTheTaskOnASeparateThread()
        {
            Int32 activeThreadId = Thread.CurrentThread.ManagedThreadId;
            Int32 actionThreadId = activeThreadId;

            var taskStatus = TaskStatus.Created;
            var stubData = Mock.Create<FakeObject>();
            var mockResponse = Mock.Create<IResponseTransferObject<FakeObject>>();
            mockResponse.Arrange(response => response.Data).Returns(new[] { stubData });

            mockResponse.LetAsync(
                            data =>
                            {
                                actionThreadId = Thread.CurrentThread.ManagedThreadId;
                            })
                        .ContinueWith(
                            task =>
                            {
                                taskStatus = task.Status;
                            }, TaskContinuationOptions.OnlyOnRanToCompletion)
                        .Wait();

            Assert.That(activeThreadId, Is.Not.EqualTo(actionThreadId));
            Assert.That(taskStatus == TaskStatus.RanToCompletion);
        }

        #endregion

        public class FakeObject
        {
        }

        public class FakeObject2
        {
        }

        public class FakeError : Error
        {
            public FakeError(String name, IEnumerable<String> messages, String errorCode)
                : base(name, messages, errorCode)
            {
            }
        }
    }
}