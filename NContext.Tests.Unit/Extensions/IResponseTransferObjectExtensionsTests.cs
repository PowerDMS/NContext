// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectExtensionsTests.cs">
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
//   Defines unit tests for IResponseTransferObject.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
        #region LetAsync Tests

        [Test]
        public void LetAsync_ResponseTransferObjectHasData_ExecutesTheTaskOnASeparateThread()
        {
            Int32 activeThreadId = Thread.CurrentThread.ManagedThreadId;
            Int32 actionThreadId = activeThreadId;

            var taskStatus = TaskStatus.Created;
            var stubData = Mock.Create<List<FakeObject>>();
            stubData.Add(new FakeObject());
            stubData.Add(new FakeObject());

            var mockResponse = new ServiceResponse<FakeObject>(stubData);
            Mock.NonPublic.Arrange<IEnumerable<FakeObject>>(mockResponse, "Data").Returns(new List<FakeObject>(stubData));
            mockResponse.Arrange(response => response.Let(Arg.IsAny<Action<IEnumerable<FakeObject>>>())).IgnoreArguments().CallOriginal();

            mockResponse.AsConcurrent()
                        .Let(3, CancellationToken.None, ParallelExecutionMode.ForceParallelism,
                            data =>
                            {
                                Console.WriteLine(data.Count() + "I'm first action. " + Thread.CurrentThread.ManagedThreadId);
                            },
                            data => {
                                data = new List<FakeObject>();
                                Console.WriteLine(data.Count() + "I'm second action. " + Thread.CurrentThread.ManagedThreadId);
                            },
                            data => { Console.WriteLine("I'm third action. " + Thread.CurrentThread.ManagedThreadId); },
                            data => { Console.WriteLine("I'm fourth action. " + Thread.CurrentThread.ManagedThreadId); },
                            data => { Console.WriteLine("I'm fifth action. " + Thread.CurrentThread.ManagedThreadId); },
                            data => { Thread.Sleep(TimeSpan.FromSeconds(2)); Console.WriteLine("I'm sixth action. " + Thread.CurrentThread.ManagedThreadId); },
                            data => { Console.WriteLine("I'm seventh action. " + Thread.CurrentThread.ManagedThreadId); },
                            data => { Console.WriteLine(data.Count() + "I'm eigth action. " + Thread.CurrentThread.ManagedThreadId); })
                        .Bind((data, task, cancellationSource) =>
                            {
                                Console.WriteLine("I'm the non-parallel action. " + Thread.CurrentThread.ManagedThreadId);
                                Console.WriteLine("Is Canceled: " + task.IsCanceled);
                                cancellationSource.Cancel();
                                Console.WriteLine("Is Cancel Requested: " + cancellationSource.IsCancellationRequested);
                                Thread.Sleep(TimeSpan.FromSeconds(5));

                                Console.WriteLine("Is Canceled: " + task.IsCanceled);
                                
                                return new ServiceResponse<FakeObject2>(Enumerable.Empty<FakeObject2>());
                            })
                        ;

            //Assert.That(activeThreadId, Is.Not.EqualTo(actionThreadId));
            //Assert.That(taskStatus == TaskStatus.RanToCompletion);
        }

        [Test]
        public void Bla()
        {
            var actionBlock = new ActionBlock<Int32>(i =>
                {
                    Console.WriteLine(i);
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 4 });
            var transformBlock = new TransformBlock<String, Int32>(s =>
                {
                    Thread.Sleep(500);
                    return Convert.ToInt32(s);
                }, 
                new ExecutionDataflowBlockOptions{ MaxDegreeOfParallelism = 4 });

            transformBlock.LinkTo(actionBlock);

            var response = new ServiceResponse<String>(new[]
                {
                    "1","1","1","1",
                    "2","2","2","2",
                    "3","3","3","3",
                    "4","4","4","4",
                    "5","5","5","5"
                });

            new ServiceResponseParallelDecorator<String>(response)
                .Post(transformBlock)
                .Let(data =>
                    {
                        Console.WriteLine("Im done.");
                        Task.WaitAll(transformBlock.Completion);
                    });

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