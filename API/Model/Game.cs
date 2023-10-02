using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class Game {
        public string PlayerName { get; set; }
        public DateTime DateTime { get; set;}
        public int Posicio { get; set; }
        public decimal Money { get; set; }
        public Player LPlayerObj { get; set; }
        
    }
}