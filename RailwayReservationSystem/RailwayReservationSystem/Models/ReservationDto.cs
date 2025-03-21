namespace RailwayReservationSystem.Models
{
    public class ReservationDto
    {
        public int PassengerID { get; set; }
        public int TrainID { get; set; }
        public int QuotaID { get; set; }
        public DateTime JourneyDate { get; set; }
        public int SeatsBooked { get; set; }
        public string ClassType { get; set; }
        public string PaymentStatus { get; set; }
        public string CancellationStatus { get; set; }
    }
}