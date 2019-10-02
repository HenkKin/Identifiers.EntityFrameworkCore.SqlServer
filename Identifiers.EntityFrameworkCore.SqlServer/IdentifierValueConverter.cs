using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    internal class IdentifierValueConverter<TDatabaseClrType> : ValueConverter<Identifier, TDatabaseClrType>
    {
        public IdentifierValueConverter(ConverterMappingHints mappingHints = null)
            : base(
                id => (TDatabaseClrType)Convert.ChangeType(id.GetValue(), typeof(TDatabaseClrType)),
                value => new Identifier(value),
                new ConverterMappingHints(valueGeneratorFactory: (p, t) => new IdentifierValueGenerator<TDatabaseClrType>())
            )
        { }
    }
}