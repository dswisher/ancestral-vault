// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Assistants.Dates;

namespace AncestralVault.Common.Models.ViewModels.PersonaDetails
{
    public class PersonaDetailsEventBoxSource
    {
        public required string PersonaId { get; init; }
        public GenealogicalDate? EventDate { get; set; }
    }
}
