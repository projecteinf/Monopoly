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
    public class PlayerInterchangesController : ControllerBase
    {
        private readonly DataContext _context;

        public PlayerInterchangesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/PlayerInterchanges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerInterchanges>>> GetPlayerInterchanges()
        {
          if (_context.PlayerInterchanges == null)
          {
              return NotFound();
          }
            return await _context.PlayerInterchanges.ToListAsync();
        }

        // GET: api/PlayerInterchanges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerInterchanges>> GetPlayerInterchanges(string id)
        {
          if (_context.PlayerInterchanges == null)
          {
              return NotFound();
          }
            var playerInterchanges = await _context.PlayerInterchanges.FindAsync(id);

            if (playerInterchanges == null)
            {
                return NotFound();
            }

            return playerInterchanges;
        }

        // PUT: api/PlayerInterchanges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayerInterchanges(string id, PlayerInterchanges playerInterchanges)
        {
            if (id != playerInterchanges.StreetName)
            {
                return BadRequest();
            }

            _context.Entry(playerInterchanges).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerInterchangesExists(id))
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

        // POST: api/PlayerInterchanges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlayerInterchanges>> PostPlayerInterchanges(PlayerInterchanges playerInterchanges)
        {
          if (_context.PlayerInterchanges == null)
          {
              return Problem("Entity set 'DataContext.PlayerInterchanges'  is null.");
          }
            _context.PlayerInterchanges.Add(playerInterchanges);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PlayerInterchangesExists(playerInterchanges.StreetName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPlayerInterchanges", new { id = playerInterchanges.StreetName }, playerInterchanges);
        }

        // DELETE: api/PlayerInterchanges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerInterchanges(string id)
        {
            if (_context.PlayerInterchanges == null)
            {
                return NotFound();
            }
            var playerInterchanges = await _context.PlayerInterchanges.FindAsync(id);
            if (playerInterchanges == null)
            {
                return NotFound();
            }

            _context.PlayerInterchanges.Remove(playerInterchanges);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerInterchangesExists(string id)
        {
            return (_context.PlayerInterchanges?.Any(e => e.StreetName == id)).GetValueOrDefault();
        }
    }
}
