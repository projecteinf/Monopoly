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
    public class StreetController : ControllerBase
    {
        private readonly DataContext _context;

        public StreetController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Street
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Street>>> GetStreets()
        {
          if (_context.Streets == null)
          {
              return NotFound();
          }
            return await _context.Streets.ToListAsync();
        }

        // GET: api/Street/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Street>> GetStreet(string id)
        {
          if (_context.Streets == null)
          {
              return NotFound();
          }
            var street = await _context.Streets.FindAsync(id);

            if (street == null)
            {
                return NotFound();
            }

            return street;
        }

        // PUT: api/Street/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStreet(string id, Street street)
        {
            if (id != street.Name)
            {
                return BadRequest();
            }

            _context.Entry(street).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StreetExists(id))
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

        // POST: api/Street
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Street>> PostStreet(Street street)
        {
          if (_context.Streets == null)
          {
              return Problem("Entity set 'DataContext.Streets'  is null.");
          }
            _context.Streets.Add(street);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StreetExists(street.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStreet", new { id = street.Name }, street);
        }

        // DELETE: api/Street/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStreet(string id)
        {
            if (_context.Streets == null)
            {
                return NotFound();
            }
            var street = await _context.Streets.FindAsync(id);
            if (street == null)
            {
                return NotFound();
            }

            _context.Streets.Remove(street);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StreetExists(string id)
        {
            return (_context.Streets?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
