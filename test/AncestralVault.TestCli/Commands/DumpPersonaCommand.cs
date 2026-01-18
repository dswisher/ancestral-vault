// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.ViewModels;
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

            logger.LogInformation("Persona Details:");
            logger.LogInformation("  Name: {Name}", viewModel.Name);

            if (!string.IsNullOrEmpty(viewModel.Notes))
            {
                logger.LogInformation(" Notes: {Notes}", viewModel.Notes);
            }

            foreach (var soloEvent in viewModel.SoloEvents)
            {
                logger.LogInformation(" Solo Event: {EventType} ({PrincipalRole}), date {EventDate}",
                    soloEvent.EventType,
                    soloEvent.PrincipalRole,
                    soloEvent.EventDate ?? "unknown");
            }

            // Report!
            logger.LogWarning("Done, in {Elapsed}.", timer.Elapsed);
        }
    }
}
