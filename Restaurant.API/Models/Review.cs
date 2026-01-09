using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int ClientId { get; set; }
        public int ReservationId { get; set; }

        // navigație
        public Client? Client { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
