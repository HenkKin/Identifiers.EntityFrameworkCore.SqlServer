using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    internal class IdentifierSqlServerAnnotationProvider<TDatabaseClrType> : SqlServerAnnotationProvider
    {
        public IdentifierSqlServerAnnotationProvider(RelationalAnnotationProviderDependencies dependencies)
            : base(dependencies)
        {
        }

        //// Adjusted implementation of
        //// https://github.com/dotnet/efcore/blob/v5.0.0-rc.2.20475.6/src/EFCore.SqlServer/Metadata/Internal/SqlServerAnnotationProvider.cs#L185
        ///// <summary>
        /////     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        /////     the same compatibility standards as public APIs. It may be changed or removed without notice in
        /////     any release. You should only use it directly in your code with extreme caution and knowing that
        /////     doing so can result in application failures when updating to a new Entity Framework Core release.
        ///// </summary>
        public override IEnumerable<IAnnotation> For(IColumn column)
        {
            //Debugger.Launch();

            var table = StoreObjectIdentifier.Table(column.Table.Name, column.Table.Schema);
            var property = column.PropertyMappings.Where(
                    m =>
                        m.TableMapping.IsSharedTablePrincipal && m.TableMapping.EntityType == m.Property.DeclaringEntityType)
                .Select(m => m.Property)
                .FirstOrDefault(
                    p => p.IsPrimaryKey() && 
                    p.DeclaringEntityType.FindPrimaryKey().Properties.Count() == 1 && // primary key should have one property
                    p.AsProperty().GetValueGeneratedConfigurationSource() == null && // check whether the ValueGenerated is not set explicitly via configuration
                    p.ClrType == typeof(Identifier));
            if (property != null)
            {
                var modelStrategy = property.DeclaringEntityType.Model.GetValueGenerationStrategy();
                var prop = property.AsProperty();
                prop.SetValueGenerated(ValueGenerated.OnAdd, ConfigurationSource.Explicit);

                if (modelStrategy == SqlServerValueGenerationStrategy.IdentityColumn
                   && SqlServerPropertyExtensions.IsCompatibleWithValueGeneration<TDatabaseClrType>(property))
                {
                    prop.SetValueGenerationStrategy<TDatabaseClrType>(modelStrategy); // throws error, because Identifier is not allowed type like int

                }
            }

            return base.For(column);
        }
    }

    /// <summary>
    ///     Extension methods for <see cref="IProperty" /> for SQL Server-specific metadata.
    /// </summary>
    internal static class SqlServerPropertyExtensions
    {

        /// <summary>
        ///     Sets the <see cref="SqlServerValueGenerationStrategy" /> to use for the property.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="value"> The strategy to use. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> The configured value. </returns>
        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        public static SqlServerValueGenerationStrategy? SetValueGenerationStrategy<TDatabaseClrType>(
            [NotNull] this IConventionProperty property,
            SqlServerValueGenerationStrategy? value,
            bool fromDataAnnotation = false)
        {
            CheckValueGenerationStrategy<TDatabaseClrType>(property, value);

            property.SetOrRemoveAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy, value, fromDataAnnotation);

            return value;
        }

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        private static void CheckValueGenerationStrategy<TDatabaseClrType>(IProperty property, SqlServerValueGenerationStrategy? value)
        {
            if (value != null)
            {
                //var propertyType = property.ClrType;
                //var propertyType = property.GetRelationalTypeMapping()?.Converter?.ProviderClrType ?? property.ClrType;
                var propertyType = typeof(TDatabaseClrType);

                if (value == SqlServerValueGenerationStrategy.IdentityColumn
                    && !IsCompatibleWithValueGeneration<TDatabaseClrType>(property))
                {
                    throw new ArgumentException(
                        SqlServerStrings.IdentityBadType(
                            property.Name, property.DeclaringEntityType.DisplayName(), propertyType.ShortDisplayName()));
                }

                if (value == SqlServerValueGenerationStrategy.SequenceHiLo
                    && !IsCompatibleWithValueGeneration<TDatabaseClrType>(property))
                {
                    throw new ArgumentException(
                        SqlServerStrings.SequenceBadType(
                            property.Name, property.DeclaringEntityType.DisplayName(), propertyType.ShortDisplayName()));
                }
            }
        }

        /// <summary>
        ///     Returns a value indicating whether the property is compatible with any <see cref="SqlServerValueGenerationStrategy" />.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> <see langword="true" /> if compatible. </returns>
        public static bool IsCompatibleWithValueGeneration<TDatabaseClrType>([NotNull] IProperty property)
        {
            //var type = property.ClrType;
            // var type = property.GetRelationalTypeMapping()?.Converter?.ProviderClrType ?? property.ClrType;
            var type = typeof(TDatabaseClrType);

            return (type.IsInteger()
                    || type == typeof(decimal))
                && (property.GetValueConverter() ?? property.FindTypeMapping()?.Converter)?.GetType() 
                == typeof(IdentifierValueConverter<>).MakeGenericType(typeof(TDatabaseClrType));
        }

        public static bool IsInteger(this Type type)
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

        public static Type UnwrapNullableType(this Type type)
            => Nullable.GetUnderlyingType(type) ?? type;
    }
}