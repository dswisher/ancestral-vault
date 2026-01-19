// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AncestralVault.Common.Models.ViewModels.PersonaDetails
{
    public class PersonaDetailsEventBox
    {
        public required string EventTypeId { get; init; }
        public required string EventTypeName { get; init; }
        public required string EventRoleTypeId { get; init; }
        public required string EventRoleTypeName { get; init; }

        public string? EventDate { get; init; }
    }
}
