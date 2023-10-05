using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;

namespace mba.Monopoly
{
    public class PlayerInterchangesConfiguration : IEntityTypeConfiguration<PlayerInterchanges > {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PlayerInterchanges> builder)
        {
            builder.HasKey(pi => new { pi.StreetName, pi.BSPlayerName, pi.BSPlayerDateTime, pi.GamePlayerNameInterchange, pi.InterchangeDateTime });
            builder.Property("Price").HasColumnType("decimal(18,2)");
            builder.HasOne(pi => pi.BoughtStreetsObj)
                .WithMany(bs => bs.LPlayerInterchangesObj)
                .HasForeignKey(pi => new {pi.StreetName, pi.BSPlayerName, pi.BSPlayerDateTime}).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pi => pi.GameObj)
                .WithMany(g => g.LPlayerInterchangesObj)
                .HasForeignKey(pi => new {pi.GamePlayerNameInterchange, pi.BSPlayerDateTime}).OnDelete(DeleteBehavior.Restrict);
        }
    }
}