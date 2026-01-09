using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress, StringLength(120)]
        public string? Email { get; set; }

        // navigație
        public List<Reservation> Reservations { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
    }
}
