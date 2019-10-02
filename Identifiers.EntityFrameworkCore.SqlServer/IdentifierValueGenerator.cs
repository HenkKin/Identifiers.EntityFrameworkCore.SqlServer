using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    internal class IdentifierValueGenerator<TDatabaseClrType> : ValueGenerator<Identifier>
    {
        private readonly ValueGenerator _valueGenerator;

        public IdentifierValueGenerator()
        {
            if (typeof(TDatabaseClrType) == typeof(short))
            {
                _valueGenerator = new TemporaryShortValueGenerator();
            }
            else if (typeof(TDatabaseClrType) == typeof(int))
            {
                _valueGenerator = new TemporaryIntValueGenerator();
            }
            else if (typeof(TDatabaseClrType) == typeof(long))
            {
                _valueGenerator = new TemporaryLongValueGenerator();
            }
            else if (typeof(TDatabaseClrType) == typeof(Guid))
            {
                _valueGenerator = new TemporaryGuidValueGenerator();
            }
            else
            {
                _valueGenerator = new TemporaryIntValueGenerator();
            }
        }

        public override Identifier Next(EntityEntry entry)
        {
            Identifier id = new Identifier(_valueGenerator.Next(entry));
            return id;
        }

        public override bool GeneratesTemporaryValues
            => true;
    }
}