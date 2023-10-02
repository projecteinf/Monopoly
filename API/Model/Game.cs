using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class Game {
        [MaxLength(30)]
        public string PlayerName { get; set; }
        public DateTime DateTime { get; set;}
        public int Posicio { get; set; }
        public decimal Money { get; set; }
        public Player LPlayerObj { get; set; }
        public ICollection<BoughtStreets> LBoughtStreetObj { get; set; }
        public ICollection<PlayerInterchanges> LPlayerInterchangesObj { get; set; }
    }
}