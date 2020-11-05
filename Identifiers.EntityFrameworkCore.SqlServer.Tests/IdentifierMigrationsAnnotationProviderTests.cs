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
    public class IdentifierSqlServerAnnotationProviderTests
    {
        //[Fact]
        //public void For_WhenCalledWithoutExistingAnnotation_ItShouldReturnExistingAnnotations()
        //{
        //    // Arrange
        //    var modelBuilder = CreateConventionModelBuilder(skipValidation: true);

        //    modelBuilder.Entity<Entity>()
        //        .HasKey(prop => prop.Id);

        //    var model = modelBuilder.FinalizeModel().GetRelationalModel();

        //    var entityType = model.Model.FindEntityType(typeof(Entity));
        //    var tableMapping = entityType.GetTableMappings().Single();

        //    var column = tableMapping.Table.Columns.Single(c => c.Name == nameof(Entity.Id));

        //    var identifierMigrationsAnnotationProvider = new IdentifierSqlServerAnnotationProvider<int>(new RelationalAnnotationProviderDependencies());

        //    // Act
        //    var results = identifierMigrationsAnnotationProvider.For(column).ToList();

        //    // Assert
        //    Assert.Empty(results);
        //}

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        public void For_WhenCalledForPropertyWithExistingAnnotations_ItShouldReturnExistingAnnotations()
        {
            // Arrange
            var modelBuilder = CreateConventionModelBuilder(skipValidation:true);

            modelBuilder.Entity<Entity>()
                .HasKey(prop => prop.Id);

            var model = modelBuilder.FinalizeModel().GetRelationalModel();

            var entityType = model.Model.FindEntityType(typeof(Entity));
            var tableMapping = entityType.GetTableMappings().Single();

            var column = tableMapping.Table.Columns.Single(c=>c.Name == nameof(Entity.Id));

            var identifierMigrationsAnnotationProvider = new IdentifierSqlServerAnnotationProvider<int>(new RelationalAnnotationProviderDependencies());

            // Act
            var results = identifierMigrationsAnnotationProvider.For(column).ToList();

            // Assert
            Assert.Single(results);
            Assert.Equal("SqlServer:Identity", results.First().Name);
            Assert.Equal("1, 1", results.First().Value);
        }

        public class Entity
        {
            public Identifier Id { get; set; }
        }

        protected virtual ModelBuilder CreateConventionModelBuilder(bool skipValidation = false)
            => SqlServerTestHelpers.Instance.CreateConventionBuilder(skipValidation: skipValidation);
    }
}
