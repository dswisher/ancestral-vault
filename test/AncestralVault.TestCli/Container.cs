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

            // Register all the commands
            services.AddScoped<RebuildCommand>();

            // Build and return the container
            return services.BuildServiceProvider(validateScopes: true);
        }
    }
}
