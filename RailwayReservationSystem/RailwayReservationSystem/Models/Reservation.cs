using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    public class Reservation
    {
        [Key]
        public int PNRNo { get; set; }

        [Required]
        public int PassengerID { get; set; }

        [Required]
        public int TrainID { get; set; }

        [Required]
        public int QuotaID { get; set; }

        [Required]
        public DateTime JourneyDate { get; set; }

        [Required]
        public int SeatsBooked { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassType { get; set; }

        [Required]
        public decimal TotalFare { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentStatus { get; set; }

        [Required]
        [StringLength(20)]
        public string CancellationStatus { get; set; }

        // Navigation properties for related entities
        public Passenger Passenger { get; set; }
        public Train Train { get; set; }
        public Quota Quota { get; set; }
    }
}