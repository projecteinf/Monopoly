using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mba.Monopoly {
    public partial class Street {
        [Key]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Color { get; set; }
        [MaxLength(20)]
        public int Position { get; set; }
        public decimal Price { get; set; }
        public decimal Mortage { get; set; }    // Hipoteca
        public decimal RentPrice { get; set; }
        [NotMapped]
        public ICollection<StreetGroup>? LStreetGroupObj { get; set; } = new List<StreetGroup>();
        [NotMapped]
        public ICollection<StreetGroup>? LStreetGroupObjR { get; set; } = new List<StreetGroup>();
        public ICollection<EstatePrices>? LEstatePricesObj { get; set; }
        public ICollection<BoughtStreets>? LBoughtStreetsObj { get; set; }
    }
}
