// --------------------------------------------------------------------------------------------------------------------
// <copyright file="when_logging_an_event.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.Logging.Tests.Specs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Machine.Specifications;

    using NContext.Configuration;
    using NContext.Extensions.Logging.Targets;

    using Telerik.JustMock;

    [Tags("slow")]
    public class when_logging_an_event
    {
        private static ILogTarget _BufferedLogTarget;

        private static ILogTarget _LogTarget;

        private static IManageLogging _LogManager;

        private static Int32 _ProgramExecutionTime = 8000;

        Establish configure_log_manager = () =>
            {
                var config = Mock.Create<ApplicationConfigurationBase>();
                Mock.Arrange(() => config.CompositionContainer).Returns(new CompositionContainer());

                _BufferedLogTarget = new BatchTarget(100, TimeSpan.FromSeconds(2), Environment.ProcessorCount);
                _LogTarget = new SingleTarget(1);
                _LogManager = new LogManager(
                    new LoggingConfiguration(
                        new HashSet<Lazy<ILogTarget>>(
                            new[]
                                {
                                    new Lazy<ILogTarget>(() => _BufferedLogTarget), 
                                    new Lazy<ILogTarget>(() => _LogTarget),
                                }), Environment.ProcessorCount));
                _LogManager.Configure(config);
            };

        Because log_event = () =>
            {
                for (int i = 0; i < 5000; i++)
                {
                    _LogManager.Log(new UserCreatedEvent(new Random(i).Next(0, 50)));
                }
            };

        It should_post_the_logentry_to_the_associated_target_if_one_exists = () =>
            {
                1.ShouldEqual(1);
                Thread.Sleep(_ProgramExecutionTime);
            };
    }

    public class UserCreatedEvent
    {
        private readonly int _UserId;

        public UserCreatedEvent(int userId)
        {
            _UserId = userId;
        }

        public static implicit operator LogEntry(UserCreatedEvent @event)
        {
            return new LogEntry(@event._UserId, new[] { "Info" });
        }
    }

    public class SingleTarget : LogTargetBase
    {
        private Guid _Id = Guid.NewGuid();

        public SingleTarget(Int32 maxDegreeOfParallelism)
            :base(maxDegreeOfParallelism)
        {
            
        }
        
        public override Boolean ShouldLog(LogEntry logEntry)
        {
            return true;
        }

        protected override void Log(LogEntry logEntry)
        {
            Logger.LogMe(_Id, logEntry);
        }
    }

    public class BatchTarget : BatchLogTargetBase
    {
        private Guid _Id = Guid.NewGuid();

        public BatchTarget(Int32 batchSize, TimeSpan flushInterval, Int32 maxDegreeOfParallelism) 
            : base(batchSize, flushInterval, maxDegreeOfParallelism)
        {
        }

        public override Boolean ShouldLog(LogEntry logEntry)
        {
            return true;
        }

        protected override void Log(IEnumerable<LogEntry> logEntries)
        {
            Logger.LogMe(_Id, logEntries);
        }
    }

    public static class Logger
    {
        public static void LogMe(Guid id, IEnumerable<LogEntry> logEntries)
        {
            Thread.Sleep(600);
            Console.WriteLine("Log Target: {0}, Current CPU: {1}, Thread: {2}, Log Count: {3}, Log Data: {4}",
                id,
                GetCurrentProcessorNumber(),
                Thread.CurrentThread.ManagedThreadId,
                logEntries.Count(),
                String.Join("|", logEntries.Select(le => le.Message)));
        }

        public static void LogMe(Guid id, LogEntry logEntry)
        {
            Thread.Sleep(600);
            Console.WriteLine("Log Target: {0}, Current CPU: {1}, Thread: {2}, Log Data: {3}",
                              id,
                              GetCurrentProcessorNumber(),
                              Thread.CurrentThread.ManagedThreadId,
                              logEntry.Message);
        }

        [DllImport("kernel32.dll")]
        private static extern int GetCurrentProcessorNumber();
    }
}