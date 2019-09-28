using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    public static class IdentifierSqlServerDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseIdentifiers<TDatabaseClrType>(this DbContextOptionsBuilder optionsBuilder) where TDatabaseClrType : IConvertible
        {
            optionsBuilder.ReplaceService<IMigrationsAnnotationProvider, IdentifierIdentityAnnotationProvider>();
            optionsBuilder.ReplaceService<IValueConverterSelector, IdentifierValueConverterSelector<TDatabaseClrType>>();

            return optionsBuilder;
        }
    }
}
