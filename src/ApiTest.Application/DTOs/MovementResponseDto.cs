using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTest.Application.DTOs
{
    public class MovementResponseDto
    {
        public int MovementId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
