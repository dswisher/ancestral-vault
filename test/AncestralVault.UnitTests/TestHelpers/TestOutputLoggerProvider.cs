// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AncestralVault.UnitTests.TestHelpers
{
    public class TestOutputLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TestOutputLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TestOutputLogger(testOutputHelper, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
