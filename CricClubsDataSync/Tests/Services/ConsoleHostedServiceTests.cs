using CricClubsDataSync.Configuration;
using CricClubsDataSync.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace CricClubsDataSync.Tests.Services;

public class ConsoleHostedServiceTests
{
    private readonly Mock<ILogger<ConsoleHostedService>> _mockLogger;
    private readonly Mock<IOptions<CricClubsConfig>> _mockCricClubsOptions;
    private readonly Mock<IOptions<DatabaseConfig>> _mockDatabaseOptions;
    private readonly Mock<IOptions<SyncOptions>> _mockSyncOptions;
    private readonly Mock<IHostApplicationLifetime> _mockAppLifetime;
    private readonly CricClubsConfig _cricClubsConfig;
    private readonly DatabaseConfig _databaseConfig;
    private readonly SyncOptions _syncOptions;

    public ConsoleHostedServiceTests()
    {
        _mockLogger = new Mock<ILogger<ConsoleHostedService>>();
        _mockCricClubsOptions = new Mock<IOptions<CricClubsConfig>>();
        _mockDatabaseOptions = new Mock<IOptions<DatabaseConfig>>();
        _mockSyncOptions = new Mock<IOptions<SyncOptions>>();
        _mockAppLifetime = new Mock<IHostApplicationLifetime>();

        _cricClubsConfig = new CricClubsConfig
        {
            ApiBaseUrl = "https://api.cricclubs.com",
            Username = "testuser",
            Password = "testpass",
            ApiKey = "test-key"
        };

        _databaseConfig = new DatabaseConfig
        {
            ConnectionString = "Server=localhost;Database=TestDb;Trusted_Connection=true;"
        };

        _syncOptions = new SyncOptions
        {
            BatchSize = 100,
            IncludePlayerStats = true
        };

        _mockCricClubsOptions.Setup(x => x.Value).Returns(_cricClubsConfig);
        _mockDatabaseOptions.Setup(x => x.Value).Returns(_databaseConfig);
        _mockSyncOptions.Setup(x => x.Value).Returns(_syncOptions);
    }

    [Fact]
    public void ConsoleHostedService_ShouldInitializeWithDependencies()
    {
        // Arrange & Act
        var service = new ConsoleHostedService(
            _mockLogger.Object,
            _mockCricClubsOptions.Object,
            _mockDatabaseOptions.Object,
            _mockSyncOptions.Object,
            _mockAppLifetime.Object);

        // Assert
        service.ShouldNotBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogStartupMessages()
    {
        // Arrange
        var service = new ConsoleHostedService(
            _mockLogger.Object,
            _mockCricClubsOptions.Object,
            _mockDatabaseOptions.Object,
            _mockSyncOptions.Object,
            _mockAppLifetime.Object);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

        // Act
        await service.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(200, CancellationToken.None); // Allow some time for execution
        await service.StopAsync(CancellationToken.None);

        // Assert
        VerifyLogCalled(LogLevel.Information, "CricClubs Data Sync service starting...");
        VerifyLogCalled(LogLevel.Information, "Configuration loaded successfully");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogApiBaseUrl()
    {
        // Arrange
        var service = new ConsoleHostedService(
            _mockLogger.Object,
            _mockCricClubsOptions.Object,
            _mockDatabaseOptions.Object,
            _mockSyncOptions.Object,
            _mockAppLifetime.Object);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

        // Act
        await service.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(200, CancellationToken.None);
        await service.StopAsync(CancellationToken.None);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("API Base URL: https://api.cricclubs.com")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogDatabaseConnectionStatus_WhenConnectionStringExists()
    {
        // Arrange
        var service = new ConsoleHostedService(
            _mockLogger.Object,
            _mockCricClubsOptions.Object,
            _mockDatabaseOptions.Object,
            _mockSyncOptions.Object,
            _mockAppLifetime.Object);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

        // Act
        await service.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(200, CancellationToken.None);
        await service.StopAsync(CancellationToken.None);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Database Connection configured: True")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogDatabaseConnectionStatus_WhenConnectionStringEmpty()
    {
        // Arrange
        var emptyDatabaseConfig = new DatabaseConfig { ConnectionString = string.Empty };
        _mockDatabaseOptions.Setup(x => x.Value).Returns(emptyDatabaseConfig);

        var service = new ConsoleHostedService(
            _mockLogger.Object,
            _mockCricClubsOptions.Object,
            _mockDatabaseOptions.Object,
            _mockSyncOptions.Object,
            _mockAppLifetime.Object);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

        // Act
        await service.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(200, CancellationToken.None);
        await service.StopAsync(CancellationToken.None);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Database Connection configured: False")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallStopApplication()
    {
        // Arrange
        var service = new ConsoleHostedService(
            _mockLogger.Object,
            _mockCricClubsOptions.Object,
            _mockDatabaseOptions.Object,
            _mockSyncOptions.Object,
            _mockAppLifetime.Object);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

        // Act
        await service.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(200, CancellationToken.None);
        await service.StopAsync(CancellationToken.None);

        // Assert
        _mockAppLifetime.Verify(x => x.StopApplication(), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogCompletionMessage()
    {
        // Arrange
        var service = new ConsoleHostedService(
            _mockLogger.Object,
            _mockCricClubsOptions.Object,
            _mockDatabaseOptions.Object,
            _mockSyncOptions.Object,
            _mockAppLifetime.Object);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

        // Act
        await service.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(200, CancellationToken.None);
        await service.StopAsync(CancellationToken.None);

        // Assert
        VerifyLogCalled(LogLevel.Information, "CricClubs Data Sync service completed successfully");
    }

    [Fact]
    public void ConsoleHostedService_ShouldThrowExceptionInConstructor_WhenConfigurationIsInvalid()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ConsoleHostedService>>();
        var mockOptions = new Mock<IOptions<CricClubsConfig>>();
        
        // Setup to throw an exception when accessing the Value property
        mockOptions.Setup(x => x.Value).Throws(new InvalidOperationException("Test exception"));

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => new ConsoleHostedService(
            mockLogger.Object,
            mockOptions.Object,
            _mockDatabaseOptions.Object,
            _mockSyncOptions.Object,
            _mockAppLifetime.Object));
    }

    private void VerifyLogCalled(LogLevel logLevel, string message)
    {
        _mockLogger.Verify(
            x => x.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }
}