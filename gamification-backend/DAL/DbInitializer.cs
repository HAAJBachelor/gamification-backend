using gamification_backend.DBData;

namespace gamification_backend.DAL;

public class DbInitializer
{
    public static void Initialize(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        context.SaveChanges();
    }
}