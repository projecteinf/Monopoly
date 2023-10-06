using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mba.Monopoly {
    public partial class PlayerInterchanges {
        [MaxLength(20)]
        public string StreetName { get; set; } // Street
        [MaxLength(30)]
        public string SellerPlayerName { get; set; }  // BoughtStreets
        public DateTime PlayerDateTime { get; set;} // BoughtStreets
        [MaxLength(30)]
        public string BuyerPlayerName { get; set; } // Game
        //public DateTime GameDateTimeInterchange { get; set;}  // Game
        public DateTime InterchangeDateTime { get; set;}
        public decimal Price { get; set; }
        [NotMapped]
        public BoughtStreets? BoughtStreetsObj { get; set; }
        [NotMapped]
        public Game? GameObj { get; set; }
        
    }
}