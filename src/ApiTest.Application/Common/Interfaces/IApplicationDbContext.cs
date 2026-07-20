using ApiTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace ApiTest.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<InventoryMovement> InventoryMovements { get; }
    IDbConnection Connection { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}