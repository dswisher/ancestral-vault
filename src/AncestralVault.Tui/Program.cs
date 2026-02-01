// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace AncestralVault.Tui
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Parse the command-line arguments
                // TODO
                var parsedArgs = Parser.Default.ParseArguments<TuiOptions>(args);

                using (var tokenSource = new CancellationTokenSource())
                await using (var provider = Container.CreateContainer())
                {
                    // shut down semi-gracefully on ctrl+c...
                    Console.CancelKeyPress += (_, eventArgs) =>
                    {
                        // TODO - put this back!
                        // Log.Warning("*** Cancel event triggered ***");

                        // ReSharper disable once AccessToDisposedClosure
                        tokenSource.Cancel();
                        eventArgs.Cancel = true;
                    };

                    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
                    using (var scope = scopeFactory.CreateScope())
                    {
                        await parsedArgs.WithParsedAsync(options => scope.ServiceProvider.GetRequiredService<TuiCommand>().ExecuteAsync(options, tokenSource.Token));
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }
    }
}
