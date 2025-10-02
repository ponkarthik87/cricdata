using CricClubsDataSync.Configuration;
using CricClubsDataSync.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace CricClubsDataSync.Tests;

public class ProgramIntegrationTests
{
    [Fact]
    public void CreateHostBuilder_ShouldConfigureServices()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var hostBuilder = Program.CreateHostBuilder(args);
        var host = hostBuilder.Build();

        // Assert
        host.ShouldNotBeNull();
        
        // Verify services are registered
        var serviceProvider = host.Services;
        serviceProvider.GetService<IOptions<CricClubsConfig>>().ShouldNotBeNull();
        serviceProvider.GetService<IOptions<DatabaseConfig>>().ShouldNotBeNull();
        serviceProvider.GetService<IOptions<SyncOptions>>().ShouldNotBeNull();
        serviceProvider.GetService<IHttpClientFactory>().ShouldNotBeNull();
        
        // Verify hosted service is registered
        var hostedServices = serviceProvider.GetServices<IHostedService>();
        hostedServices.ShouldContain(service => service is ConsoleHostedService);
    }

    [Fact]
    public void CreateHostBuilder_ShouldConfigureConfiguration()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var hostBuilder = Program.CreateHostBuilder(args);
        var host = hostBuilder.Build();

        // Assert
        var configuration = host.Services.GetRequiredService<IConfiguration>();
        configuration.ShouldNotBeNull();
        
        // Verify configuration sections exist
        var cricClubsSection = configuration.GetSection("CricClubsConfig");
        cricClubsSection.ShouldNotBeNull();
        cricClubsSection.Exists().ShouldBeTrue();
        
        var databaseSection = configuration.GetSection("DatabaseConfig");
        databaseSection.ShouldNotBeNull();
        databaseSection.Exists().ShouldBeTrue();
        
        var syncSection = configuration.GetSection("SyncOptions");
        syncSection.ShouldNotBeNull();
        syncSection.Exists().ShouldBeTrue();
    }

    [Fact]
    public void CreateHostBuilder_ShouldBindConfigurationToOptions()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var hostBuilder = Program.CreateHostBuilder(args);
        var host = hostBuilder.Build();

        // Assert
        var cricClubsOptions = host.Services.GetRequiredService<IOptions<CricClubsConfig>>();
        var cricClubsConfig = cricClubsOptions.Value;
        cricClubsConfig.ShouldNotBeNull();
        cricClubsConfig.ApiBaseUrl.ShouldBe("https://api.cricclubs.com");
        cricClubsConfig.RequestTimeoutSeconds.ShouldBe(30);
        cricClubsConfig.MaxRetryAttempts.ShouldBe(3);

        var databaseOptions = host.Services.GetRequiredService<IOptions<DatabaseConfig>>();
        var databaseConfig = databaseOptions.Value;
        databaseConfig.ShouldNotBeNull();
        databaseConfig.ConnectionString.ShouldContain("CricClubsData");
        databaseConfig.CommandTimeoutSeconds.ShouldBe(30);

        var syncOptions = host.Services.GetRequiredService<IOptions<SyncOptions>>();
        var syncConfig = syncOptions.Value;
        syncConfig.ShouldNotBeNull();
        syncConfig.BatchSize.ShouldBe(50);
        syncConfig.IncludePlayerStats.ShouldBeTrue();
        syncConfig.MatchFormats.ShouldContain("T20");
        syncConfig.MatchFormats.ShouldContain("ODI");
        syncConfig.MatchFormats.ShouldContain("Test");
    }

    [Fact]
    public void CreateHostBuilder_ShouldConfigureHttpClient()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var hostBuilder = Program.CreateHostBuilder(args);
        var host = hostBuilder.Build();

        // Assert
        var httpClientFactory = host.Services.GetRequiredService<IHttpClientFactory>();
        httpClientFactory.ShouldNotBeNull();
        
        var httpClient = httpClientFactory.CreateClient();
        httpClient.ShouldNotBeNull();
    }

    [Fact]
    public void CreateHostBuilder_ShouldAcceptCommandLineArguments()
    {
        // Arrange
        var args = new[] { "--environment", "Development" };

        // Act
        var hostBuilder = Program.CreateHostBuilder(args);
        var host = hostBuilder.Build();

        // Assert
        host.ShouldNotBeNull();
        var configuration = host.Services.GetRequiredService<IConfiguration>();
        configuration["environment"].ShouldBe("Development");
    }

    [Fact]
    public void CreateHostBuilder_ShouldConfigureLogging()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var hostBuilder = Program.CreateHostBuilder(args);
        var host = hostBuilder.Build();

        // Assert
        var loggerFactory = host.Services.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
        loggerFactory.ShouldNotBeNull();
    }

    [Fact]
    public void CreateHostBuilder_ShouldRegisterConsoleHostedService()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var hostBuilder = Program.CreateHostBuilder(args);
        var host = hostBuilder.Build();

        // Assert
        var hostedServices = host.Services.GetServices<IHostedService>().ToList();
        hostedServices.ShouldNotBeEmpty();
        
        var consoleHostedService = hostedServices.FirstOrDefault(s => s is ConsoleHostedService);
        consoleHostedService.ShouldNotBeNull();
        consoleHostedService.ShouldBeOfType<ConsoleHostedService>();
    }

    [Fact]
    public void CreateHostBuilder_ShouldConfigureEnvironmentSpecificSettings()
    {
        // Arrange
        var args = Array.Empty<string>();
        var originalEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");

        try
        {
            // Act
            var hostBuilder = Program.CreateHostBuilder(args);
            var host = hostBuilder.Build();

            // Assert
            var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
            hostEnvironment.ShouldNotBeNull();
            hostEnvironment.EnvironmentName.ShouldBe("Development");
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", originalEnvironment);
        }
    }
}