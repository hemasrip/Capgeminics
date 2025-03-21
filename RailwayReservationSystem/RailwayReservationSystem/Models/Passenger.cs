using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    public class Passenger
    {
        [Key]
        public int PassengerID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        public int? Age { get; set; }

        [StringLength(50)]
        public string PassengerType { get; set; }

        // Navigation property for the related User
        public User User { get; set; }

        // Navigation property for related Reservations
        public List<Reservation> Reservations { get; set; }
    }
}