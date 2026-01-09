using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.API.Models
{
    public enum OrderStatus
    {
        New = 0,
        Preparing = 1,
        Ready = 2,
        Delivered = 3,
        Cancelled = 4
    }

    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.New;

        [Required(ErrorMessage = "Clientul este obligatoriu.")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Range(0, 100000, ErrorMessage = "Total invalid.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
