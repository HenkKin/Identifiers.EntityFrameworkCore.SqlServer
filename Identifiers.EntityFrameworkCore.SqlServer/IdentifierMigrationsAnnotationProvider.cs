using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    internal class IdentifierMigrationsAnnotationProvider<TDatabaseClrType> : SqlServerMigrationsAnnotationProvider
    {
        public IdentifierMigrationsAnnotationProvider(MigrationsAnnotationProviderDependencies dependencies)
            : base(dependencies)
        {
        }

        // Adjusted implementation of
        // https://github.com/aspnet/EntityFrameworkCore/blob/fe9363252ca5afefaadcef031d612400afe78e27/src/EFCore.SqlServer/Migrations/Internal/SqlServerMigrationsAnnotationProvider.cs
        public override IEnumerable<IAnnotation> For(IProperty property)
        {
            var annotation = property.FindAnnotation("Identifier");
            if (property.ClrType == typeof(Identifier) && annotation != null)
            {
                if (property.GetIdentifierValueGenerationStrategy<TDatabaseClrType>() == SqlServerValueGenerationStrategy.IdentityColumn)
                {
                    var seed = property.GetIdentitySeed();

                    var increment = property.GetIdentityIncrement();

                    return new[]
                    {
                        new Annotation(
                            SqlServerAnnotationNames.Identity,
                            string.Format(CultureInfo.InvariantCulture, "{0}, {1}", seed ?? 1, increment ?? 1))
                    };
                }
            }

            return base.For(property);
        }
    }

    /// <summary>
    /// overrides of https://github.com/aspnet/EntityFrameworkCore/blob/f780a3aef9839131aef118cd73dda625e22fb980/src/EFCore.SqlServer/Extensions/SqlServerPropertyExtensions.cs
    /// </summary>
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    public static class IdentifierSqlServerPropertyExtensions
    {
        public static SqlServerValueGenerationStrategy GetIdentifierValueGenerationStrategy<TDatabaseClrType>([NotNull] this IProperty property)
        {
            var annotation = property[SqlServerAnnotationNames.ValueGenerationStrategy];
            if (annotation != null)
            {
                return (SqlServerValueGenerationStrategy)annotation;
            }

            var sharedTablePrincipalPrimaryKeyProperty = property.FindSharedTableRootPrimaryKeyProperty();
            if (sharedTablePrincipalPrimaryKeyProperty != null)
            {
                return sharedTablePrincipalPrimaryKeyProperty.GetValueGenerationStrategy()
                       == SqlServerValueGenerationStrategy.IdentityColumn
                    ? SqlServerValueGenerationStrategy.IdentityColumn
                    : SqlServerValueGenerationStrategy.None;
            }

            if (property.ValueGenerated != ValueGenerated.OnAdd
                || property.GetDefaultValue() != null
                || property.GetDefaultValueSql() != null
                || property.GetComputedColumnSql() != null)
            {
                return SqlServerValueGenerationStrategy.None;
            }

            var modelStrategy = property.DeclaringEntityType.Model.GetValueGenerationStrategy();

            if (modelStrategy == SqlServerValueGenerationStrategy.SequenceHiLo
                && IsCompatibleWithIdentifierValueGeneration<TDatabaseClrType>(property))
            {
                return SqlServerValueGenerationStrategy.SequenceHiLo;
            }

            return modelStrategy == SqlServerValueGenerationStrategy.IdentityColumn
                   && IsCompatibleWithIdentifierValueGeneration<TDatabaseClrType>(property)
                ? SqlServerValueGenerationStrategy.IdentityColumn
                : SqlServerValueGenerationStrategy.None;
        }

        public static bool IsCompatibleWithIdentifierValueGeneration<TDatabaseClrType>([NotNull] IProperty property)
        {
            var type = typeof(TDatabaseClrType);

            var isCompatible = (type.IsInteger()
                    || type == typeof(decimal))
                   && (property.FindTypeMapping()?.Converter
                       ?? property.GetValueConverter())
                   != null;

            return isCompatible;
        }

        // https://github.com/aspnet/EntityFrameworkCore/blob/f780a3aef9839131aef118cd73dda625e22fb980/src/Shared/SharedTypeExtensions.cs
        private static bool IsInteger(this Type type)
        {
            type = type.UnwrapNullableType();

            return type == typeof(int)
                   || type == typeof(long)
                   || type == typeof(short)
                   || type == typeof(byte)
                   || type == typeof(uint)
                   || type == typeof(ulong)
                   || type == typeof(ushort)
                   || type == typeof(sbyte)
                   || type == typeof(char);
        }
        private static Type UnwrapNullableType(this Type type) => Nullable.GetUnderlyingType(type) ?? type;
    }
}