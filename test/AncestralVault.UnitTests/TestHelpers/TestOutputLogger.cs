// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AncestralVault.UnitTests.TestHelpers
{
    public class TestOutputLogger : ILogger
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly string categoryName;

        public TestOutputLogger(ITestOutputHelper testOutputHelper, string categoryName)
        {
            this.testOutputHelper = testOutputHelper;
            this.categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull => new NoOpDisposable();

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            testOutputHelper.WriteLine($"[{logLevel}] {categoryName}: {message}");

            if (exception != null)
            {
                testOutputHelper.WriteLine(exception.ToString());
            }
        }

        private class NoOpDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
