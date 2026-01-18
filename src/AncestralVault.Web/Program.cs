// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using AncestralVault.Common;
using AncestralVault.Common.Utilities;
using AncestralVault.Web.Options;
using CommandLine;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AncestralVault.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Parse command line arguments
            var parsedArgs = Parser.Default.ParseArguments<WebOptions>(args);

            parsedArgs.WithParsed(options =>
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container
                builder.Services.AddControllersWithViews();
                builder.Services.RegisterVaultCommon();

                var app = builder.Build();

                // Configure VaultSeeker before app runs
                using (var scope = app.Services.CreateScope())
                {
                    var seeker = scope.ServiceProvider.GetRequiredService<IVaultSeeker>();
                    seeker.Configure(options.VaultPath);
                }

                // Configure the HTTP request pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }

                app.UseStaticFiles();
                app.UseRouting();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.Run();
            });

            parsedArgs.WithNotParsed(errors =>
            {
                // Errors are automatically displayed by CommandLineParser
                Environment.Exit(1);
            });
        }
    }
}
