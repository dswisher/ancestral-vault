// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Models.Assistants.PersonNames;
using AncestralVault.Common.Models.VaultDb;

namespace AncestralVault.Common.Loaders
{
    public interface ILoaderHelpers
    {
        string BuildPersonaId(LoaderContext context, string recordId, PersonNameParseResult parsedName);
        Persona AddPersona(LoaderContext context, string? recordId, string personaName, string? personaId = null);

        Task VerifyPersonaExists(LoaderContext context, string personaId, CancellationToken stoppingToken);
        Task VerifyCompositePersonaExists(LoaderContext context, string compositePersonaId, CancellationToken stoppingToken);
    }
}
