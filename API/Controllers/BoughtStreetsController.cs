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
    public class BoughtStreetsController : ControllerBase
    {
        private readonly DataContext _context;

        public BoughtStreetsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/BoughtStreets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoughtStreets>>> GetBoughtStreets()
        {
          if (_context.BoughtStreets == null)
          {
              return NotFound();
          }
            return await _context.BoughtStreets.ToListAsync();
        }

        // GET: api/BoughtStreets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoughtStreets>> GetBoughtStreets(string id)
        {
          if (_context.BoughtStreets == null)
          {
              return NotFound();
          }
            var boughtStreets = await _context.BoughtStreets.FindAsync(id);

            if (boughtStreets == null)
            {
                return NotFound();
            }

            return boughtStreets;
        }

        // PUT: api/BoughtStreets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoughtStreets(string id, BoughtStreets boughtStreets)
        {
            if (id != boughtStreets.StreetName)
            {
                return BadRequest();
            }

            _context.Entry(boughtStreets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoughtStreetsExists(id))
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

        // POST: api/BoughtStreets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BoughtStreets>> PostBoughtStreets(BoughtStreets boughtStreets)
        {
          if (_context.BoughtStreets == null)
          {
              return Problem("Entity set 'DataContext.BoughtStreets'  is null.");
          }
            _context.BoughtStreets.Add(boughtStreets);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BoughtStreetsExists(boughtStreets.StreetName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBoughtStreets", new { id = boughtStreets.StreetName }, boughtStreets);
        }

        // DELETE: api/BoughtStreets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoughtStreets(string id)
        {
            if (_context.BoughtStreets == null)
            {
                return NotFound();
            }
            var boughtStreets = await _context.BoughtStreets.FindAsync(id);
            if (boughtStreets == null)
            {
                return NotFound();
            }

            _context.BoughtStreets.Remove(boughtStreets);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoughtStreetsExists(string id)
        {
            return (_context.BoughtStreets?.Any(e => e.StreetName == id)).GetValueOrDefault();
        }
    }
}
