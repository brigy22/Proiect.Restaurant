using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class TableEntity
    {
        public int Id { get; set; }

        [Range(1, 500)]
        public int TableNumber { get; set; }

        [Range(1, 20)]
        public int Capacity { get; set; }

        // ex: "Interior", "Terasa"
        [Required, StringLength(30)]
        public string Area { get; set; } = "Interior";

        public bool IsActive { get; set; } = true;

        // navigație
        public List<Reservation> Reservations { get; set; } = new();
    }
}
