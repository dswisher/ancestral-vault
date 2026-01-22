// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AncestralVault.Common.Models.Assistants.PersonNames
{
    public class PersonNameParseResult
    {
        /// <summary>
        /// The prefix, such as "Dr.". Also known as title or prenomial.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// The given name, such as "John David", "Robert", or "Mary Jane". Also known as first name, personal name, or mononame.
        /// </summary>
        /// <remarks>
        /// This may include a middle name, middle initial, or multiple given names.
        /// </remarks>
        public string? GivenNames { get; set; }

        /// <summary>
        /// The surname, such as "Smith" or "Johnson". Also known as last name or family name.
        /// </summary>
        public required string Surname { get; set; }

        /// <summary>
        /// The suffix, such as "Jr". Also known as postnomial.
        /// </summary>
        public string? Suffix { get; set; }
    }
}
