// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common;
using Microsoft.Extensions.DependencyInjection;

namespace AncestralVault.Tui
{
    public static class Container
    {
        public static ServiceProvider CreateContainer()
        {
            // Create the service collection
            var services = new ServiceCollection();

            // Register the common bits
            services.RegisterVaultCommon();

            // Register the command
            services.AddScoped<TuiCommand>();

            // Build and return the container
            return services.BuildServiceProvider(validateScopes: true);
        }
    }
}
