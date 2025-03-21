using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}