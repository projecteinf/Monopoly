using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class BoughtStreets {
        [MaxLength(20)]
        public string StreetName { get; set; }
        [MaxLength(10)]
        public string GamePlayerName { get; set; }
        public DateTime GameDateTime { get; set;}
        public Street StreetObj { get; set; }
        public Game GameObj { get; set; }
    }
}