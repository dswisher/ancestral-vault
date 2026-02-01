// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common;
using AncestralVault.Tui.UI;
using AncestralVault.Tui.UI.Screens;
using AncestralVault.Tui.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AncestralVault.Tui
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

            // Register the navigation context (singleton so it persists for the app lifetime)
            services.AddSingleton<NavigationContext>();

            // Register the screen navigator (scoped so it lives for the session)
            services.AddScoped<ScreenNavigator>();

            // Register screens (transient so they're created fresh each time)
            services.AddTransient<ScreenA>();
            services.AddTransient<ScreenB>();

            services.AddTransient<CompositePersonaView>();
            services.AddTransient<HomeView>();

            // Register the command
            services.AddScoped<TuiCommand>();

            // Build and return the container
            return services.BuildServiceProvider(validateScopes: true);
        }
    }
}
