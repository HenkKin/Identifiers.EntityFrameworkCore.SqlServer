using System;
using Identifiers.TypeConverters;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    public class IdentifierValueConverter<TDatabaseClrType> : ValueConverter<Identifier, TDatabaseClrType> where TDatabaseClrType : IConvertible
    {
        public IdentifierValueConverter(ConverterMappingHints mappingHints = null)
            : base(
                id => IdentifierTypeConverter.FromIdentifier<TDatabaseClrType>(id),
                value => IdentifierTypeConverter.ToIdentifier<TDatabaseClrType>(value),
                new ConverterMappingHints(valueGeneratorFactory: (p, t) => new IdentifierValueGenerator<TDatabaseClrType>()) 
            )
        { }
    }
}
