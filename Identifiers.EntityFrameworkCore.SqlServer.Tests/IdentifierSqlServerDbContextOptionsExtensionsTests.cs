using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Identifiers.EntityFrameworkCore.SqlServer.Tests
{
    public class IdentifierSqlServerDbContextOptionsExtensionsTests
    {
        [Fact]
        public void WhenNotCalled_ItShouldHaveDefaultSituation()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<TestDbContext>(builder =>
                builder.UseSqlServer("TestDatabase")
            );

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var testDbContext = serviceProvider.GetRequiredService<TestDbContext>();

            // Act
            var migrationsAnnotationProvider = testDbContext.GetService<IMigrationsAnnotationProvider>();
            var valueConverterSelector = testDbContext.GetService<IValueConverterSelector>();

            // Assert
            Assert.NotNull(testDbContext);
            Assert.NotEqual(typeof(IdentifierMigrationsAnnotationProvider<>), migrationsAnnotationProvider.GetType());
            Assert.Equal(typeof(SqlServerMigrationsAnnotationProvider), migrationsAnnotationProvider.GetType());
            Assert.NotEqual(typeof(IdentifierValueConverterSelector<int>), valueConverterSelector.GetType());
            Assert.Equal(typeof(ValueConverterSelector), valueConverterSelector.GetType());
        }

        [Fact]
        public void WhenCalled_ItShouldRegisterIdentifierMigrationsAnnotationProvider()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<TestDbContext>(builder =>
                builder.UseSqlServer("TestDatabase")
                    .UseIdentifiers<int>());

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var testDbContext = serviceProvider.GetRequiredService<TestDbContext>();

            // Act
            var migrationsAnnotationProvider = testDbContext.GetService<IMigrationsAnnotationProvider>();

            // Assert
            Assert.NotNull(testDbContext);

            Assert.Equal(typeof(IdentifierMigrationsAnnotationProvider<int>), migrationsAnnotationProvider.GetType());
        }


        [Fact]
        public void WhenCalled_ItShouldRegisterIdentifierValueConverterSelector()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<TestDbContext>(builder =>
                builder.UseSqlServer("TestDatabase")
                    .UseIdentifiers<int>());

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var testDbContext = serviceProvider.GetRequiredService<TestDbContext>();

            // Act
            var valueConverterSelector = testDbContext.GetService<IValueConverterSelector>();

            // Assert
            Assert.NotNull(testDbContext);
            Assert.Equal(typeof(IdentifierValueConverterSelector<int>), valueConverterSelector.GetType());
        }

        public class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
            {

            }
        }
    }
}
