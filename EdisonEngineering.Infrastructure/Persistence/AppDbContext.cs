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
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Blog>()
            .HasQueryFilter(x => !x.IsDeleted);

        // Menu self-reference relationship
        modelBuilder.Entity<Menu>()
            .HasMany(m => m.Children)
            .WithOne(m => m.Parent)
            .HasForeignKey(m => m.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // RolePermission -> Permission relationship
        modelBuilder.Entity<RolePermission>()
            .HasOne(x => x.Permission)
            .WithMany()
            .HasForeignKey(x => x.PermissionId);

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

        modelBuilder.Entity<Permission>()
            .HasData(

            new Permission
            {
                Id = 1,
                Name = "Create Blog",
                Code = "blog.create"
            },

            new Permission
            {
                Id = 2,
                Name = "Edit Blog",
                Code = "blog.edit"
            },

            new Permission
            {
                Id = 3,
                Name = "Delete Blog",
                Code = "blog.delete"
            },

            new Permission
            {
                Id = 4,
                Name = "View Leads",
                Code = "lead.view"
            },

            new Permission
            {
                Id = 5,
                Name = "Manage Jobs",
                Code = "job.manage"
            });

        modelBuilder.Entity<RolePermission>()
            .HasData(

            new RolePermission
            {
                Id = 1,
                Role = "Admin",
                PermissionId = 1
            },

            new RolePermission
            {
                Id = 2,
                Role = "Admin",
                PermissionId = 2
            },

            new RolePermission
            {
                Id = 3,
                Role = "Admin",
                PermissionId = 3
            },

            new RolePermission
            {
                Id = 4,
                Role = "Admin",
                PermissionId = 4
            },

            new RolePermission
            {
                Id = 5,
                Role = "Admin",
                PermissionId = 5
            });

    }
}