using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;

namespace mba.Monopoly
{
    public class BoughtStreetsConfiguration : IEntityTypeConfiguration<BoughtStreets> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BoughtStreets> builder)
        {
            builder.HasKey(bs => new { bs.StreetName, bs.GamePlayerName, bs.GameDateTime });
            builder.HasOne(bs => bs.GameObj)
                .WithMany(g => g.LBoughtStreetObj)
                .HasForeignKey(bs => new {bs.GamePlayerName,bs.GameDateTime}).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bs => bs.StreetObj)
                .WithMany(s => s.LBoughtStreetsObj)
                .HasForeignKey(bs => bs.StreetName).OnDelete(DeleteBehavior.Restrict);
        }
    }
}