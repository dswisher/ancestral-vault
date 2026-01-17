// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Models.VaultJson;

namespace AncestralVault.Common.Parsers
{
    public interface IVaultJsonParser
    {
        Task<List<IVaultJsonEntity>> LoadVaultJsonEntitiesAsync(FileInfo file, bool validateProps, CancellationToken stoppingToken);
    }
}
