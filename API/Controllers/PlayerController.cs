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
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers() => await Player.GetAll(_context);
        
        // GET: api/Player/Miquel
        [HttpGet("{Name}")]
        public async Task<ActionResult<Player>> GetPlayer(string name)
        {
            var player = await Player.GetByName(_context, name);
            if (player == null) return NotFound();
            else return player;
        }

        // POST: api/Player
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {           
            try { await Player.Save(_context, player); } 
            catch (DbUpdateException) {
                if (Player.Exists(_context,player.Name)) return Conflict();
                else throw;
            }

            return CreatedAtAction("GetPlayer", new { Name = player.Name }, player);
        }
    }
}