// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace AncestralVault.Common.Models.ViewModels.PersonaDetails
{
    public class PersonaDetailsViewModel
    {
        public required string Name { get; set; }

        public List<PersonaDetailsEventBox> EventBoxItems { get; } = [];
    }
}
