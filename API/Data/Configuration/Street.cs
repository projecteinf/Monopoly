using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Licenses;

namespace mba.Monopoly
{
    public class StreetConfiguration : IEntityTypeConfiguration<Street> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Street> builder)
        {
            builder.Property("Price").HasColumnType("decimal(18,2)");
            builder.Property("Mortage").HasColumnType("decimal(18,2)");
            builder.Property("RentPrice").HasColumnType("decimal(18,2)");
            
            List<Street> streets = File.ReadAllLines("Data/Street.csv")
            .Skip(1)
            .Select(line => line.Split(","))
            .Select(fields=> new Street {
                Name = fields[0],
                Color = fields[1],
                StreetGroupName = fields[2],
                Position = int.Parse(fields[3]),
                Price = decimal.Parse(fields[4]),
                Mortage = decimal.Parse(fields[5]),
                RentPrice = decimal.Parse(fields[7])
            })
            .ToList();

            builder.HasData(streets);
        }
    }
}