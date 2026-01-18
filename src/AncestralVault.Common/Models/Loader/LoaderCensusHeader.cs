// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AncestralVault.Common.Models.Loader
{
    public class LoaderCensusHeader
    {
        public required string Id { get; set; }
        public required string State { get; set; }
        public required string County { get; set; }
        public required string Township { get; set; }
        public string? IncorporatedPlace { get; set; }
        public required string EnumerationDistrict { get; set; }
        public required string SupervisorsDistrict { get; set; }
        public required string EnumerationDate { get; set; }
        public required string Sheet { get; set; }
    }
}
