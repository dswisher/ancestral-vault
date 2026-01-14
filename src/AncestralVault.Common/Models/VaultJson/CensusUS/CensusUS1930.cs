// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace AncestralVault.Common.Models.VaultJson.CensusUS
{
    public class CensusUS1930 : IVaultJsonEntity
    {
        // TODO - add the header

        public List<CensusUS1930Row> Rows { get; set; } = [];
    }
}
