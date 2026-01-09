using System.ComponentModel.DataAnnotations;

namespace Restaurant.Web.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele complet este obligatoriu.")]
        [StringLength(80, ErrorMessage = "Numele poate avea maxim 80 caractere.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emailul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Email invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefonul este obligatoriu.")]
        [StringLength(20, ErrorMessage = "Telefon prea lung.")]
        public string Phone { get; set; } = string.Empty;

        
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
