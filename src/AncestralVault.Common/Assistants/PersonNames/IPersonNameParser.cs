// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Models.Assistants.PersonNames;

namespace AncestralVault.Common.Assistants.PersonNames
{
    public interface IPersonNameParser
    {
        PersonNameParseResult Parse(string rawName);
    }
}
