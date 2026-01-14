// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace AncestralVault.Common.Database
{
    public class DeferrableForeignKeysSqlGenerator : SqliteMigrationsSqlGenerator
    {
        public DeferrableForeignKeysSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
            IRelationalAnnotationProvider migrationsAnnotations)
            : base(dependencies, migrationsAnnotations)
        {
        }


        protected override void ForeignKeyConstraint(
            AddForeignKeyOperation operation,
            IModel? model,
            MigrationCommandListBuilder builder)
        {
            base.ForeignKeyConstraint(operation, model, builder);
            builder.Append(" DEFERRABLE INITIALLY DEFERRED");
        }
    }
}
