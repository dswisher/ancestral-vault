// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AncestralVault.Common.Models.ViewModels
{
    public class SoloEventViewModel
    {
        public required string EventType { get; set; }
        public required string PrincipalPersonaId { get; set; }
        public required string PrincipalRole { get; set; }
        public string? EventDate { get; set; }      // TODO: switch this to a "genealogical date" or some such
    }
}
