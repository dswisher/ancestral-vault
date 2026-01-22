// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using AncestralVault.Common.Models.Assistants.PersonNames;

namespace AncestralVault.Common.Assistants.PersonNames
{
    public class PersonNameParser : IPersonNameParser
    {
        public PersonNameParseResult Parse(string rawName)
        {
            // TODO - should we have multiple given names (a list/array) instead of a single string?

            // TODO - this is wrong, but a good first start
            var bits = rawName.Split(' ');
            var surname = bits.Last();
            var givenNames = string.Join(' ', bits.Take(bits.Length - 1));

            return new PersonNameParseResult
            {
                GivenNames = givenNames,
                Surname = surname
            };
        }
    }
}
