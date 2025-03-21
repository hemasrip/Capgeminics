using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationSystem.Data;
using RailwayReservationSystem.Models;
using RailwayReservationSystem.Services;

namespace RailwayReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly RailwayContext _context;

        public ReservationsController(RailwayContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservations.ToListAsync();
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.PNRNo)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reservations
        [HttpPost]
        public async Task<ActionResult<ReservationResponseDto>> PostReservation(ReservationDto reservationDto)
        {
            // Step 1: Fetch the train and quota to check seat availability
            var train = await _context.Trains.FindAsync(reservationDto.TrainID);
            if (train == null)
            {
                return BadRequest("Train not found.");
            }

            var quota = await _context.Quotas.FindAsync(reservationDto.QuotaID);
            if (quota == null)
            {
                return BadRequest("Quota not found.");
            }

            // Step 2: Check if enough seats are available
            if (train.SeatsAvailable < reservationDto.SeatsBooked)
            {
                return BadRequest("Not enough seats available on the train.");
            }

            if (quota.SeatsAvailable < reservationDto.SeatsBooked)
            {
                return BadRequest("Not enough seats available in the selected quota.");
            }

            // Step 3: Calculate the total fare using the FareCalculationService
            var fareService = HttpContext.RequestServices.GetService<FareCalculationService>();
            decimal totalFare = fareService.CalculateFare(
                reservationDto.TrainID,
                reservationDto.ClassType,
                quota.QuotaType,
                reservationDto.SeatsBooked
            );

            // Step 4: Map the DTO to the Reservation entity
            var reservation = new Reservation
            {
                PassengerID = reservationDto.PassengerID,
                TrainID = reservationDto.TrainID,
                QuotaID = reservationDto.QuotaID,
                JourneyDate = reservationDto.JourneyDate,
                SeatsBooked = reservationDto.SeatsBooked,
                ClassType = reservationDto.ClassType,
                TotalFare = totalFare,
                PaymentStatus = reservationDto.PaymentStatus,
                CancellationStatus = reservationDto.CancellationStatus
            };
            //payment method
            var paymentService = HttpContext.RequestServices.GetService<PaymentService>();
            bool paymentSuccessful = await paymentService.ProcessPayment(reservation.TotalFare, "CreditCard");

            if (!paymentSuccessful)
            {
                return BadRequest("Payment failed. Please try again.");
            }
            reservation.PaymentStatus = "Completed";

            // Step 5: Update seat availability
            train.SeatsAvailable -= reservationDto.SeatsBooked;
            quota.SeatsAvailable -= reservationDto.SeatsBooked;

            // Step 6: Add the reservation to the database
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Step 7: Map the Reservation to ReservationResponseDto
            var response = new ReservationResponseDto
            {
                PNRNo = reservation.PNRNo,
                PassengerID = reservation.PassengerID,
                TrainID = reservation.TrainID,
                QuotaID = reservation.QuotaID,
                JourneyDate = reservation.JourneyDate,
                SeatsBooked = reservation.SeatsBooked,
                ClassType = reservation.ClassType,
                TotalFare = reservation.TotalFare,
                PaymentStatus = reservation.PaymentStatus,
                CancellationStatus = reservation.CancellationStatus
            };

            return CreatedAtAction("GetReservation", new { id = reservation.PNRNo }, response);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<CancellationResponseDto>> CancelReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Train)
                .Include(r => r.Quota)
                .FirstOrDefaultAsync(r => r.PNRNo == id);

            if (reservation == null) return NotFound("Reservation not found.");
            if (reservation.CancellationStatus == "Cancelled") return BadRequest("Reservation is already cancelled.");
            if (reservation.PaymentStatus != "Completed") return BadRequest("Cannot cancel a reservation with pending or failed payment.");

            var paymentService = HttpContext.RequestServices.GetService<PaymentService>();
            DateTime cancellationDate = DateTime.UtcNow;
            decimal refundAmount = paymentService.CalculateRefund(reservation.TotalFare, reservation.JourneyDate, cancellationDate);

            reservation.CancellationStatus = "Cancelled";
            reservation.Train.SeatsAvailable += reservation.SeatsBooked;
            reservation.Quota.SeatsAvailable += reservation.SeatsBooked;

            await _context.SaveChangesAsync();

            return Ok(new CancellationResponseDto { RefundAmount = refundAmount });
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.PNRNo == id);
        }
    }
}
