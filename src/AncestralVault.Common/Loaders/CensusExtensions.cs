// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Models.Loader;
using AncestralVault.Common.Models.VaultJson.CensusUS;

namespace AncestralVault.Common.Loaders
{
    public static class CensusExtensions
    {
        public static LoaderCensusRow ToLoaderRow(this CensusUS1900Row row)
        {
            return new LoaderCensusRow
            {
                Line = row.Line,
                Name = row.Name,
                Relation = row.Relation,
                Sex = row.Sex,
                Color = row.Color,
                Age = row.Age,
                MaritalCondition = row.MaritalCondition,
                BirthPlace = row.BirthPlace,
                DwellingNumber = row.DwellingNumber,
                FamilyNumber = row.FamilyNumber,
                FatherBirthPlace = row.FatherBirthPlace,
                MotherBirthPlace = row.MotherBirthPlace,
                YearsMarried = row.YearsMarried,
                MotherOfHowManyChildren = row.MotherOfHowManyChildren,
                NumberOfTheseChildrenLiving = row.NumberOfTheseChildrenLiving,
                BirthMonth = row.BirthMonth,
                BirthYear = row.BirthYear
            };
        }


        public static LoaderCensusRow ToLoaderRow(this CensusUS1930Row row)
        {
            return new LoaderCensusRow
            {
                Line = row.Line,
                Name = row.Name,
                Relation = row.Relation,
                Sex = row.Sex,
                Color = row.Color,
                Age = row.Age,
                MaritalCondition = row.MaritalCondition,
                BirthPlace = row.BirthPlace,
                DwellingNumber = row.DwellingNumber,
                FamilyNumber = row.FamilyNumber,
                FatherBirthPlace = row.FatherBirthPlace,
                MotherBirthPlace = row.MotherBirthPlace,
                AgeAtFirstMarriage = row.AgeAtFirstMarriage
            };
        }


        public static LoaderCensusRow ToLoaderRow(this CensusUS1940Row row)
        {
            return new LoaderCensusRow
            {
                Line = row.Line,
                Name = row.Name,
                Relation = row.Relation,
                Sex = row.Sex,
                Color = row.Color,
                Age = row.Age,
                MaritalCondition = row.MaritalCondition,
                BirthPlace = row.BirthPlace,
                HouseholdNumber = row.HouseholdNumber
            };
        }
    }
}
