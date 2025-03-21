using RailwayReservationSystem.Data;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Services
{
    public class FareCalculationService
    {
        private readonly RailwayContext _context;

        public FareCalculationService(RailwayContext context)
        {
            _context = context;
        }

        public decimal CalculateFare(int trainId, string classType, string quotaType, int seatsBooked)
        {
            // Step 1: Fetch the train to get the base fare
            var train = _context.Trains
                .FirstOrDefault(t => t.TrainID == trainId);

            if (train == null)
            {
                throw new ArgumentException("Train not found.");
            }

            decimal baseFare = train.Fare;

            // Step 2: Apply class type multiplier
            decimal classMultiplier = GetClassMultiplier(classType);
            decimal fareAfterClass = baseFare * classMultiplier;

            // Step 3: Apply quota discount
            decimal quotaDiscount = GetQuotaDiscount(quotaType);
            decimal fareAfterQuota = fareAfterClass * (1 - quotaDiscount);

            // Step 4: Multiply by the number of seats booked
            decimal totalFare = fareAfterQuota * seatsBooked;

            // Step 5: Round to 2 decimal places
            return Math.Round(totalFare, 2);
        }

        private decimal GetClassMultiplier(string classType)
        {
            // Define class type multipliers (example values)
            return classType.ToLower() switch
            {
                "ac1tier" => 2.5m,  // AC 1st Class: 2.5x base fare
                "ac2tier" => 2.0m,  // AC 2-Tier: 2x base fare
                "ac3tier" => 1.5m,  // AC 3-Tier: 1.5x base fare
                "sleeper" => 1.0m,  // Sleeper: 1x base fare
                "general" => 0.8m,  // General: 0.8x base fare
                _ => throw new ArgumentException("Invalid class type.")
            };
        }

        private decimal GetQuotaDiscount(string quotaType)
        {
            // Define quota discounts (example values as percentages)
            return quotaType.ToLower() switch
            {
                "senior citizen" => 0.3m,  // 30% discount
                "ladies" => 0.1m,          // 10% discount
                "tatkal" => 0.0m,          // No discount for Tatkal
                "general" => 0.0m,         // No discount for General
                _ => throw new ArgumentException("Invalid quota type.")
            };
        }
    }
}