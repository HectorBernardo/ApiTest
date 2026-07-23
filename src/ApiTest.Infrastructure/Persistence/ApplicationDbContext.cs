using ApiTest.Application.Common.Interfaces;
using ApiTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ApiTest.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null) return;
        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public IDbConnection Connection => this.Database.GetDbConnection();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration for Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(c => c.CategoryId);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Description).HasMaxLength(250);
        });

        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Tecnología y Electrónica", IsDeleted = true },
            new Category { CategoryId = 2, Name = "Hogar y Línea Blanca", IsDeleted = true },
            new Category { CategoryId = 3, Name = "Deportes", IsDeleted = true }
        );

        // Configuration for Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.ProductId);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Description).HasMaxLength(500);
            entity.Property(p => p.Price).HasPrecision(18, 2);
            entity.Property(p => p.Stock).IsRequired();
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuration for InventoryMovement
        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.ToTable("InventoryMovements");
            entity.HasKey(m => m.MovementId);
            entity.Property(m => m.MovementType).IsRequired().HasMaxLength(20);
            entity.Property(m => m.Reason).HasMaxLength(150);
            entity.Property(m => m.Quantity).IsRequired();
            entity.Property(m => m.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            entity.HasOne(m => m.Product)
                .WithMany(p => p.InventoryMovements)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}