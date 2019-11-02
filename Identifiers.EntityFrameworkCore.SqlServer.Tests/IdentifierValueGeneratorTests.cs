using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Moq;
using Xunit;

namespace Identifiers.EntityFrameworkCore.SqlServer.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    public class IdentifierValueGeneratorTests
    {
        [Fact]
        public void WhenCalled_ItShouldGenerateTemporaryValues()
        {
            // Act
            var identifierValueGeneratorShort = new IdentifierValueGenerator<short>();
            var identifierValueGeneratorInt = new IdentifierValueGenerator<int>();
            var identifierValueGeneratorLong = new IdentifierValueGenerator<long>();
            var identifierValueGeneratorGuid = new IdentifierValueGenerator<Guid>();
            var identifierValueGeneratorDateTime = new IdentifierValueGenerator<DateTime>();
            var identifierValueGeneratorString = new IdentifierValueGenerator<string>();

            // Assert
            Assert.True(identifierValueGeneratorShort.GeneratesTemporaryValues);
            Assert.True(identifierValueGeneratorInt.GeneratesTemporaryValues);
            Assert.True(identifierValueGeneratorLong.GeneratesTemporaryValues);
            Assert.True(identifierValueGeneratorGuid.GeneratesTemporaryValues);
            Assert.True(identifierValueGeneratorDateTime.GeneratesTemporaryValues);
            Assert.True(identifierValueGeneratorString.GeneratesTemporaryValues);
        }

        [Theory]
        [InlineData(typeof(long), (long.MinValue + 1001))]
        [InlineData(typeof(int), (int.MinValue + 1001))]
        [InlineData(typeof(short), (short)(short.MinValue + 101))]
        public void Next_WhenCalled_ItShouldGeneratedExpectedValue(Type valueType, object expectedValue)
        {
            // Arrange
            var identifierValueGeneratorType = typeof(IdentifierValueGenerator<>).MakeGenericType(valueType);
            var identifierValueGenerator = (ValueGenerator<Identifier>)Activator.CreateInstance(identifierValueGeneratorType);
            var stateManagerMock = new Mock<IStateManager>();

            var entityType = new EntityType(typeof(object), new Model(), ConfigurationSource.DataAnnotation);


            var internalEntityEntry = new InternalClrEntityEntry(stateManagerMock.Object, entityType, new object());
            var entityEntry = new EntityEntry(internalEntityEntry);

            // Act
            var result = identifierValueGenerator.Next(entityEntry);

            // Assert
            Assert.Equal(expectedValue, result.GetValue());
        }

        [Theory]
        [InlineData(typeof(long), (long.MinValue + 1001), (long.MinValue + 1002), (long.MinValue + 1003))]
        [InlineData(typeof(int), (int.MinValue + 1001), (int.MinValue + 1002), (int.MinValue + 1003))]
        [InlineData(typeof(short), (short)(short.MinValue + 101), (short)(short.MinValue + 102), (short)(short.MinValue + 103))]
        public void Next_WhenCalledThreeTimes_ItShouldGeneratedExpectedValues(Type valueType, object expectedValue1, object expectedValue2, object expectedValue3)
        {
            // Arrange
            var identifierValueGeneratorType = typeof(IdentifierValueGenerator<>).MakeGenericType(valueType);
            var identifierValueGenerator = (ValueGenerator<Identifier>)Activator.CreateInstance(identifierValueGeneratorType);
            var stateManagerMock = new Mock<IStateManager>();

            var entityType = new EntityType(typeof(object), new Model(), ConfigurationSource.DataAnnotation);


            var internalEntityEntry = new InternalClrEntityEntry(stateManagerMock.Object, entityType, new object());
            var entityEntry = new EntityEntry(internalEntityEntry);

            // Act
            var result1 = identifierValueGenerator.Next(entityEntry);
            var result2 = identifierValueGenerator.Next(entityEntry);
            var result3 = identifierValueGenerator.Next(entityEntry);

            // Assert
            Assert.Equal(expectedValue1, result1.GetValue());
            Assert.Equal(expectedValue2, result2.GetValue());
            Assert.Equal(expectedValue3, result3.GetValue());
        }
    }
}
