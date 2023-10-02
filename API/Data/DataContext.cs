using Microsoft.EntityFrameworkCore;
using mba.Monopoly;

namespace mba.Monopoly
{
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Player> Players { get; set; }
        public DbSet<Street> Streets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new EstatePriceConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new BoughtStreetsConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerInterchangesConfiguration());
        }
        public DbSet<mba.Monopoly.BoughtStreets> BoughtStreets { get; set; } = default!;
        public DbSet<mba.Monopoly.EstatePrices> EstatePrices { get; set; } = default!;
        public DbSet<mba.Monopoly.PlayerInterchanges> PlayerInterchanges { get; set; } = default!;
    }
}