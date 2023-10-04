using Microsoft.EntityFrameworkCore;
using mba.Monopoly;

namespace mba.Monopoly
{
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Player> Players { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<mba.Monopoly.BoughtStreets> BoughtStreets { get; set; } = default!;
        public DbSet<mba.Monopoly.EstatePrices> EstatePrices { get; set; } = default!;
        public DbSet<mba.Monopoly.PlayerInterchanges> PlayerInterchanges { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new EstatePriceConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new BoughtStreetsConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerInterchangesConfiguration());
        }
        protected override void Seed       
    }
}