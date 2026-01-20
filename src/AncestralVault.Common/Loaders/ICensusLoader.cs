// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Models.Loader;

namespace AncestralVault.Common.Loaders
{
    public interface ICensusLoader
    {
        void LoadCensus(LoaderContext context, LoaderCensus census);
    }
}
