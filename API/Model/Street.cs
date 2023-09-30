using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class Street {
        [Key]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
