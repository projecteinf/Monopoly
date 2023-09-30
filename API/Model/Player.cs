using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class Player {
        [Key]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}