using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xunit;

namespace Identifiers.EntityFrameworkCore.SqlServer.Tests
{
    public class IdentifierMigrationsAnnotationProviderTests
    {
        [Fact]
        public void For_WhenCalledWithoutExistingAnnotation_ItShouldReturnExistingAnnotations()
        {
            // Arrange
            var identifierMigrationsAnnotationProvider = new IdentifierMigrationsAnnotationProvider(new MigrationsAnnotationProviderDependencies());
            var entityType = new EntityType(typeof(object), new Model(), ConfigurationSource.DataAnnotation);
            IProperty property = new Property("id", typeof(int), null, null, entityType, ConfigurationSource.DataAnnotation, null);

            // Act
            var results = identifierMigrationsAnnotationProvider.For(property);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void For_WhenCalledForPropertyWithExistingAnnotations_ItShouldReturnExistingAnnotations()
        {
            // Arrange
            var identifierMigrationsAnnotationProvider = new IdentifierMigrationsAnnotationProvider(new MigrationsAnnotationProviderDependencies());

            var conventionSet = new ConventionSet();
            var entityType = new EntityType(typeof(Entity), new Model(conventionSet), ConfigurationSource.DataAnnotation);
            
            Property property = new Property("Id", typeof(int), null, null, entityType, ConfigurationSource.DataAnnotation, null);
            property.AddAnnotation("Identifier", 12);

            // Act
            var results = identifierMigrationsAnnotationProvider.For(property).ToList();

            // Assert
            Assert.Single(results);
            Assert.Equal("SqlServer:ValueGenerationStrategy", results.First().Name);
            Assert.Equal(12, results.First().Value);
        }

        public class Entity
        {
            public int Id { get; set; }
        }
    }
}
