// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AncestralVault.Common.Models.Loader
{
    public class LoaderCensusRow
    {
        // Common fields across all census years
        public int Line { get; set; }
        public required string Name { get; set; }
        public required string Relation { get; set; }
        public required string Sex { get; set; }
        public required string Color { get; set; }
        public required string Age { get; set; }
        public required string MaritalCondition { get; set; }
        public required string BirthPlace { get; set; }

        // 1900 & 1930: Dwelling and Family numbering
        public int? DwellingNumber { get; set; }
        public int? FamilyNumber { get; set; }

        // 1940: Household numbering (different from dwelling/family)
        public int? HouseholdNumber { get; set; }

        // Parent birth places (1900 & 1930 only)
        public string? FatherBirthPlace { get; set; }
        public string? MotherBirthPlace { get; set; }

        // 1900 only: Marriage and children information
        public int? YearsMarried { get; set; }
        public int? MotherOfHowManyChildren { get; set; }
        public int? NumberOfTheseChildrenLiving { get; set; }

        // 1900 only: Detailed birth information
        public string? BirthMonth { get; set; }
        public string? BirthYear { get; set; }

        // 1930 only: Marriage information
        public string? AgeAtFirstMarriage { get; set; }
    }
}
