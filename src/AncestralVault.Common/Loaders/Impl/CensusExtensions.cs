// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using AncestralVault.Common.Models.Loader;
using AncestralVault.Common.Models.VaultJson.CensusUS;

namespace AncestralVault.Common.Loaders.Impl
{
    public static class CensusExtensions
    {
        public static LoaderCensus ToLoader(this CensusUS1900 census)
        {
            return new LoaderCensus
            {
                Header = census.Header.ToLoader(),
                Rows = census.Rows.Select(r => r.ToLoader()).ToList()
            };
        }


        public static LoaderCensus ToLoader(this CensusUS1930 census)
        {
            return new LoaderCensus
            {
                Header = census.Header.ToLoader(),
                Rows = census.Rows.Select(r => r.ToLoader()).ToList()
            };
        }


        public static LoaderCensus ToLoader(this CensusUS1940 census)
        {
            return new LoaderCensus
            {
                Header = census.Header.ToLoader(),
                Rows = census.Rows.Select(r => r.ToLoader()).ToList()
            };
        }


        private static LoaderCensusHeader ToLoader(this CensusUSHeader header)
        {
            return new LoaderCensusHeader
            {
                Id = header.Id,
                State = header.State,
                County = header.County,
                Township = header.Township,
                IncorporatedPlace = header.IncorporatedPlace,
                EnumerationDistrict = header.EnumerationDistrict,
                SupervisorsDistrict = header.SupervisorsDistrict,
                EnumerationDate = header.EnumerationDate,
                Sheet = header.Sheet,
            };
        }


        private static LoaderCensusRow ToLoader(this CensusUS1900Row row)
        {
            return new LoaderCensusRow
            {
                Id = row.Id,
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


        private static LoaderCensusRow ToLoader(this CensusUS1930Row row)
        {
            return new LoaderCensusRow
            {
                Id = row.Id,
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


        private static LoaderCensusRow ToLoader(this CensusUS1940Row row)
        {
            return new LoaderCensusRow
            {
                Id = row.Id,
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
