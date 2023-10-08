using Microsoft.EntityFrameworkCore;
using mba.Monopoly;

namespace mba.Monopoly
{
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { 
        }
        public DbSet<Player> Players { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<BoughtStreets> BoughtStreets { get; set; } = default!;
        public DbSet<EstatePrices> EstatePrices { get; set; } = default!;
        public DbSet<PlayerInterchanges> PlayerInterchanges { get; set; } = default!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new StreetConfiguration());
            modelBuilder.ApplyConfiguration(new StreetGroupConfiguration());
            modelBuilder.ApplyConfiguration(new EstatePriceConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new BoughtStreetsConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerInterchangesConfiguration());
            
        }
      
    }
}