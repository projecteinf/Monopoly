using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mba.Monopoly {
    public partial class Player {
        [Key]
        [MaxLength(30)]
        public string Name { get; set; }
        public List<Game>? LGamesPlayerObj { get; set; }

        public static Task<List<Player>> GetAll(DataContext context) {
            return context.Players.ToListAsync();
        }
        public static ValueTask<Player> GetByName(DataContext context,string name) {
            return context.Players.FindAsync(name)!;
        }
        public static async ValueTask<Player> Save(DataContext context, Player player) {
            context.Players.Add(player);
            await context.SaveChangesAsync(); 
            return player;
        }
        public static bool Exists(DataContext context, string name) => (context.Players?.Any(e => e.Name == name)).GetValueOrDefault();
        
    }
}