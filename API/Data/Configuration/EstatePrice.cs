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
        }
    }
}