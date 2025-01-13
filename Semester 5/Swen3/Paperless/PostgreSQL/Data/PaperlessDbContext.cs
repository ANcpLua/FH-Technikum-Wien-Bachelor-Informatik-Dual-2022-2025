using Microsoft.EntityFrameworkCore;
using PostgreSQL.Entities;

namespace PostgreSQL.Data;

public class PaperlessDbContext : DbContext
{
    public PaperlessDbContext(DbContextOptions<PaperlessDbContext> options) : base(options) { }

    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.FilePath).IsRequired(false);
            entity.Property(e => e.DateUploaded).IsRequired();
            entity.Property(e => e.OcrText).IsRequired(false);
        });

        base.OnModelCreating(modelBuilder);
    }
}