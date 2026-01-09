using System.ComponentModel.DataAnnotations;

namespace Restaurant.API.Models
{
    public class MenuCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu.")]
        [StringLength(50, ErrorMessage = "Numele categoriei poate avea maxim 50 caractere.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Descrierea poate avea maxim 200 caractere.")]
        public string? Description { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
