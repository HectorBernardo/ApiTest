using System.ComponentModel.DataAnnotations;

namespace ApiTest.Domain.Entities;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Navigation properties
    public Category Category { get; set; } = null!;
    public ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
}