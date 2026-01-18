// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.TestCli.Commands;
using AncestralVault.TestCli.Options;
using AncestralVault.TestCli.Options.Common;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AncestralVault.TestCli
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Parse the args
                // TODO: remove object when more options are added
                var parsedArgs = Parser.Default.ParseArguments<
                    DumpPersonaOptions,
                    object>(args);

                // Set up logging
                var logConfig = new LoggerConfiguration()
                    .WriteTo.Console();

                // .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                // .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}");

                if (parsedArgs.Value is ILogOptions lo)
                {
                    if (lo.Verbose)
                    {
                        logConfig.MinimumLevel.Debug();
                    }
                }

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
                        await parsedArgs.WithParsedAsync<DumpPersonaOptions>(options => scope.ServiceProvider.GetRequiredService<DumpPersonaCommand>().ExecuteAsync(options, tokenSource.Token));
                    }
                }

                // No errors!
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
