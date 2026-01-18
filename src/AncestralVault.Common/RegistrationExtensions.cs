// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Common.Loaders;
using AncestralVault.Common.Parsers;
using AncestralVault.Common.Repositories;
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
            services.RegisterLoaders();
            services.RegisterRepositories();
        }


        public static void RegisterLoaders(this IServiceCollection services)
        {
            services.AddSingleton<ICensusLoader, CensusLoader>();
            services.AddSingleton<IVaultJsonLoader, VaultJsonLoader>();
            services.AddSingleton<IVaultJsonParser, VaultJsonParser>();
        }


        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ICompositePersonaRepository, CompositePersonaRepository>();
            services.AddSingleton<IPersonaRepository, PersonaRepository>();
        }
    }
}
