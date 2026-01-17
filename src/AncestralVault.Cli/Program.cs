// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Cli.Commands;
using AncestralVault.Cli.Options;
using AncestralVault.Cli.Options.Common;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AncestralVault.Cli
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Parse the args
                var parsedArgs = Parser.Default.ParseArguments<
                    LoadFileOptions,
                    RebuildOptions>(args);

                // Set up logging
                var logConfig = new LoggerConfiguration()
                    // .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    // .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                    .WriteTo.Console();

                // .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}");

                if (parsedArgs.Value is ILogOptions lo)
                {
                    if (lo.Verbose)
                    {
                        logConfig.MinimumLevel.Debug();
                    }
                }

                // Create the logger
                Log.Logger = logConfig.CreateLogger();

                // Do the work
                using (var tokenSource = new CancellationTokenSource())
                await using (var provider = Container.CreateContainer())
                {
                    // shut down semi-gracefully on ctrl+c...
                    Console.CancelKeyPress += (_, eventArgs) =>
                    {
                        Log.Warning("*** Cancel event triggered ***");

                        // ReSharper disable once AccessToDisposedClosure
                        tokenSource.Cancel();
                        eventArgs.Cancel = true;
                    };

                    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
                    using (var scope = scopeFactory.CreateScope())
                    {
                        await parsedArgs.WithParsedAsync<LoadFileOptions>(options => scope.ServiceProvider.GetRequiredService<LoadFileCommand>().ExecuteAsync(options, tokenSource.Token));
                        await parsedArgs.WithParsedAsync<RebuildOptions>(options => scope.ServiceProvider.GetRequiredService<RebuildCommand>().ExecuteAsync(options, tokenSource.Token));
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }
    }
}
