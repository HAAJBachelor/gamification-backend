using System.Reflection;
using gamification_backend.DAL;
using gamification_backend.DBData;
using gamification_backend.Game;
using gamification_backend.Service;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

public class Program
{
    public static int StartTime { get; private set; }

    public static void Main(string[] args)
    {
        StartTime = 600;
        var builder = WebApplication.CreateBuilder(args);

        //set cors based on environment
        builder.Configuration.AddJsonFile(
            builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.Production.json",
            optional: true);
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Singleton);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IGameManager, GameManager>();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.IsEssential = true;
            options.Cookie.MaxAge = TimeSpan.FromMinutes(120);
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File($"Logs/{Assembly.GetExecutingAssembly().GetName().Name}.log")
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        builder.Configuration.AddUserSecrets<Program>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(MyAllowSpecificOrigins);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseSession();

        app.UseWebSockets();

        app.MapControllers();

        DbInitializer.Initialize(app);

        app.Run();
    }
}