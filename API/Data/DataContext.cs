using Microsoft.EntityFrameworkCore;

namespace mba.Monopoly
{
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Player> Players { get; set; }
        public DbSet<Street> Streets { get; set; }
    }
}