using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure WeatherForecast entity
        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Date)
                .IsRequired();

            entity.Property(e => e.Summary)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.Condition)
                .HasMaxLength(100);

            entity.Property(e => e.Location)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Humidity)
                .HasPrecision(5, 2);

            entity.Property(e => e.WindSpeed)
                .HasPrecision(5, 2);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // Add indexes
            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => e.Location);
            entity.HasIndex(e => e.IsActive);
        });
    }
}
