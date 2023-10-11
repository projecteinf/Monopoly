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
            List<PlayerInterchanges> LInterchangesObj = await Street.Interchanges(_context,StreetName,DateTime);
            if (game == null) return BadRequest("Game not found");
            else return game.HasStreet(LInterchangesObj!.ToList(),game.LBoughtStreetObj,StreetName);
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
            List<PlayerInterchanges> LPlayerInterchangesObj = await PlayerInterchanges.GetAll(_context, dateTime);
            Street? street = await Street.GetAllData(_context, streetName);           
            
            bool allBought = game!.boughtAllGroup(LPlayerInterchangesObj!.ToList(),game!.LBoughtStreetObj!.ToList(),street.LStreetGroupObjR!.ToList());
            if (!allBought) return BadRequest("Seller has not all the streets.");
            else {
                List<BoughtStreets> boughtStreets = await Street.Sold(_context,dateTime);
                if (!game!.EnoughMoney(boughtStreets!.Find(bs=>bs.StreetName==streetName)!, street.LEstatePricesObj!.ToList())) return BadRequest("Seller has no money.");
                else return await boughtStreets!.Find(bs=>bs.StreetName==streetName && bs.GameDateTime==dateTime && bs.GamePlayerName==playerName)!.Build(_context,game,1,0);
            }
        }

        
        [HttpGet("{Seller}/{Buyer}/{DateTime}/{StreetName}/{Money}")]
        public async Task<ActionResult<PlayerInterchanges>> Interchange(string seller,string buyer, DateTime dateTime, string streetName, decimal money)
        {
            Game buyerObj = await Game.GetPlayerTransactions(_context, buyer, dateTime);

            if (buyerObj == null) return NotFound("Buyer not found.");
            else if (buyerObj.EnoughMoney(money)) return BadRequest("Buyer has no money.");

            Game sellerObj = await Game.GetPlayerTransactions(_context,seller,dateTime);

            List<PlayerInterchanges> interchanges = await Street.Interchanges(_context,streetName,dateTime);
            if (sellerObj.HasStreet(interchanges,sellerObj.LBoughtStreetObj,streetName)) {
                Street street = await Street.GetByName(_context,streetName);
                if (street == null) return NotFound("Street not found.");
                return await buyerObj.InterchangeStreet(_context, sellerObj, street, money);
            }
            else return BadRequest("Seller has no street.");
        }
        
        // POST: api/Game
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {

            try { await Game.Save(_context, game); } 
            catch (DbUpdateException) {
                if (Game.Exists(_context,game.PlayerName,game.DateTime)) return Conflict();
                else throw;
            }

            return CreatedAtAction("GetGame", new { PlayerName=game.PlayerName, DateTime=game.DateTime }, game);
        } 



        
        private bool GameExists(string PlayerName, DateTime DateTime)
        {
            return (_context.Games?
                .Any(e => e.PlayerName == PlayerName && e.DateTime==DateTime)).GetValueOrDefault();
        }
    }
}
