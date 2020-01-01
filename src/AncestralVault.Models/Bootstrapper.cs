
using System;
using System.IO;
using System.Text.Json;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using AncestralVault.Models.Abstracts;
using AncestralVault.Models.Services;


namespace AncestralVault.Models
{
    public class Bootstrapper : IStartupFilter
    {
        private readonly AbstractLoader abstractLoader;
        private readonly IOptionsMonitor<ModelOptions> options;
        private readonly ILogger logger;


        public Bootstrapper(AbstractLoader abstractLoader, IOptionsMonitor<ModelOptions> options, ILogger<Bootstrapper> logger)
        {
            this.abstractLoader = abstractLoader;
            this.options = options;
            this.logger = logger;
        }


        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                // Process all the files in the model path...
                logger.LogInformation("Loading all files under {0}.", options.CurrentValue.Path);

                var directory = new DirectoryInfo(options.CurrentValue.Path);
                foreach (var file in directory.EnumerateFiles("*.json", SearchOption.AllDirectories))
                {
                    LoadFile(file);
                }

                // Go on to the next startup bit
                next(builder);
            };
        }


        private void LoadFile(FileInfo file)
        {
            logger.LogInformation("Loading {0}..", file.Name);
            using (var stream = file.OpenRead())
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.DeserializeAsync<Abstract>(stream, jsonOptions).Result;

                abstractLoader.Load(data);
            }
        }
    }
}

