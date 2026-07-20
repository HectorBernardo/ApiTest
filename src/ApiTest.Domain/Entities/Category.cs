using System.ComponentModel.DataAnnotations;

namespace ApiTest.Domain.Entities;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}