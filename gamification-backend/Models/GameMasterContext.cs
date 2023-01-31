using Microsoft.EntityFrameworkCore;

namespace GamificationBackend.Models;

//Vet ikke hva jeg skal med denne enda :P
public class GameMasterContext : DbContext
{
    public GameMasterContext(DbContextOptions<GameMasterContext> options) : base(options)
    {
        
    }

    //public DbSet<GameMaster> GameMaster { get; set; } = null!;
}