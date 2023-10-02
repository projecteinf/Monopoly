using Microsoft.EntityFrameworkCore;

namespace mba.Monopoly
{
    public class GameConfiguration : IEntityTypeConfiguration<Game> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(g => new { g.PlayerName, g.DateTime });
            builder.HasOne(g => g.LPlayerObj)
                .WithMany(p => p.LGamesPlayerObj)
                .HasForeignKey(g => g.PlayerName).OnDelete(DeleteBehavior.Restrict);
        }
    }
}