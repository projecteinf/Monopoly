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

        // Obtenir els jugadors que han jugat una partida
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

     
        // Obtenir les propietats d'un jugador en una partida
        // GET: api/Game/PlayerName/Date
        [HttpGet("{PlayerName}/{DateTime}")]
        public async Task<ActionResult<Game>> GetGame(string PlayerName, DateTime DateTime)
        {
            if (_context.Games == null) return NotFound();
            else {
                Game Game = await _context.Games
                                .Include(g=>g.LBoughtStreetObj)
                                .Include(g=>g.LPlayerInterchangesObj)
                                .FirstOrDefaultAsync(g => g.PlayerName == PlayerName && g.DateTime == DateTime);

                if (Game == null) return BadRequest("Game not found");
                else return Game;
            }
            
        }

        // HasStreet - Obtenir si un jugador té un carrer en una partida
        [HttpGet("hasStreet/{PlayerName}/{DateTime}/{StreetName}")]
        public async Task<ActionResult<bool>> HasStreet(string PlayerName, DateTime DateTime, string StreetName)
        {
            if (_context.Games == null) return NotFound();
            
            var Game = await _context.Games.Include(g=>g.LBoughtStreetObj)
                            .FirstOrDefaultAsync(g => g.PlayerName == PlayerName && g.DateTime == DateTime);

            if (Game == null) return BadRequest("Game not found");
            List<PlayerInterchanges> interchanges = _context.PlayerInterchanges.Where(pi => pi.StreetName == StreetName && pi.PlayerDateTime == DateTime).ToList();
            return Game.HasStreet(interchanges,Game.LBoughtStreetObj,StreetName);
        }

        // Comprar un carrer
        [HttpGet("{PlayerName}/{DateTime}/{StreetName}")]
        public async Task<ActionResult<Game>> Buy(string PlayerName, DateTime DateTime, string StreetName)
        {
            Game game = await _context.Games.FirstOrDefaultAsync(g => g.PlayerName == PlayerName && g.DateTime == DateTime);
            Street street = await _context.Streets.FindAsync(StreetName);
            List<BoughtStreets> boughtStreets = await _context.BoughtStreets.Where(bs => bs.GameDateTime == DateTime).ToListAsync();

            if (!game.HasAvailable(street)) return BadRequest("Seller has no money.");
            else
                if (street.Sold(boughtStreets)) return BadRequest("This street is already sold.");
                else return await boughtStreet(game, street);            
        }

        [HttpGet("{Seller}/{Buyer}/{DateTime}/{StreetName}/{Money}")]
        public async Task<ActionResult<PlayerInterchanges>> Interchange(string Seller,string Buyer, DateTime DateTime, string StreetName, decimal Money)
        {
            Game buyerObj = await _context.Games.Include(g=>g.LBoughtStreetObj)
                                .FirstOrDefaultAsync(g => g.PlayerName == Buyer && g.DateTime == DateTime);

            if (buyerObj == null) return NotFound("Buyer not found.");
            if (buyerObj.Money < Money) return BadRequest("Buyer has no money.");

            Game sellerObj = await _context.Games.Include(g=>g.LBoughtStreetObj)
                                .FirstOrDefaultAsync(g => g.PlayerName == Seller && g.DateTime == DateTime);

            List<PlayerInterchanges> interchanges = _context.PlayerInterchanges.Where(pi => pi.StreetName == StreetName && pi.PlayerDateTime == DateTime).ToList();
            if (sellerObj.HasStreet(interchanges,sellerObj.LBoughtStreetObj,StreetName)) {
                Street street = await _context.Streets.FindAsync(StreetName);
                if (street == null) return NotFound("Street not found.");
                return await interchangeStreet(sellerObj, buyerObj, street, Money);
            }
            else return BadRequest("Seller has no street.");
        }
        private bool hasStreet(Game Player, DateTime DateTime, string StreetName) {
            List<PlayerInterchanges> interchanges = _context.PlayerInterchanges.Where(pi => pi.StreetName == StreetName && pi.PlayerDateTime == DateTime).ToList();
            
            if (interchanges.Count > 0) {
                PlayerInterchanges lastInterchange = interchanges.OrderByDescending(pi => pi.InterchangeDateTime).First();
                if (lastInterchange!=null) return lastInterchange.BuyerPlayerName == Player.PlayerName;
                else return false;
            }
            else {
                if (Player.LBoughtStreetObj == null) return false;
                else return Player.LBoughtStreetObj.ToList().Find(bs => bs.StreetName == StreetName) != null;
            } 
        }

        private async Task<ActionResult<PlayerInterchanges>> interchangeStreet(Game seller,Game buyer,Street street,decimal Money) {
            buyer.Money -= Money;
            seller.Money += Money;

            _context.Entry(buyer).State = EntityState.Modified;
            _context.Entry(seller).State = EntityState.Modified;
            
            if (buyer.LBoughtStreetObj == null) buyer.LBoughtStreetObj = new List<BoughtStreets>();
            
            if (buyer.LBoughtStreetObj.ToList().Find(bs => bs.StreetName == street.Name) == null) 
                buyer.LBoughtStreetObj.Add(
                    new BoughtStreets { StreetName = street.Name, GamePlayerName = buyer.PlayerName, 
                                        GameDateTime = buyer.DateTime,
                                        numHotels = 0, numHouses = 0, StreetObj = street, GameObj = buyer });

            
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
            
            if (game.LBoughtStreetObj == null) game.LBoughtStreetObj = new List<BoughtStreets>();
            
            game.LBoughtStreetObj.Add(
                new BoughtStreets { StreetName = street.Name, GamePlayerName = game.PlayerName, 
                                    GameDateTime = game.DateTime, StreetObj = street, GameObj = game });

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return game;
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

        
        private bool GameExists(string PlayerName, DateTime DateTime)
        {
            return (_context.Games?
                .Any(e => e.PlayerName == PlayerName && e.DateTime==DateTime)).GetValueOrDefault();
        }
    }
}
