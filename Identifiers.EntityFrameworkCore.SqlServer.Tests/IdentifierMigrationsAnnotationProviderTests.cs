using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xunit;

namespace Identifiers.EntityFrameworkCore.SqlServer.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    public class IdentifierMigrationsAnnotationProviderTests
    {
        [Fact]
        public void For_WhenCalledWithoutExistingAnnotation_ItShouldReturnExistingAnnotations()
        {
            // Arrange
            var identifierMigrationsAnnotationProvider = new IdentifierMigrationsAnnotationProvider<int>(new MigrationsAnnotationProviderDependencies());
            var entityType = new EntityType(typeof(object), new Model(), ConfigurationSource.DataAnnotation);
            IProperty property = new Property("id", typeof(Identifier), null, null, entityType, ConfigurationSource.DataAnnotation, null);

            // Act
            var results = identifierMigrationsAnnotationProvider.For(property);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        public void For_WhenCalledForPropertyWithExistingAnnotations_ItShouldReturnExistingAnnotations()
        {
            // Arrange
            var identifierMigrationsAnnotationProvider = new IdentifierMigrationsAnnotationProvider<int>(new MigrationsAnnotationProviderDependencies());

            var conventionSet = new ConventionSet();
            var model = new Model(conventionSet);
            model.AddAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy,
                SqlServerValueGenerationStrategy.IdentityColumn);

            var converter = new IdentifierValueConverter<int>(new ConverterMappingHints(valueGeneratorFactory: (property1, type) => new IdentifierValueGenerator<int>()));

            var property = model.AddEntityType(typeof(Entity), ConfigurationSource.Explicit)
                .AddProperty("Id", typeof(Identifier), ConfigurationSource.Explicit, ConfigurationSource.Explicit);
            property.ValueGenerated = ValueGenerated.OnAdd;
            property.SetValueConverter(converter);
            property.AddAnnotation("Identifier", SqlServerValueGenerationStrategy.IdentityColumn);

            // Act
            var results = identifierMigrationsAnnotationProvider.For(property).ToList();

            // Assert
            Assert.Single(results);
            Assert.Equal("SqlServer:Identity", results.First().Name);
            Assert.Equal("1, 1", results.First().Value);
        }

        public class Entity
        {
            public Identifier Id { get; set; }
        }
    }
}
