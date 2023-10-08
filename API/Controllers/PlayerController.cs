using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mba.Monopoly;

namespace Monopoly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly DataContext _context;

        public PlayerController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
          if (_context.Players == null)
          {
              return NotFound();
          }
            return await _context.Players.ToListAsync();
        }


        // GET: api/Player/Miquel

        [HttpGet("{Name}")]
        public async Task<ActionResult<Player>> GetPlayer(string name)
        {
          if (_context.Players == null)
          {
              return NotFound();
          }
            var player = await _context.Players.FindAsync(name);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }
        // POST: api/Player
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            if (_context.Players == null) return Problem("Entity set 'DataContext.Players'  is null.");

            _context.Players.Add(player);
            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                if (PlayerExists(player.Name)) return Conflict();
                else throw;
            }

            return CreatedAtAction("GetPlayer", new { Name = player.Name }, player);
        }
        private bool PlayerExists(string name)
        {
            return (_context.Players?.Any(e => e.Name == name)).GetValueOrDefault();
        }
    }
}
