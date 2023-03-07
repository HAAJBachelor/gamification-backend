using gamification_backend.DBData;
using gamification_backend.Models;

namespace gamification_backend.DAL;

public class DbInitializer
{
    public static void Initialize(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var record = new SessionRecord();
        record.Id = 100;
        record.Score = 10;
        record.Username = "Anon";
        record.Time = 50;

        context.SessionRecords.Add(record);

        context.SaveChanges();
    }
}