using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class PlayerInterchanges {
        [MaxLength(20)]
        public string StreetName { get; set; } // Street
        [MaxLength(30)]
        public string BSPlayerName { get; set; }  // BoughtStreets
        public DateTime BSPlayerDateTime { get; set;} // BoughtStreets
        [MaxLength(30)]
        public string GamePlayerNameInterchange { get; set; } // Game
        //public DateTime GameDateTimeInterchange { get; set;}  // Game
        public DateTime InterchangeDateTime { get; set;}
        public decimal Price { get; set; }
        public BoughtStreets BoughtStreetsObj { get; set; }
        public Game GameObj { get; set; }
        
    }
}