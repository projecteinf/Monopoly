using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class Street {
        [Key]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Color { get; set; }
        [MaxLength(20)]
        public int Position { get; set; }
        public string? StreetGroupName { get; set; }
        public decimal Price { get; set; }
        public decimal Mortage { get; set; }    // Hipoteca
        public decimal RentPrice { get; set; }
        public ICollection<Street>? LStreetGroupObj { get; set; }
        public ICollection<EstatePrices>? LEstatePricesObj { get; set; }
        public ICollection<BoughtStreets>? LBoughtStreetsObj { get; set; }
        
    }
}
