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
    public class QuotasController : ControllerBase
    {
        private readonly RailwayContext _context;

        public QuotasController(RailwayContext context)
        {
            _context = context;
        }

        // GET: api/Quotas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quota>>> GetQuotas()
        {
            return await _context.Quotas.ToListAsync();
        }

        // GET: api/Quotas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quota>> GetQuota(int id)
        {
            var quota = await _context.Quotas.FindAsync(id);

            if (quota == null)
            {
                return NotFound();
            }

            return quota;
        }

        // PUT: api/Quotas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuota(int id, Quota quota)
        {
            if (id != quota.QuotaID)
            {
                return BadRequest();
            }

            _context.Entry(quota).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuotaExists(id))
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

        // POST: api/Quotas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quota>> PostQuota(Quota quota)
        {
            _context.Quotas.Add(quota);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuota", new { id = quota.QuotaID }, quota);
        }

        // DELETE: api/Quotas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuota(int id)
        {
            var quota = await _context.Quotas.FindAsync(id);
            if (quota == null)
            {
                return NotFound();
            }

            _context.Quotas.Remove(quota);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuotaExists(int id)
        {
            return _context.Quotas.Any(e => e.QuotaID == id);
        }
    }
}
