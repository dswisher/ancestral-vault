// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common;
using AncestralVault.TestCli.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AncestralVault.TestCli
{
    public static class Container
    {
        public static ServiceProvider CreateContainer()
        {
            // Create the service collection
            var services = new ServiceCollection();

            // Register MSFT logging
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            // Register the common bits
            services.RegisterVaultCommon();

            // Register all the commands
            services.AddScoped<DumpPersonaCommand>();

            // Build and return the container
            return services.BuildServiceProvider(validateScopes: true);
        }
    }
}
