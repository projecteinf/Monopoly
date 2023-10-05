using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mba.Monopoly;

// GENERAT DE FORMA MANUAL => CLAU COMPOSTA PER PUT I DELETE I GET !!

namespace Monopoly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;

        public GameController(DataContext context) { _context = context; }

        // GET: api/Game
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
          if (_context.Games == null) return NotFound();
          else return await _context.Games.ToListAsync();
        }


        // GET: api/Game/Date
        [HttpGet("{DateTime}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetGame(DateTime DateTime)
        {
            if (_context.Games == null) return NotFound();
            else {
                    var query = from g in _context.Games
                                    where g.DateTime == DateTime
                                    select new {
                                        PlayerName = g.PlayerName,
                                        Posicio = g.Posicio,
                                        Money = g.Money
                                    };
                    return await query.ToListAsync();
            }
        }


        // GET: api/Game/PlayerName/Date
        [HttpGet("{PlayerName}/{DateTime}")]
        public async Task<ActionResult<Game>> GetGame(string PlayerName, DateTime DateTime)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }
            var Game = await _context.Games.FindAsync(PlayerName, DateTime);

            if (Game == null)
            {
                return NotFound();
            }

            return Game;
        }


        [HttpGet("{PlayerName}/{DateTime}/{StreetName}")]
        public async Task<ActionResult<Game>> Buy(string PlayerName, DateTime DateTime, string StreetName)
        {
            if (_context.Players == null || _context.Streets == null) return Problem("Entity set 'DataContext.Players' or 'DataContext.Streets' is null.");

            Game game = await _context.Games.Include(g=>g.LBoughtStreetObj)
                                .FirstOrDefaultAsync(g => g.PlayerName == PlayerName && g.DateTime == DateTime);
            
            Street street = await _context.Streets.FindAsync(StreetName);            
            List<BoughtStreets> boughtStreets = await _context.BoughtStreets.Where(bs => bs.GameDateTime == DateTime && bs.StreetName == StreetName).ToListAsync();
            if (boughtStreets.Count > 0) return BadRequest("This street is already bought.");
            else return await boughtStreet(game, street);            
        }

        private async Task<ActionResult<Game>> boughtStreet(Game game,Street street) {
            game.Money -= street.Price;

            _context.Entry(game).State = EntityState.Modified;

            game.LBoughtStreetObj.Add(
                new BoughtStreets { StreetName = street.Name, GamePlayerName = game.PlayerName, 
                                    GameDateTime = game.DateTime, StreetObj = street, GameObj = game });

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return game;
        }

        // PUT: api/Game/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{playerName}/{dateTime}")]
        public async Task<IActionResult> PutGame(string PlayerName, DateTime DateTime, Game Game)
        {
            if (PlayerName != Game.PlayerName || DateTime != Game.DateTime)
            {
                return BadRequest();
            }

            _context.Entry(Game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(PlayerName, DateTime))
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

        // POST: api/Game
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game Game)
        {
          if (_context.Games == null)
          {
              return Problem("Entity set 'DataContext.Games'  is null.");
          }
            _context.Games.Add(Game);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GameExists(Game.PlayerName, Game.DateTime))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGame", new { PlayerName=Game.PlayerName, DateTime=Game.DateTime }, Game);
        } 

        // DELETE: api/Game/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(string PlayerName, DateTime DateTime)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }
            var Game = await _context.Games.FindAsync(PlayerName, DateTime);
            if (Game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(Game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(string PlayerName, DateTime DateTime)
        {
            return (_context.Games?
                .Any(e => e.PlayerName == PlayerName && e.DateTime==DateTime)).GetValueOrDefault();
        }
    }
}
