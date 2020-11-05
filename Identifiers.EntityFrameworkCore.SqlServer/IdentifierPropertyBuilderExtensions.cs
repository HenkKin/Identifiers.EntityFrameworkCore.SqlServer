using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    public static class IdentifierPropertyBuilderExtensions
    {
        [Obsolete("Using IdentifierValueGeneratedOnAdd is not necessary")]
        public static PropertyBuilder<Identifier> IdentifierValueGeneratedOnAdd(this PropertyBuilder<Identifier> propertyBuilder)
        {
            //propertyBuilder = propertyBuilder
            //    .ValueGeneratedOnAdd()
            //    .HasAnnotation("Identifier", SqlServerValueGenerationStrategy.IdentityColumn);

            //propertyBuilder.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            return propertyBuilder;
        }
    }
}