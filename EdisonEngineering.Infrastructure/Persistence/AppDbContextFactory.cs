using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EdisonEngineering.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 🔷 Build configuration
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../EdisonEngineering.API");

        var configuration = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

        // 🔷 Get connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // 🔷 Configure DbContext
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}