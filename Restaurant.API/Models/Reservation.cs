using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public DateTime ReservationDateTime { get; set; }

        [Range(1, 20)]
        public int NumberOfPeople { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending / Confirmed / Cancelled

        // FK
        public int ClientId { get; set; }
        public int TableEntityId { get; set; }

        // navigație
        public Client? Client { get; set; }
        public TableEntity? TableEntity { get; set; }

        public Review? Review { get; set; } // 0..1 review
    }
}
