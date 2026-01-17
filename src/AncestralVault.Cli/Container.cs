// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common;
using AncestralVault.Cli.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AncestralVault.Cli
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
            services.AddScoped<LoadFileCommand>();
            services.AddScoped<RebuildCommand>();

            // Build and return the container
            return services.BuildServiceProvider(validateScopes: true);
        }
    }
}
