using Microsoft.EntityFrameworkCore;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Enums;

namespace RedarborStore.Infrastructure.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<InventoryMovement> InventoryMovements { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Price)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Stock)
                .HasDefaultValue(0);

            entity.Property(e => e.CategoryId)
                .IsRequired();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.ToTable("InventoryMovements");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ProductId)
                .IsRequired();

            entity.Property(e => e.MovementType)
                .HasConversion(
                    v => v == MovementType.None ? "" : v.ToString(),
                    v => string.IsNullOrEmpty(v)  ? MovementType.None : (MovementType)Enum.Parse(typeof(MovementType), v) 
                )
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.Property(e => e.Reason)
                .HasMaxLength(500);

            entity.Property(e => e.MovementDate)
                .HasDefaultValueSql("GETDATE()");
        });
    }
}