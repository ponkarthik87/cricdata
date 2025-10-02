using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CricClubsDataSync.Configuration;

namespace CricClubsDataSync.Services;

public class ConsoleHostedService : BackgroundService
{
    private readonly ILogger<ConsoleHostedService> _logger;
    private readonly CricClubsConfig _cricClubsConfig;
    private readonly DatabaseConfig _databaseConfig;
    private readonly SyncOptions _syncOptions;
    private readonly IHostApplicationLifetime _appLifetime;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IOptions<CricClubsConfig> cricClubsConfig,
        IOptions<DatabaseConfig> databaseConfig,
        IOptions<SyncOptions> syncOptions,
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _cricClubsConfig = cricClubsConfig.Value;
        _databaseConfig = databaseConfig.Value;
        _syncOptions = syncOptions.Value;
        _appLifetime = appLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("CricClubs Data Sync service starting...");
            
            // TODO: Implement main synchronization logic
            _logger.LogInformation("Configuration loaded successfully");
            _logger.LogInformation("API Base URL: {ApiBaseUrl}", _cricClubsConfig.ApiBaseUrl);
            _logger.LogInformation("Database Connection configured: {HasConnection}", !string.IsNullOrEmpty(_databaseConfig.ConnectionString));
            
            // Simulate async work for now
            await Task.Delay(100, stoppingToken);
            
            _logger.LogInformation("CricClubs Data Sync service completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during sync execution");
        }
        finally
        {
            // Stop the application
            _appLifetime.StopApplication();
        }
    }
}