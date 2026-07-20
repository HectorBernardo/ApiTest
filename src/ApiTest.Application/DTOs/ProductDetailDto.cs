namespace ApiTest.Application.DTOs;

public class ProductDetailDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<InventoryMovementDto> InventoryMovements { get; set; } = new();
}

public class InventoryMovementDto
{
    public int MovementId { get; set; }
    public int Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}