// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace AncestralVault.Common.Models.Loader
{
    public class LoaderCensus
    {
        public required LoaderCensusHeader Header { get; set; }
        public required List<LoaderCensusRow> Rows { get; set; }
    }
}
