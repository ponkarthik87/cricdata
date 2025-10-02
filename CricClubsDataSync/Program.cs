using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using CricClubsDataSync.Configuration;

namespace CricClubsDataSync;

class Program
{
    static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/cricclubs-sync-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting CricClubs Data Sync application");

            var host = CreateHostBuilder(args).Build();
            
            // Run the application
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                // Configure configuration objects
                services.Configure<CricClubsConfig>(context.Configuration.GetSection("CricClubsConfig"));
                services.Configure<DatabaseConfig>(context.Configuration.GetSection("DatabaseConfig"));
                services.Configure<SyncOptions>(context.Configuration.GetSection("SyncOptions"));

                // Add HTTP client
                services.AddHttpClient();

                // Add hosted service for console application
                services.AddHostedService<Services.ConsoleHostedService>();

                // TODO: Add Entity Framework DbContext
                // TODO: Add repository services
                // TODO: Add business services
            });
}
