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
            var Game = await _context.Games.FirstOrDefaultAsync(g => g.PlayerName == PlayerName && g.DateTime == DateTime);

            // List<BoughtStreets> boughtStreets = await getBoughtStreets(PlayerName, DateTime);

            if (Game == null)
            {
                return BadRequest("Game not found");
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
            
            if (game.Money < street.Price ) return BadRequest("Seller has no money.");

            List<BoughtStreets> boughtStreets = await _context.BoughtStreets.Where(bs => bs.GameDateTime == DateTime && bs.StreetName == StreetName).ToListAsync();
            if (boughtStreets.Count > 0) return BadRequest("This street is already bought.");
            else return await boughtStreet(game, street);            
        }

        [HttpGet("{Seller}/{Buyer}/{DateTime}/{StreetName}/{Money}")]
        public async Task<ActionResult<PlayerInterchanges>> Interchange(string Seller,string Buyer, DateTime DateTime, string StreetName, decimal Money)
        {
            if (_context.Players == null || _context.Streets == null) return Problem("Entity set 'DataContext.Players' or 'DataContext.Streets' is null.");

            Game buyerObj = await _context.Games.Include(g=>g.LBoughtStreetObj)
                                .FirstOrDefaultAsync(g => g.PlayerName == Buyer && g.DateTime == DateTime);

            if (buyerObj == null) return NotFound("Buyer not found.");
            if (buyerObj.Money < Money) return BadRequest("Buyer has no money.");

            Game sellerObj = await _context.Games.Include(g=>g.LBoughtStreetObj)
                                .FirstOrDefaultAsync(g => g.PlayerName == Seller && g.DateTime == DateTime);

            List<BoughtStreets> boughtStreets = 
                await _context.BoughtStreets
                    .Where(bs => bs.GameDateTime == DateTime && bs.StreetName == StreetName
                            && bs.GamePlayerName==Seller).ToListAsync();

            List<PlayerInterchanges> playerInterchanges = 
                await _context.PlayerInterchanges
                    .Where(pi => pi.PlayerDateTime == DateTime && pi.StreetName == StreetName).ToListAsync();

            if (playerInterchanges.Count > 0) {
                PlayerInterchanges lastInterchange = playerInterchanges.OrderByDescending(pi => pi.InterchangeDateTime).First();
                if (lastInterchange.BuyerPlayerName != Seller) return BadRequest("Seller has not this street.");
            }
            else if (boughtStreets.Count == 0) return BadRequest("Seller has not this street.");
            

            if (sellerObj == null) return NotFound("Seller not found.");
            if (sellerObj.LBoughtStreetObj.ToList().Find(bs => bs.StreetName == StreetName) == null) return BadRequest("Seller has not this street.");


            Street street = await _context.Streets.FindAsync(StreetName);
            if (street == null) return NotFound("Street not found.");
            
            
            return await interchangeStreet(sellerObj, buyerObj, street, Money);
        }

        private async Task<ActionResult<PlayerInterchanges>> interchangeStreet(Game seller,Game buyer,Street street,decimal Money) {
            buyer.Money -= Money;
            seller.Money += Money;

            _context.Entry(buyer).State = EntityState.Modified;
            _context.Entry(seller).State = EntityState.Modified;
            
            /* buyer.LBoughtStreetObj.Add(
                new BoughtStreets { StreetName = street.Name, GamePlayerName = buyer.PlayerName, 
                                    GameDateTime = buyer.DateTime, StreetObj = street, GameObj = buyer });
 
            seller.LBoughtStreetObj.Remove(seller.LBoughtStreetObj.ToList().Find(bs => bs.StreetName == street.Name));
            */           
            
            PlayerInterchanges playerInterchanges = new PlayerInterchanges { 
                    StreetName = street.Name, 
                    SellerPlayerName = seller.PlayerName, 
                    PlayerDateTime = seller.DateTime,
                    BuyerPlayerName = buyer.PlayerName,
                    InterchangeDateTime = DateTime.Now, 
                    Price = Money
                    //BoughtStreetsObj = buyer.LBoughtStreetObj.ToList().Find(bs => bs.StreetName == street.Name),
                    //GameObj = seller
                    };

            if (buyer.LPlayerInterchangesObj == null) buyer.LPlayerInterchangesObj = new List<PlayerInterchanges>();
            buyer.LPlayerInterchangesObj.Add(playerInterchanges);
            //seller.LPlayerInterchangesObj.Add(playerInterchanges);
            await _context.SaveChangesAsync();  
            //_context.PlayerInterchanges.Add(playerInterchanges);
            //_context.Entry(playerInterchanges).State = EntityState.Modified;
            

            return playerInterchanges;
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
