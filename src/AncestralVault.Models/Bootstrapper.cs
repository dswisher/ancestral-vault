
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AncestralVault.Models
{
    public class Bootstrapper : IStartupFilter
    {
        private readonly IOptionsMonitor<ModelOptions> options;
        private readonly ILogger logger;


        public Bootstrapper(IOptionsMonitor<ModelOptions> options, ILogger<Bootstrapper> logger)
        {
            this.options = options;
            this.logger = logger;
        }


        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                logger.LogInformation("Running bootstrap code...");
                logger.LogInformation("Model path: '{0}'.", options.CurrentValue.Path);
                next(builder);
            };
        }
    }
}

