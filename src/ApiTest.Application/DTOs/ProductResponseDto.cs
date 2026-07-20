namespace ApiTest.Application.DTOs;

public class ProductResponseDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public CategoryResponseDto? Category { get; set; }
}

public class CategoryResponseDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
}