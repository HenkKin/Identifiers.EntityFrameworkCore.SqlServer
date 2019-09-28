Identifiers.EntityFrameworkCore.SqlServer
=========================================

[![NuGet](https://img.shields.io/nuget/dt/Identifiers.EntityFrameworkCore.SqlServer.svg)](https://www.nuget.org/packages/Identifiers.EntityFrameworkCore.SqlServer) 
[![NuGet](https://img.shields.io/nuget/vpre/Identifiers.EntityFrameworkCore.SqlServer.svg)](https://www.nuget.org/packages/Identifiers.EntityFrameworkCore.SqlServer)

### Summary

The Identifiers.EntityFrameworkCore.SqlServer library is an extension on [Identifiers](https://github.com/HenkKin/Identifiers/).

This library is Cross-platform, supporting `netstandard2.1`.


### Installing Identifiers.EntityFrameworkCore.SqlServer

You should install [Identifiers.EntityFrameworkCore.SqlServer with NuGet](https://www.nuget.org/packages/Identifiers.EntityFrameworkCore.SqlServer):

    Install-Package Identifiers.EntityFrameworkCore.SqlServer

Or via the .NET Core command line interface:

    dotnet add package Identifiers.EntityFrameworkCore.SqlServer

Either commands, from Package Manager Console or .NET Core CLI, will download and install Identifiers.EntityFrameworkCore.SqlServer and all required dependencies.

### Dependencies

- [Identifiers](https://www.nuget.org/packages/Identifiers/)
- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/)

### Usage

IIf you're using EntityFrameworkCore and you want to use this Identifier type in your entities, then you can use [Identifiers.EntityFrameworkCore.SqlServer](https://github.com/HenkKin/Identifiers.EntityFrameworkCore.SqlServer/) package which includes a `DbContextOptionsBuilder.UseIdentifiers<[InternalClrType:short|int|long|Guid]>()` extension method, allowing you to register all needed IValueConverterSelectors and IMigrationsAnnotationProviders. 
It also includes a `PropertyBuilder<Identifier>.IdentifierValueGeneratedOnAdd()` extension method, allowing you to register all needed configuration to use `SqlServerValueGenerationStrategy.IdentityColumn`. 

To use it:

```csharp
...
using Identifiers.EntityFrameworkCore.SqlServer;

public class Startup
{
    ...
    
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        ...
         services.AddDbContextPool<YourDbContext>((serviceProvider, options) =>
                    options.UseIdentifiers<short|int|long|Guid>()
                );
                
         // or
                
         services.AddDbContext<YourDbContext>((serviceProvider, options) =>
                    options.UseIdentifiers<short|int|long|Guid>()
                );
        ...
    }
    
    ...
```

Using the PropertyBuilder:

```csharp
...
using Identifiers.EntityFrameworkCore.SqlServer;

public class YourDbContext : DbContext
{
    ...
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ...
        modelBuilder.Entity<YourEntity>()
            .Property(e => e.YourIdProperty)
            .IdentifierValueGeneratedOnAdd();
    }
    ...
```
