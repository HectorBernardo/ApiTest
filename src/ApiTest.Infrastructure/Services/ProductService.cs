using ApiTest.Application.Common.Interfaces;
using ApiTest.Domain.Entities;
using Dapper;

namespace ApiTest.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IApplicationDbContext _context;

    public ProductService(IApplicationDbContext context) => _context = context;

    public async Task CreateProductAsync(Product product)
    {
        const string sql = @"INSERT INTO Products (Name, Description, Price, Stock, CategoryId) 
                         VALUES (@Name, @Description, @Price, @Stock, @CategoryId);
                         SELECT CAST(SCOPE_IDENTITY() as int);";

        var newId = await _context.Connection.QuerySingleAsync<int>(sql, product);
        product.ProductId = newId;
    }

    public async Task DeleteProductAsync(int productId)
    {
        const string sql = "UPDATE Products SET IsDeleted = 1 WHERE ProductId = @ProductId";
        await _context.Connection.ExecuteAsync(sql, new { ProductId = productId });
    }

    public async Task UpdateProductAsync(Product product)
    {
        const string sql = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price WHERE ProductId = @ProductId";
        await _context.Connection.ExecuteAsync(sql, product);
    }
}