﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    public static class IdentifierSqlServerDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseIdentifiers<TDatabaseClrType>(this DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<IRelationalAnnotationProvider, IdentifierSqlServerAnnotationProvider<TDatabaseClrType>>();
            optionsBuilder.ReplaceService<IValueConverterSelector, IdentifierValueConverterSelector<TDatabaseClrType>>();

            return optionsBuilder;
        }
    }
}