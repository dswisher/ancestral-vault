
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AncestralVault.Models
{
    public static class ModelExtensions
    {
        public static void AddGenTech(this IServiceCollection services)
        {
            // TODO - register our classes

            services.AddTransient<IStartupFilter, Bootstrapper>();
        }
    }
}

