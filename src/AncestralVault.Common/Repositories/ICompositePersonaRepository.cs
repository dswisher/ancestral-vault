// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.ViewModels.PersonaDetails;

namespace AncestralVault.Common.Repositories
{
    public interface ICompositePersonaRepository
    {
        Task<PersonaDetailsViewModel?> GetPersonaDetailsAsync(
            AncestralVaultDbContext dbContext,
            string compositePersonaId,
            CancellationToken stoppingToken);
    }
}
