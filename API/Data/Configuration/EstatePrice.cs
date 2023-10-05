using Microsoft.EntityFrameworkCore;

namespace mba.Monopoly
{
    public class EstatePriceConfiguration : IEntityTypeConfiguration<EstatePrices> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<EstatePrices> builder)
        {
            builder.HasKey(ep => new { ep.StreeName, ep.numberOfHouses, ep.numberOfHotels });
            builder.HasOne(ep => ep.StreetObj)
                .WithMany(s => s.LEstatePricesObj)
                .HasForeignKey(ep => ep.StreeName).OnDelete(DeleteBehavior.Restrict);

            builder.Property("Price").HasColumnType("decimal(18,2)");
            builder.Property("RentPrice").HasColumnType("decimal(18,2)");

            List<EstatePrices> estatePricesL = new List<EstatePrices>();
            string[] lines = File.ReadAllLines("Data/Street.csv");
            foreach (string line in lines.Skip(1))
            {
                string[] fields = line.Split(",");
                string streetName = fields[0];
                decimal housePrice = decimal.Parse(fields[6]);
                for(int i=8;i<fields.Length-1;i++) {
                    estatePricesL.Add(CrearStatePrice(streetName, i-7, 0, housePrice, decimal.Parse(fields[i])));
                }   
                estatePricesL.Add(CrearStatePrice(streetName, 0, 1, housePrice, decimal.Parse(fields[fields.Length-1])));             
            }
            builder.HasData(estatePricesL);
        }

        private EstatePrices CrearStatePrice(string streetName, int numberOfHouses, int numberOfHotels, decimal housePrice, decimal RentPrice)
        {
            EstatePrices estatePrice = new EstatePrices();
            estatePrice.StreeName = streetName;
            estatePrice.numberOfHouses = numberOfHouses;
            estatePrice.numberOfHotels = numberOfHotels;
            estatePrice.Price = housePrice;
            estatePrice.RentPrice = RentPrice;
            return estatePrice;
        }
    }
}