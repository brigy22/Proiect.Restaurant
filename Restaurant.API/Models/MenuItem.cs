using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(30)]
        public string Category { get; set; } = "Fel principal"; // Aperitiv, Desert etc.

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}

