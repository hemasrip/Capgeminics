using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationSystem.Data;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        private readonly RailwayContext _context;

        public TrainsController(RailwayContext context)
        {
            _context = context;
        }

        // GET: api/Trains
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Train>>> GetTrains()
        {
            return await _context.Trains.ToListAsync();
        }

        // GET: api/Trains/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Train>> GetTrain(int id)
        {
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
            {
                return NotFound();
            }

            return train;
        }

        // PUT: api/Trains/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrain(int id, Train train)
        {
            if (id != train.TrainID)
            {
                return BadRequest();
            }

            _context.Entry(train).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainExists(id))
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

        // POST: api/Trains
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Train>> PostTrain(Train train)
        {
            _context.Trains.Add(train);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrain", new { id = train.TrainID }, train);
        }

        // DELETE: api/Trains/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrain(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }

            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainExists(int id)
        {
            return _context.Trains.Any(e => e.TrainID == id);
        }
    }
}
