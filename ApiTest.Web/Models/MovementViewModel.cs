namespace ApiTest.Web.Models
{
    public class MovementViewModel
    {
        public int MovementId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public string MovementType { get; set; } = null!; // "Input" o "Output"
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
