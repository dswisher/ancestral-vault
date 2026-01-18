// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace AncestralVault.Common.Models.ViewModels
{
    public class PersonaDetailsViewModel
    {
        public required string Name { get; set; }
        public string? Notes { get; set; }

        public List<SoloEventViewModel> SoloEvents { get; } = [];
    }
}
