using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace mba.Monopoly {
    public partial class Game {
        [MaxLength(30)]
        public string PlayerName { get; set; }
        public DateTime DateTime { get; set;}
        public int Posicio { get; set; }
        public decimal Money { get; set; }
        public Player? LPlayerObj { get; set; }
        public ICollection<BoughtStreets>? LBoughtStreetObj { get; set; }
        public ICollection<PlayerInterchanges>? LPlayerInterchangesObj { get; set; }
        public static Task<List<Game>> GetAll(DataContext context) => context.Games.ToListAsync();
        
        public static Task<Game> GetGameById(DataContext context, string playerName, DateTime dateTime) 
            => context.Games.FirstOrDefaultAsync(g => g.PlayerName == playerName && g.DateTime == dateTime)!;
        public static async Task<List<dynamic>> GetPlayersInfo(DataContext context, DateTime dateTime)
        {
            var query = from g in context.Games
                        where g.DateTime == dateTime
                        select new { PlayerName = g.PlayerName, Posicio = g.Posicio, Money = g.Money };
                        
            var result = await query.ToListAsync();
            return result.Cast<dynamic>().ToList();
        }
        public static async Task<Game> GetPlayerTransactions(DataContext context, string playerName, DateTime dateTime)
        {
            Game? game = await context.Games
                            .Include(g=>g.LBoughtStreetObj)
                            .Include(g=>g.LPlayerInterchangesObj)
                            .FirstOrDefaultAsync(g => g.PlayerName == playerName && g.DateTime == dateTime);
            
            return game!;
        }

        public async Task<ActionResult<Game>> Buy(DataContext context,Street street) {
            this.Pay(street.Price);

            context.Entry(this).State = EntityState.Modified;
            if (this.LBoughtStreetObj == null) this.LBoughtStreetObj = new List<BoughtStreets>();
            
            this.LBoughtStreetObj.Add(
                new BoughtStreets { StreetName = street.Name, GamePlayerName = this.PlayerName, 
                                    GameDateTime = this.DateTime, StreetObj = street, GameObj = this });

            try { await context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return this;
        }
    }
}