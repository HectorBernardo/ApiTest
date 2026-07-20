using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTest.Domain.Entities;

public class InventoryMovement
{
    [Key]
    public int MovementId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } // Positive for input, negative for output
    public string MovementType { get; set; } = string.Empty; // "Input" or "Output"
    public string Reason { get; set; } = string.Empty; // e.g., "Purchase", "Sale", "Damaged"
    public DateTime CreatedAt { get; set; }

    // Navigation property
    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;
}