using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.API.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele produsului este obligatoriu.")]
        [StringLength(80, ErrorMessage = "Numele produsului poate avea maxim 80 caractere.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Descrierea poate avea maxim 300 caractere.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Prețul este obligatoriu.")]
        [Range(0.01, 10000, ErrorMessage = "Prețul trebuie să fie mai mare ca 0.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Range(0, 5000, ErrorMessage = "Stocul nu poate fi negativ.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Categoria este obligatorie.")]
        public int MenuCategoryId { get; set; }
        public MenuCategory? MenuCategory { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
