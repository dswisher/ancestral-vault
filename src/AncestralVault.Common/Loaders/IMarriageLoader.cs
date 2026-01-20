// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Models.VaultJson;

namespace AncestralVault.Common.Loaders
{
    public interface IMarriageLoader
    {
        void LoadMarriage(LoaderContext context, JsonMarriage json);
    }
}
