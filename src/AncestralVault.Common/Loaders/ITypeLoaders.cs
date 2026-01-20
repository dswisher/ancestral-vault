// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Models.VaultJson;

namespace AncestralVault.Common.Loaders
{
    public interface ITypeLoaders
    {
        void LoadEventRoleType(LoaderContext context, JsonEventRoleType json);
        void LoadEventType(LoaderContext context, JsonEventType json);
        void LoadPlaceType(LoaderContext context, JsonPlaceType json);
    }
}
