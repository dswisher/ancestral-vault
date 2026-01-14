// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Loaders;
using AncestralVault.Common.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace AncestralVault.Common
{
    public static class RegistrationExtensions
    {
        public static void RegisterVaultCommon(this IServiceCollection services)
        {
            services.AddSingleton<IVaultJsonLoader, VaultJsonLoader>();
            services.AddSingleton<IVaultJsonParser, VaultJsonParser>();
        }
    }
}
