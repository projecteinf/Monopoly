using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class Player {
        [Key]
        [MaxLength(30)]
        public string Name { get; set; }
        public List<Game>? LGamesPlayerObj { get; set; }
    }
}