using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xunit;

namespace Identifiers.EntityFrameworkCore.SqlServer.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    public class IdentifierValueConverterTests
    {
        [Fact]
        public void ConvertFromProviderExpression_WhenIntegerIsProvided_ItShouldReturnIdentifierWithInternalInteger()
        {
            // Arrange
            const int id = 20;

            var identifierValueConverter = new IdentifierValueConverter<int>(new ConverterMappingHints());

            // Act
            var result = identifierValueConverter.ConvertFromProviderExpression.Compile().Invoke(id);

            // Assert
            Assert.Equal(id, result.GetValue());
        }

        [Fact]
        public void ConvertToProviderExpression_WhenIdentifierWithInternalIntegerIsProvided_ItShouldReturnInternalInteger()
        {
            // Arrange
            const int id = 20;
            var identifier = new Identifier(id);

            var identifierValueConverter = new IdentifierValueConverter<int>(new ConverterMappingHints());

            // Act
            var result = identifierValueConverter.ConvertToProviderExpression.Compile().Invoke(identifier);

            // Assert
            Assert.Equal(id, result);
        }


        [Theory]
        [InlineData(typeof(int), 0)]
        [InlineData(typeof(short), 0)]
        [InlineData(typeof(long), 0)]
        [InlineData(typeof(string), null)]
        public void ConvertToProviderExpression_WhenIdentifierWithInternalNullIsProvided_ItShouldReturnDefaultValue(Type type, object expectedValue)
        {
            // Arrange
            var identifier = new Identifier(null);

            var valueConverter = (ValueConverter)Activator.CreateInstance(typeof(IdentifierValueConverter<>).MakeGenericType(type), args: new ConverterMappingHints());
            //var identifierValueConverter = new IdentifierValueConverter<int>(new ConverterMappingHints());

            // Act
            var result = valueConverter.ConvertToProviderExpression.Compile().DynamicInvoke(identifier);

            // Assert
            Assert.Equal(Convert.ChangeType(expectedValue, type), result);
        }
        
        [Fact]
        public void ConvertToProviderExpression_WhenIdentifierWithInternalNullIsProvidedForTypeGuid_ItShouldReturnDefaultGuid()
        {
            // Arrange
            var identifier = new Identifier(null);


            var identifierValueConverter = new IdentifierValueConverter<Guid>(new ConverterMappingHints());

            // Act
            var result = identifierValueConverter.ConvertToProviderExpression.Compile().Invoke(identifier);

            // Assert
            Assert.Equal(Guid.Empty, result);
        }

        [Fact]
        public void ValueGeneratorFactory_WhenValueGeneratedIsNeeded_ItShouldReturnIdentifierValueGenerator()
        {
            // Arrange
            var identifierValueConverter = new IdentifierValueConverter<int>(new ConverterMappingHints());

            var conventionSet = new ConventionSet();
            var entityType = new EntityType(typeof(IdentifierMigrationsAnnotationProviderTests.Entity), new Model(conventionSet), ConfigurationSource.DataAnnotation);
            Property property = new Property("Id", typeof(int), null, null, entityType, ConfigurationSource.DataAnnotation, null);


            // Act
            var result = identifierValueConverter.MappingHints.ValueGeneratorFactory.Invoke(property, entityType);

            // Assert
            Assert.Equal(typeof(IdentifierValueGenerator<int>), result.GetType());
        }
    }
}