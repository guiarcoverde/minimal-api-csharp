using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Infrastructure.Db;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<Administrator> Administrators { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>().HasData(
            new Administrator
            {
                Id = 1,
                Email = "admin@teste.com",
                Password = "123456",
                Profile = "Admin"
            });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        var connectionString = _configuration.GetConnectionString("mysql");

        if (!string.IsNullOrEmpty(connectionString))
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}