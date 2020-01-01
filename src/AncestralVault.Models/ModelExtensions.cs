
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using AncestralVault.Models.InMemory;
using AncestralVault.Models.Services;


namespace AncestralVault.Models
{
    public static class ModelExtensions
    {
        public static void AddGenTech(this IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, Bootstrapper>();

            services.AddSingleton<AbstractLoader>();
            services.AddSingleton<IMemoryRepository, MemoryRepository>();
        }
    }
}

