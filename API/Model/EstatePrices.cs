using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class EstatePrices {
        [MaxLength(20)]
        public string StreeName { get; set; }
        public int numberOfHouses { get; set; }
        public int numberOfHotels { get; set; }
        public decimal Price { get; set; }
        public decimal RentPrice { get; set; }
        public virtual Street? StreetObj { get; set; }

        public static async ValueTask<EstatePrices> GetById(DataContext context,string streetName, int numHouses, int numHotels) {
            return await context.EstatePrices.FindAsync(streetName,numHouses,numHotels)!;
        }   
    }
}
