using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        // Navigation property for related Passengers
        public List<Passenger> Passengers { get; set; }
    }
}