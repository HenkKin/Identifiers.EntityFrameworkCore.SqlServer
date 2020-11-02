using Identifiers.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataAccessClientExample.DataLayer
{
    public class GuidDbContext : DbContext
    {
        public GuidDbContext(DbContextOptions<GuidDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExampleEntity>()
                .ToTable("ExampleEntities");
            modelBuilder.Entity<ExampleEntity>()
             .Property(p=>p.Id)
             .IdentifierValueGeneratedOnAdd();


            base.OnModelCreating(modelBuilder);
        }
    }

    internal class GuidDbContextFactory : IDesignTimeDbContextFactory<GuidDbContext>
    {
        public GuidDbContext CreateDbContext(string[] args)
        {
            void OptionsBuilder(DbContextOptionsBuilder x) =>
                x.UseIdentifiers<Guid>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ExampleIdentifiersGuid;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            var services = new ServiceCollection()
                .AddDbContextPool<GuidDbContext>(builder =>
                    OptionsBuilder(builder)
                ); ;

            var serviceProvider = services.BuildServiceProvider();

            var context = serviceProvider.GetRequiredService<GuidDbContext>();
            return context;
        }
    }
}
