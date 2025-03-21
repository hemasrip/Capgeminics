using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    public class Quota
    {
        [Key]
        public int QuotaID { get; set; }

        [Required]
        public int TrainID { get; set; }

        [Required]
        [StringLength(50)]
        public string QuotaType { get; set; }

        [Required]
        public int SeatsAvailable { get; set; }

        // Navigation property for the related Train
        public Train Train { get; set; }

        // Navigation property for related Reservations
        public List<Reservation> Reservations { get; set; }
    }
}