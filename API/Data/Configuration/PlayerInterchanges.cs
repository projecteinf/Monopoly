using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;

namespace mba.Monopoly
{
    public class PlayerInterchangesConfiguration : IEntityTypeConfiguration<PlayerInterchanges > {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PlayerInterchanges> builder)
        {
            builder.HasKey(pi => new { pi.StreetName, pi.SellerPlayerName, pi.PlayerDateTime, pi.BuyerPlayerName, pi.InterchangeDateTime });
            builder.Property("Price").HasColumnType("decimal(18,2)");
            builder.HasOne(pi => pi.BoughtStreetsObj)
                .WithMany(bs => bs.LPlayerInterchangesObj)
                .HasForeignKey(pi => new {pi.StreetName, pi.SellerPlayerName, pi.PlayerDateTime}).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(pi => pi.GameObj)
                .WithMany(g => g.LPlayerInterchangesObj)
                .HasForeignKey(pi => new {pi.BuyerPlayerName, pi.PlayerDateTime}).OnDelete(DeleteBehavior.Restrict);
        }
    }
}