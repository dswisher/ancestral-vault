// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.ViewModels.PersonaDetails;
using AncestralVault.Common.Repositories;
using AncestralVault.Common.Utilities;
using AncestralVault.TestCli.Options;
using Microsoft.Extensions.Logging;

namespace AncestralVault.TestCli.Commands
{
    public class DumpPersonaCommand
    {
        private readonly IVaultSeeker seeker;
        private readonly IPersonaRepository personaRepo;
        private readonly ICompositePersonaRepository compositePersonaRepo;
        private readonly IAncestralVaultDbContextFactory dbContextFactory;
        private readonly ILogger<DumpPersonaCommand> logger;

        public DumpPersonaCommand(
            IVaultSeeker seeker,
            IPersonaRepository personaRepo,
            ICompositePersonaRepository compositePersonaRepo,
            IAncestralVaultDbContextFactory dbContextFactory,
            ILogger<DumpPersonaCommand> logger)
        {
            this.seeker = seeker;
            this.personaRepo = personaRepo;
            this.compositePersonaRepo = compositePersonaRepo;
            this.dbContextFactory = dbContextFactory;
            this.logger = logger;
        }


        public async Task ExecuteAsync(DumpPersonaOptions options, CancellationToken stoppingToken)
        {
            // We must have an ID to dump
            if (string.IsNullOrEmpty(options.PersonaId) && string.IsNullOrEmpty(options.CompositePersonaId))
            {
                logger.LogWarning("You must specify either a Persona ID or Composite Persona ID to dump.");
                return;
            }

            // Keep track of the time
            var timer = Stopwatch.StartNew();

            // Set up the vault info
            seeker.Configure(options.VaultPath);

            // Define the view model that we'll try to populate
            PersonaDetailsViewModel? viewModel = null;

            // Connect to the database
            await using (var context = dbContextFactory.CreateDbContext())
            {
                // Fetch the details
                if (!string.IsNullOrEmpty(options.PersonaId))
                {
                    viewModel = await personaRepo.GetPersonaDetailsAsync(context, options.PersonaId!, stoppingToken);
                }
                else if (!string.IsNullOrEmpty(options.CompositePersonaId))
                {
                    viewModel = await compositePersonaRepo.GetPersonaDetailsAsync(context, options.CompositePersonaId!, stoppingToken);
                }
            }

            // Output the results
            if (viewModel == null)
            {
                logger.LogWarning("No persona found with the specified ID.");
                return;
            }

            WriteDetails(viewModel);

            // Report!
            logger.LogWarning("Done, in {Elapsed}.", timer.Elapsed);
        }


        private static void WriteDetails(PersonaDetailsViewModel viewModel)
        {
            WriteSmallBanner("HEADER");
            WriteHeader(viewModel);

            WriteSmallBanner("EVENTS");
            var first = true;
            foreach (var eventBox in viewModel.EventBoxItems)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Console.WriteLine();
                }

                WriteEventBox(eventBox);
            }

            Console.WriteLine();
        }


        private static void WriteHeader(PersonaDetailsViewModel viewModel)
        {
            Console.WriteLine("Name: {0}", viewModel.Name);
        }


        private static void WriteEventBox(PersonaDetailsEventBox eventBox)
        {
            Console.WriteLine("Event Date: {0}", eventBox.EventDate);
            Console.WriteLine("Event Type: {0} ({1})", eventBox.EventTypeName, eventBox.EventTypeId);
            Console.WriteLine("Event Role: {0} ({1})", eventBox.EventRoleTypeName, eventBox.EventRoleTypeId);
        }


        private static void WriteSmallBanner(string title)
        {
            const int totalWidth = 80;

            var dashes = new string('-', (totalWidth - title.Length - 2) / 2);

            Console.WriteLine();
            Console.WriteLine("{0} {1} {0}", dashes, title);
        }
    }
}
