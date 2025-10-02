using Microsoft.Extensions.Configuration;

namespace CricClubsDataSync.Tests.TestHelpers;

public static class ConfigurationTestHelper
{
    public static IConfiguration CreateTestConfiguration()
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Tests/appsettings.test.json", optional: false)
            .AddEnvironmentVariables();

        return configBuilder.Build();
    }

    public static IConfiguration CreateInMemoryConfiguration(Dictionary<string, string?> configValues)
    {
        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues);

        return configBuilder.Build();
    }

    public static Dictionary<string, string?> GetDefaultTestConfiguration()
    {
        return new Dictionary<string, string?>
        {
            ["CricClubsConfig:ApiBaseUrl"] = "https://test.api.cricclubs.com",
            ["CricClubsConfig:Username"] = "testuser",
            ["CricClubsConfig:Password"] = "testpass",
            ["CricClubsConfig:ApiKey"] = "test-key",
            ["CricClubsConfig:RequestTimeoutSeconds"] = "30",
            ["CricClubsConfig:MaxRetryAttempts"] = "3",
            ["CricClubsConfig:RateLimitDelayMs"] = "1000",
            ["CricClubsConfig:MaxConcurrentRequests"] = "5",
            
            ["DatabaseConfig:ConnectionString"] = "Server=localhost;Database=TestDb;Trusted_Connection=true;",
            ["DatabaseConfig:CommandTimeoutSeconds"] = "30",
            ["DatabaseConfig:EnableSensitiveDataLogging"] = "false",
            ["DatabaseConfig:EnableDetailedErrors"] = "false",
            
            ["SyncOptions:BatchSize"] = "50",
            ["SyncOptions:IncludePlayerStats"] = "true",
            ["SyncOptions:IncludeMatchDetails"] = "true",
            ["SyncOptions:CalculateAnalytics"] = "true",
            ["SyncOptions:SyncHistoricalData"] = "false",
            ["SyncOptions:MatchFormats:0"] = "T20",
            ["SyncOptions:MatchFormats:1"] = "ODI",
            ["SyncOptions:MatchFormats:2"] = "Test"
        };
    }
}