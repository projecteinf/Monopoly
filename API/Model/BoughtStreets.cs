using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mba.Monopoly {
    public partial class BoughtStreets {
        [MaxLength(20)]
        public string StreetName { get; set; }
        [MaxLength(30)]
        public string GamePlayerName { get; set; }
        public DateTime GameDateTime { get; set;}
        public int numHouses { get; set; }
        public int numHotels { get; set; }
        public bool isMotaged { get; set; }
        public Street? StreetObj { get; set; }
        public Game? GameObj { get; set; }
        public ICollection<PlayerInterchanges>? LPlayerInterchangesObj { get; set; }

        public async Task<ActionResult<BoughtStreets>> Build(DataContext context,Game game,int numHouses,int numHotels) {
            
            EstatePrices? ep = await EstatePrices.GetById(context,this.StreetName,numHouses,numHotels);
            game.Pay(ep!.Price);
            this.Build(numHouses,numHotels);

            context.Entry(game).State = EntityState.Modified;
            context.Entry(this).State = EntityState.Modified;
            await context.SaveChangesAsync(); 
            return this;
        }
    }
}