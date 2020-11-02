using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    internal class IdentifierSqlServerAnnotationProvider<TDatabaseClrType> : SqlServerAnnotationProvider
    {
        public IdentifierSqlServerAnnotationProvider(RelationalAnnotationProviderDependencies dependencies)
            : base(dependencies)
        {
        }

        // Adjusted implementation of
        // https://github.com/dotnet/efcore/blob/v5.0.0-rc.2.20475.6/src/EFCore.SqlServer/Metadata/Internal/SqlServerAnnotationProvider.cs#L185
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override IEnumerable<IAnnotation> For(IColumn column)
        {
            var table = StoreObjectIdentifier.Table(column.Table.Name, column.Table.Schema);
            var property = column.PropertyMappings.Where(
                    m =>
                        m.TableMapping.IsSharedTablePrincipal && m.TableMapping.EntityType == m.Property.DeclaringEntityType)
                .Select(m => m.Property)
                .FirstOrDefault(
                    p =>p.ClrType == typeof(Identifier) && p.FindAnnotation("Identifier") != null);

            if (property != null)
            {
                var seed = property.GetIdentitySeed(table);
                var increment = property.GetIdentityIncrement(table);

                return new[] { new Annotation(
                        SqlServerAnnotationNames.Identity,
                        string.Format(CultureInfo.InvariantCulture, "{0}, {1}", seed ?? 1, increment ?? 1)) };
            }

            return base.For(column);
        }
    }
}