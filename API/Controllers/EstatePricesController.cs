using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mba.Monopoly;

namespace Monopoly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatePricesController : ControllerBase
    {
        private readonly DataContext _context;

        public EstatePricesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/EstatePrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstatePrices>>> GetEstatePrices()
        {
          if (_context.EstatePrices == null)
          {
              return NotFound();
          }
            return await _context.EstatePrices.ToListAsync();
        }

        // GET: api/EstatePrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstatePrices>> GetEstatePrices(string id)
        {
          if (_context.EstatePrices == null)
          {
              return NotFound();
          }
            var estatePrices = await _context.EstatePrices.FindAsync(id);

            if (estatePrices == null)
            {
                return NotFound();
            }

            return estatePrices;
        }

        // PUT: api/EstatePrices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstatePrices(string id, EstatePrices estatePrices)
        {
            if (id != estatePrices.StreeName)
            {
                return BadRequest();
            }

            _context.Entry(estatePrices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstatePricesExists(id))
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

        // POST: api/EstatePrices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstatePrices>> PostEstatePrices(EstatePrices estatePrices)
        {
          if (_context.EstatePrices == null)
          {
              return Problem("Entity set 'DataContext.EstatePrices'  is null.");
          }
            _context.EstatePrices.Add(estatePrices);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EstatePricesExists(estatePrices.StreeName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEstatePrices", new { id = estatePrices.StreeName }, estatePrices);
        }

        // DELETE: api/EstatePrices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstatePrices(string id)
        {
            if (_context.EstatePrices == null)
            {
                return NotFound();
            }
            var estatePrices = await _context.EstatePrices.FindAsync(id);
            if (estatePrices == null)
            {
                return NotFound();
            }

            _context.EstatePrices.Remove(estatePrices);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstatePricesExists(string id)
        {
            return (_context.EstatePrices?.Any(e => e.StreeName == id)).GetValueOrDefault();
        }
    }
}
