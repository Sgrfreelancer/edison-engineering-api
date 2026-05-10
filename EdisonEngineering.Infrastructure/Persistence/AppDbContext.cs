using Microsoft.EntityFrameworkCore;
using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Menu> Menus { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Lead> Leads { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<AppConfig> AppConfigs { get; set; }
    public DbSet<CityPricing> CityPricings { get; set; }
    public DbSet<Subsidy> Subsidies { get; set; }
    public DbSet<ElectricitySlab> ElectricitySlabs { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Menu self-reference relationship
        modelBuilder.Entity<Menu>()
            .HasMany(m => m.Children)
            .WithOne(m => m.Parent)
            .HasForeignKey(m => m.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // CityPricing decimal precision
        modelBuilder.Entity<CityPricing>()
            .Property(x => x.CostPerKW)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CityPricing>()
            .Property(x => x.RatePerUnit)
            .HasPrecision(18, 2);

        // ElectricitySlab decimal precision
        modelBuilder.Entity<ElectricitySlab>()
            .Property(x => x.Rate)
            .HasPrecision(18, 2);

        // Subsidy decimal precision
        modelBuilder.Entity<Subsidy>()
            .Property(x => x.MinKW)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Subsidy>()
            .Property(x => x.MaxKW)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Subsidy>()
            .Property(x => x.SubsidyAmountPerKW)
            .HasPrecision(18, 2);
    }
}