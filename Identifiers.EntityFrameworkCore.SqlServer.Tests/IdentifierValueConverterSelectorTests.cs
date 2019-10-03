using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xunit;

namespace Identifiers.EntityFrameworkCore.SqlServer.Tests
{
    public class IdentifierValueConverterSelectorTests
    {
        [Fact]
        public void WhenCalledForIdentifier_ItShouldReturnAValueConverterInfo()
        {
            // Arrange
            var identifierValueConverterSelector = new IdentifierValueConverterSelector<int>(new ValueConverterSelectorDependencies());

            // Act
            var results = identifierValueConverterSelector.Select(typeof(Identifier)).ToList();

            // Assert
            Assert.Equal(typeof(int), results.Single().ProviderClrType);
            Assert.Equal(typeof(Identifier), results.Single().ModelClrType);
            Assert.Null(results.Single().MappingHints);
        }

        [Fact]
        public void WhenCalledForNullableIdentifier_ItShouldReturnAValueConverterInfo()
        {
            // Arrange
            var identifierValueConverterSelector = new IdentifierValueConverterSelector<int>(new ValueConverterSelectorDependencies());

            // Act
            var results = identifierValueConverterSelector.Select(typeof(Identifier?)).ToList();

            // Assert
            Assert.Equal(typeof(int), results.Single().ProviderClrType);
            Assert.Equal(typeof(Identifier?), results.Single().ModelClrType);
            Assert.Null(results.Single().MappingHints);
        }

        [Fact]
        public void WhenCalledForNullableIdentifierAndWithProviderType_ItShouldReturnAValueConverterInfo()
        {
            // Arrange
            var identifierValueConverterSelector = new IdentifierValueConverterSelector<int>(new ValueConverterSelectorDependencies());

            // Act
            var results = identifierValueConverterSelector.Select(typeof(Identifier?), typeof(int)).ToList();

            // Assert
            Assert.Equal(typeof(int), results.Single().ProviderClrType);
            Assert.Equal(typeof(Identifier?), results.Single().ModelClrType);
            Assert.Null(results.Single().MappingHints);
        }
        
        [Fact]
        public void WhenCalledForIdentifierAndFactoryIsUsed_ItShouldCreateAValueConverter()
        {
            // Arrange
            var identifierValueConverterSelector = new IdentifierValueConverterSelector<int>(new ValueConverterSelectorDependencies());
            var results = identifierValueConverterSelector.Select(typeof(Identifier)).ToList();
            var valueConverterInfo = results.Single();

            // Act
            var valueConverter = valueConverterInfo.Create();

            // Assert
            Assert.NotNull(valueConverter);
            Assert.NotNull(valueConverter.MappingHints);
            Assert.Equal(typeof(int), valueConverter.ProviderClrType);
            Assert.Equal(typeof(Identifier), valueConverter.ModelClrType);
            Assert.Null(results.Single().MappingHints);
        }

        [Fact]
        public void WhenCalledForInteger_ItShouldReturnDefaultValueConverterInfos()
        {
            // Arrange
            var identifierValueConverterSelector = new IdentifierValueConverterSelector<int>(new ValueConverterSelectorDependencies());

            // Act
            var results = identifierValueConverterSelector.Select(typeof(int)).ToList();

            // Assert
            Assert.Equal(12, results.Count);
            Assert.True(results.All(x=>x.ModelClrType == typeof(int)));
        }
    }
}
