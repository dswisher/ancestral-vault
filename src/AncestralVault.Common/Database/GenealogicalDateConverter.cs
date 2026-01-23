// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Assistants.Dates;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AncestralVault.Common.Database
{
    public class GenealogicalDateConverter : ValueConverter<GenealogicalDate?, string?>
    {
        public GenealogicalDateConverter()
            : base(
                v => v == null ? null : v.ToString(),
                v => v == null ? null : GenealogicalDate.Parse(v))
        {
        }
    }
}
