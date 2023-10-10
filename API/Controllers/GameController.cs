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
        public async Task<ActionResult<IEnumerable<Game>>> GetGames() => await Game.GetAll(_context);
        

        // Obtenir els jugadors que han jugat una partida
        // GET: api/Game/Date
        [HttpGet("{DateTime}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetGame(DateTime DateTime) => await Game.GetPlayersInfo(_context, DateTime);
     
        // Obtenir les propietats d'un jugador en una partida
        // GET: api/Game/PlayerName/Date
        [HttpGet("{PlayerName}/{DateTime}")]
        public async Task<ActionResult<Game>> GetGame(string PlayerName, DateTime DateTime)
        {
            Game game=await Game.GetPlayerTransactions(_context, PlayerName, DateTime);
            if (game == null) return BadRequest("Game not found");
            else return game;
        }

        // HasStreet - Obtenir si un jugador té un carrer en una partida
        [HttpGet("hasStreet/{PlayerName}/{DateTime}/{StreetName}")]
        public async Task<ActionResult<bool>> HasStreet(string PlayerName, DateTime DateTime, string StreetName)
        {
            Game? game = await Game.GetPlayerTransactions(_context, PlayerName, DateTime);

            if (game == null) return BadRequest("Game not found");
            else return game.HasStreet(game.LPlayerInterchangesObj!.ToList(),game.LBoughtStreetObj,StreetName);
        }

        // Comprar un carrer
        [HttpGet("{PlayerName}/{DateTime}/{StreetName}")]
        public async Task<ActionResult<Game>> Buy(string PlayerName, DateTime DateTime, string StreetName)
        {
            Game? game = await Game.GetGameById(_context, PlayerName, DateTime);
            Street? street = await Street.GetByName(_context, StreetName);

            if (!game.EnoughMoney(street)) return BadRequest("Seller has no money.");
            else {
                List<BoughtStreets> boughtStreets = await Street.Sold(_context,DateTime);
                if (street.isSold(boughtStreets)) return BadRequest("This street is already sold.");
                else return await game.Buy(_context,street);
            }
                
        }

        [HttpGet("BuildHouse/{PlayerName}/{DateTime}/{StreetName}")]
        public async Task<ActionResult<BoughtStreets>> BuildHouse(string playerName, DateTime dateTime, string streetName)
        {
            Game? game = await Game.GetPlayerTransactions(_context, playerName, dateTime);
            Street? street = await Street.GetAllData(_context, streetName);           
            
            bool allBought = game!.boughtAllGroup(game!.LPlayerInterchangesObj!.ToList(),game!.LBoughtStreetObj!.ToList(),street.LStreetGroupObjR!.ToList());
            if (!allBought) return BadRequest("Seller has not all the streets.");
            else {
                List<BoughtStreets> boughtStreets = await Street.Sold(_context,dateTime);
                if (!game!.EnoughMoney(boughtStreets!.Find(bs=>bs.StreetName==streetName)!, street.LEstatePricesObj!.ToList())) return BadRequest("Seller has no money.");
                else return await boughtStreets!.Find(bs=>bs.StreetName==streetName && bs.GameDateTime==dateTime)!.Build(_context,game,1,0);
            }
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
        
        
        private bool GameExists(string PlayerName, DateTime DateTime)
        {
            return (_context.Games?
                .Any(e => e.PlayerName == PlayerName && e.DateTime==DateTime)).GetValueOrDefault();
        }
    }
}
