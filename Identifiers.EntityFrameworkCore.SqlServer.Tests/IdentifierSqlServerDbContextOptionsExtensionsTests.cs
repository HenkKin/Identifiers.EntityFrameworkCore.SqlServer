using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
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
            var relationalAnnotationProvider = testDbContext.GetService<IRelationalAnnotationProvider>();
            var valueConverterSelector = testDbContext.GetService<IValueConverterSelector>();

            // Assert
            Assert.NotNull(testDbContext);
            Assert.NotEqual(typeof(IdentifierSqlServerAnnotationProvider<>), relationalAnnotationProvider.GetType());
            Assert.Equal(typeof(SqlServerAnnotationProvider), relationalAnnotationProvider.GetType());
            Assert.NotEqual(typeof(IdentifierValueConverterSelector<int>), valueConverterSelector.GetType());
            Assert.Equal(typeof(ValueConverterSelector), valueConverterSelector.GetType());
        }

        [Fact]
        public void WhenCalled_ItShouldRegisterIdentifierSqlServerAnnotationProvider()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<TestDbContext>(builder =>
                builder.UseSqlServer("TestDatabase")
                    .UseIdentifiers<int>());

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var testDbContext = serviceProvider.GetRequiredService<TestDbContext>();

            // Act
            var relationalAnnotationProvider = testDbContext.GetService<IRelationalAnnotationProvider>();

            // Assert
            Assert.NotNull(testDbContext);

            Assert.Equal(typeof(IdentifierSqlServerAnnotationProvider<int>), relationalAnnotationProvider.GetType());
            Assert.NotEqual(typeof(SqlServerAnnotationProvider), relationalAnnotationProvider.GetType());
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
