using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
                
        public static ValueTask<Street> GetByName(DataContext context,string name) =>  context.Streets.FindAsync(name)!;
        public static Task<List<BoughtStreets>> Sold(DataContext context, DateTime dateTime) => context.BoughtStreets.Where(bs => bs.GameDateTime == dateTime).ToListAsync();
        public static Task<Street> GetAllData(DataContext context,string name) => context.Streets
                .Include(s => s.LStreetGroupObjR)
                .Include(p => p.LEstatePricesObj)
                .FirstOrDefaultAsync(s => s.Name==name)!;

    }
}
