using CricClubsDataSync.Configuration;
using CricClubsDataSync.Tests.TestHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace CricClubsDataSync.Tests.Configuration;

public class ConfigurationBindingTests
{
    [Fact]
    public void CricClubsConfig_ShouldBindFromConfiguration()
    {
        // Arrange
        var configValues = new Dictionary<string, string?>
        {
            ["CricClubsConfig:ApiBaseUrl"] = "https://custom.api.com",
            ["CricClubsConfig:Username"] = "customuser",
            ["CricClubsConfig:Password"] = "custompass",
            ["CricClubsConfig:ApiKey"] = "custom-key",
            ["CricClubsConfig:RequestTimeoutSeconds"] = "120",
            ["CricClubsConfig:MaxRetryAttempts"] = "10",
            ["CricClubsConfig:RateLimitDelayMs"] = "2000",
            ["CricClubsConfig:MaxConcurrentRequests"] = "15"
        };

        var configuration = ConfigurationTestHelper.CreateInMemoryConfiguration(configValues);
        var services = new ServiceCollection();
        services.Configure<CricClubsConfig>(configuration.GetSection("CricClubsConfig"));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetRequiredService<IOptions<CricClubsConfig>>();
        var config = options.Value;

        // Assert
        config.ApiBaseUrl.ShouldBe("https://custom.api.com");
        config.Username.ShouldBe("customuser");
        config.Password.ShouldBe("custompass");
        config.ApiKey.ShouldBe("custom-key");
        config.RequestTimeoutSeconds.ShouldBe(120);
        config.MaxRetryAttempts.ShouldBe(10);
        config.RateLimitDelayMs.ShouldBe(2000);
        config.MaxConcurrentRequests.ShouldBe(15);
    }

    [Fact]
    public void DatabaseConfig_ShouldBindFromConfiguration()
    {
        // Arrange
        var configValues = new Dictionary<string, string?>
        {
            ["DatabaseConfig:ConnectionString"] = "Server=custom;Database=CustomDb;",
            ["DatabaseConfig:CommandTimeoutSeconds"] = "90",
            ["DatabaseConfig:EnableSensitiveDataLogging"] = "true",
            ["DatabaseConfig:EnableDetailedErrors"] = "true"
        };

        var configuration = ConfigurationTestHelper.CreateInMemoryConfiguration(configValues);
        var services = new ServiceCollection();
        services.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetRequiredService<IOptions<DatabaseConfig>>();
        var config = options.Value;

        // Assert
        config.ConnectionString.ShouldBe("Server=custom;Database=CustomDb;");
        config.CommandTimeoutSeconds.ShouldBe(90);
        config.EnableSensitiveDataLogging.ShouldBeTrue();
        config.EnableDetailedErrors.ShouldBeTrue();
    }

    [Fact]
    public void SyncOptions_ShouldBindFromConfiguration()
    {
        // Arrange
        var configValues = new Dictionary<string, string?>
        {
            ["SyncOptions:SeasonIds:0"] = "1",
            ["SyncOptions:SeasonIds:1"] = "2",
            ["SyncOptions:CompetitionIds:0"] = "10",
            ["SyncOptions:TeamIds:0"] = "100",
            ["SyncOptions:TeamIds:1"] = "200",
            ["SyncOptions:FromDate"] = "2023-01-01",
            ["SyncOptions:ToDate"] = "2023-12-31",
            ["SyncOptions:IncludePlayerStats"] = "false",
            ["SyncOptions:IncludeMatchDetails"] = "false",
            ["SyncOptions:CalculateAnalytics"] = "false",
            ["SyncOptions:SyncHistoricalData"] = "true",
            ["SyncOptions:BatchSize"] = "25",
            ["SyncOptions:MatchFormats:0"] = "T20",
            ["SyncOptions:MatchFormats:1"] = "ODI"
        };

        var configuration = ConfigurationTestHelper.CreateInMemoryConfiguration(configValues);
        var services = new ServiceCollection();
        services.Configure<SyncOptions>(configuration.GetSection("SyncOptions"));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetRequiredService<IOptions<SyncOptions>>();
        var config = options.Value;

        // Assert
        config.SeasonIds.ShouldBe(new List<int> { 1, 2 });
        config.CompetitionIds.ShouldBe(new List<int> { 10 });
        config.TeamIds.ShouldBe(new List<int> { 100, 200 });
        config.FromDate.ShouldBe(new DateTime(2023, 1, 1));
        config.ToDate.ShouldBe(new DateTime(2023, 12, 31));
        config.IncludePlayerStats.ShouldBeFalse();
        config.IncludeMatchDetails.ShouldBeFalse();
        config.CalculateAnalytics.ShouldBeFalse();
        config.SyncHistoricalData.ShouldBeTrue();
        config.BatchSize.ShouldBe(25);
        config.MatchFormats.ShouldBe(new List<string> { "T20", "ODI" });
    }

    [Fact]
    public void SyncOptions_ShouldHandleEmptyArrays()
    {
        // Arrange
        var configValues = new Dictionary<string, string?>
        {
            ["SyncOptions:BatchSize"] = "50"
        };

        var configuration = ConfigurationTestHelper.CreateInMemoryConfiguration(configValues);
        var services = new ServiceCollection();
        services.Configure<SyncOptions>(configuration.GetSection("SyncOptions"));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetRequiredService<IOptions<SyncOptions>>();
        var config = options.Value;

        // Assert
        config.SeasonIds.ShouldBeEmpty();
        config.CompetitionIds.ShouldBeEmpty();
        config.TeamIds.ShouldBeEmpty();
        config.MatchFormats.ShouldBeEmpty();
    }

    [Fact]
    public void AllConfigs_ShouldBindFromDefaultConfiguration()
    {
        // Arrange
        var configValues = ConfigurationTestHelper.GetDefaultTestConfiguration();
        var configuration = ConfigurationTestHelper.CreateInMemoryConfiguration(configValues);
        var services = new ServiceCollection();
        
        services.Configure<CricClubsConfig>(configuration.GetSection("CricClubsConfig"));
        services.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));
        services.Configure<SyncOptions>(configuration.GetSection("SyncOptions"));
        
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var cricClubsOptions = serviceProvider.GetRequiredService<IOptions<CricClubsConfig>>();
        var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseConfig>>();
        var syncOptions = serviceProvider.GetRequiredService<IOptions<SyncOptions>>();

        // Assert
        cricClubsOptions.Value.ShouldNotBeNull();
        cricClubsOptions.Value.ApiBaseUrl.ShouldBe("https://test.api.cricclubs.com");
        
        databaseOptions.Value.ShouldNotBeNull();
        databaseOptions.Value.ConnectionString.ShouldContain("TestDb");
        
        syncOptions.Value.ShouldNotBeNull();
        syncOptions.Value.BatchSize.ShouldBe(50);
        syncOptions.Value.MatchFormats.ShouldContain("T20");
        syncOptions.Value.MatchFormats.ShouldContain("ODI");
        syncOptions.Value.MatchFormats.ShouldContain("Test");
    }
}