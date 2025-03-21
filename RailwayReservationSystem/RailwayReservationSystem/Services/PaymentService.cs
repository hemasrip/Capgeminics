namespace RailwayReservationSystem.Services
{
    public class PaymentService
    {
        public async Task<bool> ProcessPayment(decimal amount, string paymentMethod)
        {
            await Task.Delay(1000); // Simulate network delay
            Random random = new Random();
            bool paymentSuccessful = random.NextDouble() < 0.8; // 80% success rate
            return paymentSuccessful;
        }
        public decimal CalculateRefund(decimal totalFare, DateTime journeyDate, DateTime cancellationDate)
        {
            TimeSpan timeDifference = journeyDate - cancellationDate;
            double hoursBeforeJourney = timeDifference.TotalHours;

            if (hoursBeforeJourney > 48) return totalFare * 0.8m; // 80% refund
            else if (hoursBeforeJourney > 0) return totalFare * 0.5m; // 50% refund
            else return 0m; // No refund after journey date
        }
    }
}