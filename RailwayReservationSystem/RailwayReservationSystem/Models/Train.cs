using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    public class Train
    {
        [Key]
        public int TrainID { get; set; }

        [Required]
        [StringLength(100)]
        public string TrainName { get; set; }

        [Required]
        [StringLength(100)]
        public string Source { get; set; }

        [Required]
        [StringLength(100)]
        public string Destination { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public decimal Fare { get; set; }

        [Required]
        public int SeatsAvailable { get; set; }

        // Navigation properties for related Quotas and Reservations
        public List<Quota> Quotas { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}