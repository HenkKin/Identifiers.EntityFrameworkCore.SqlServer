using Identifiers.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace DataAccessClientExample.DataLayer
{
    public class IntDbContext : DbContext
    {
        public IntDbContext(DbContextOptions<IntDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExampleIdentifierEntity>()
                .ToTable("ExampleIdentifierEntities");

            modelBuilder.Entity<ExampleIntEntity>()
                .ToTable("ExampleIntEntities");
            //modelBuilder.Entity<ExampleIntEntity>()
            // .Property(p => p.Id)
            // .IdentifierValueGeneratedOnAdd();

            modelBuilder.Entity<ExampleLongEntity>()
                .ToTable("ExampleLongEntities");
            //modelBuilder.Entity<ExampleIntEntity>()
            // .Property(p => p.Id)
            // .IdentifierValueGeneratedOnAdd();

            modelBuilder.Entity<ExampleGuidEntity>()
                .ToTable("ExampleGuidEntities");
            //modelBuilder.Entity<ExampleIntEntity>()
            // .Property(p => p.Id)
            // .IdentifierValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }


    internal class IntDbContextFactory : IDesignTimeDbContextFactory<IntDbContext>
    {
        public IntDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch();
            void OptionsBuilder(DbContextOptionsBuilder x) =>
                x.UseIdentifiers<int>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ExampleIdentifiersInt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            var services = new ServiceCollection()
                .AddDbContextPool<IntDbContext>(builder =>
                    OptionsBuilder(builder)
                ); ;

            var serviceProvider = services.BuildServiceProvider();

            var context = serviceProvider.GetRequiredService<IntDbContext>();
            return context;
        }
    }
}
