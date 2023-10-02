using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class BoughtStreets {
        [MaxLength(20)]
        public string StreetName { get; set; }
        [MaxLength(30)]
        public string GamePlayerName { get; set; }
        public DateTime GameDateTime { get; set;}
        public int numHouses { get; set; }
        public int numHotels { get; set; }
        public Street? StreetObj { get; set; }
        public Game? GameObj { get; set; }
        public ICollection<PlayerInterchanges> LPlayerInterchangesObj { get; set; }
    }
}