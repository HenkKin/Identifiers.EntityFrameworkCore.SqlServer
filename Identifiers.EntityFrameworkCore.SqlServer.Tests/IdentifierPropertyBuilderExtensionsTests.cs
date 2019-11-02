using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xunit;

namespace Identifiers.EntityFrameworkCore.SqlServer.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    public class IdentifierPropertyBuilderExtensionsTests
    {
        [Fact]
        public void WhenNotCalled_ItShouldHaveDefaultSituation()
        {
            // Arrange
            var entityType = new EntityType("Entity", new Model(), ConfigurationSource.DataAnnotation);

            // Act
            var result = new PropertyBuilder<Identifier>(new Property("Id", typeof(Identifier), null, null, entityType, ConfigurationSource.Convention, ConfigurationSource.Convention));

            // Assert
            Assert.Equal(ValueGenerated.Never, result.Metadata.ValueGenerated);
            Assert.Null(result.Metadata.FindAnnotation("Identifier"));
            Assert.Equal(PropertySaveBehavior.Save, result.Metadata.GetBeforeSaveBehavior());
        }

        [Fact]
        public void WhenCalled_ItShouldSetValueGeneratedToOnAdd()
        {
            // Arrange
            var entityType = new EntityType("Entity", new Model(), ConfigurationSource.DataAnnotation);
            var propertyBuilder = new PropertyBuilder<Identifier>(new Property("Id", typeof(Identifier), null, null, entityType, ConfigurationSource.Convention, ConfigurationSource.Convention));

            // Act
            var result = propertyBuilder.IdentifierValueGeneratedOnAdd();

            // Assert
            Assert.Equal(ValueGenerated.OnAdd, result.Metadata.ValueGenerated);
        }

        [Fact]
        public void WhenCalled_ItShouldAddIdentifierAnnotation()
        {
            // Arrange
            var entityType = new EntityType("Entity", new Model(), ConfigurationSource.DataAnnotation);
            var propertyBuilder = new PropertyBuilder<Identifier>(new Property("Id", typeof(Identifier), null, null, entityType, ConfigurationSource.Convention, ConfigurationSource.Convention));

            // Act
            var result = propertyBuilder.IdentifierValueGeneratedOnAdd();

            // Assert
            var identifierAnnotation = result.Metadata.FindAnnotation("Identifier");
            Assert.NotNull(identifierAnnotation);
            Assert.Equal("Identifier", identifierAnnotation.Name);
            Assert.Equal(SqlServerValueGenerationStrategy.IdentityColumn, identifierAnnotation.Value);
        }


        [Fact]
        public void WhenCalled_ItShouldSetBeforeSaveBehaviorToIgnore()
        {
            // Arrange
            var entityType = new EntityType("Entity", new Model(), ConfigurationSource.DataAnnotation);
            var propertyBuilder = new PropertyBuilder<Identifier>(new Property("Id", typeof(Identifier), null, null, entityType, ConfigurationSource.Convention, ConfigurationSource.Convention));

            // Act
            var result = propertyBuilder.IdentifierValueGeneratedOnAdd();

            // Assert
            Assert.Equal(PropertySaveBehavior.Ignore, result.Metadata.GetBeforeSaveBehavior());
        }
    }
}
