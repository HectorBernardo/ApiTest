using ApiTest.Domain.Entities;

public interface IProductService
{
    Task DeleteProductAsync(int productId);
    Task UpdateProductAsync(Product product);
    Task CreateProductAsync(Product product);
}