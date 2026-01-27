// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Assistants.Persons;
using AncestralVault.Common.Assistants.Places;
using AncestralVault.Common.Database;
using AncestralVault.Common.Loaders;
using AncestralVault.Common.Loaders.Impl;
using AncestralVault.Common.Parsers;
using AncestralVault.Common.Repositories;
using AncestralVault.Common.Repositories.Minions;
using AncestralVault.Common.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace AncestralVault.Common
{
    public static class RegistrationExtensions
    {
        public static void RegisterVaultCommon(this IServiceCollection services)
        {
            // Infrastructure
            services.AddSingleton<IAncestralVaultDbContextFactory, AncestralVaultDbContextFactory>();
            services.AddSingleton<IVaultSeeker, VaultSeeker>();

            // Other bits
            services.RegisterAssistants();
            services.RegisterLoaders();
            services.RegisterRepositories();
        }


        public static void RegisterAssistants(this IServiceCollection services)
        {
            services.AddSingleton<IPersonNameParser, PersonNameParser>();
            services.AddSingleton<IPlaceCache, PlaceCache>();
            services.AddSingleton<IPlaceNameParser, PlaceNameParser>();
        }


        public static void RegisterLoaders(this IServiceCollection services)
        {
            services.AddSingleton<ICensusLoader, CensusLoader>();
            services.AddSingleton<ILoaderHelpers, LoaderHelpers>();
            services.AddSingleton<IMarriageLoader, MarriageLoader>();
            services.AddSingleton<ITombstoneLoader, TombstoneLoader>();
            services.AddSingleton<ITypePopulator, TypePopulator>();
            services.AddSingleton<IVaultJsonLoader, VaultJsonLoader>();
            services.AddSingleton<IVaultJsonParser, VaultJsonParser>();
        }


        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ICompositePersonaRepository, CompositePersonaRepository>();
            services.AddSingleton<IDbBrowserRepository, DbBrowserRepository>();
            services.AddSingleton<IPersonaRepository, PersonaRepository>();

            services.AddSingleton<IPersonaMergeMinion, PersonaMergeMinion>();
        }
    }
}
