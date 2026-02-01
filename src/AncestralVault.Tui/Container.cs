// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common;
using AncestralVault.Tui.UI.Screens;
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

            // Register the screen navigator (scoped so it lives for the session)
            services.AddScoped<ScreenNavigator>();

            // Register screens (transient so they're created fresh each time)
            services.AddTransient<ScreenA>();
            services.AddTransient<ScreenB>();

            // Register the command
            services.AddScoped<TuiCommand>();

            // Build and return the container
            return services.BuildServiceProvider(validateScopes: true);
        }
    }
}
