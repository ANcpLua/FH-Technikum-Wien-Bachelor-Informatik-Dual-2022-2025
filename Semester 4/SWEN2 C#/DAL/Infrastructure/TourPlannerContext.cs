using DAL.PersistenceModel;
using Microsoft.EntityFrameworkCore;

namespace DAL.Infrastructure;

public class TourPlannerContext : DbContext
{
    public TourPlannerContext(DbContextOptions<TourPlannerContext> options)
        : base(options)
    {
    }

    public DbSet<TourPersistence> ToursPersistence { get; set; } = null!;
    public DbSet<TourLogPersistence> TourLogsPersistence { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourPersistence>(entity => {
            entity.ToTable("Tours");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(500);
            entity.Property(t => t.From).IsRequired().HasMaxLength(100);
            entity.Property(t => t.To).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Distance).HasColumnType("decimal(18,2)");
            entity.Property(t => t.EstimatedTime);
            entity.Property(t => t.TransportType).HasMaxLength(50);
            entity.Property(t => t.ImagePath).HasMaxLength(10000);
            entity.Property(t => t.RouteInformation).HasMaxLength(30000);

            entity
                .HasMany(t => t.TourLogPersistence)
                .WithOne(tl => tl.TourPersistence)
                .HasForeignKey(tl => tl.TourPersistenceId);
        });

        modelBuilder.Entity<TourLogPersistence>(entity => {
            entity.ToTable("TourLogs");
            entity.HasKey(tl => tl.Id);
            entity
                .Property(tl => tl.DateTime)
                .HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => v.ToUniversalTime()
                );
            entity.Property(tl => tl.Comment).IsRequired().HasMaxLength(500);
            entity.Property(tl => tl.Difficulty);
            entity.Property(tl => tl.Rating);
            entity.Property(tl => tl.TotalDistance).HasColumnType("decimal(18,2)");
            entity.Property(tl => tl.TotalTime);

            entity
                .HasOne(tl => tl.TourPersistence)
                .WithMany(t => t.TourLogPersistence)
                .HasForeignKey(tl => tl.TourPersistenceId);
        });
    }
}
