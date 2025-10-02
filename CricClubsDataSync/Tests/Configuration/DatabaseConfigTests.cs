using CricClubsDataSync.Configuration;
using Shouldly;
using Xunit;

namespace CricClubsDataSync.Tests.Configuration;

public class DatabaseConfigTests
{
    [Fact]
    public void DatabaseConfig_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var config = new DatabaseConfig();

        // Assert
        config.ConnectionString.ShouldBe(string.Empty);
        config.CommandTimeoutSeconds.ShouldBe(30);
        config.EnableSensitiveDataLogging.ShouldBeFalse();
        config.EnableDetailedErrors.ShouldBeFalse();
    }

    [Fact]
    public void DatabaseConfig_ShouldAllowPropertyAssignment()
    {
        // Arrange
        var config = new DatabaseConfig();
        const string expectedConnectionString = "Server=localhost;Database=TestDb;Trusted_Connection=true;";
        const int expectedTimeout = 60;
        const bool expectedSensitiveLogging = true;
        const bool expectedDetailedErrors = true;

        // Act
        config.ConnectionString = expectedConnectionString;
        config.CommandTimeoutSeconds = expectedTimeout;
        config.EnableSensitiveDataLogging = expectedSensitiveLogging;
        config.EnableDetailedErrors = expectedDetailedErrors;

        // Assert
        config.ConnectionString.ShouldBe(expectedConnectionString);
        config.CommandTimeoutSeconds.ShouldBe(expectedTimeout);
        config.EnableSensitiveDataLogging.ShouldBe(expectedSensitiveLogging);
        config.EnableDetailedErrors.ShouldBe(expectedDetailedErrors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void DatabaseConfig_ShouldAllowEmptyConnectionString(string connectionString)
    {
        // Arrange
        var config = new DatabaseConfig();

        // Act
        config.ConnectionString = connectionString;

        // Assert
        config.ConnectionString.ShouldBe(connectionString);
    }

    [Fact]
    public void DatabaseConfig_ShouldAllowNullConnectionString()
    {
        // Arrange
        var config = new DatabaseConfig();

        // Act
        config.ConnectionString = null!;

        // Assert
        config.ConnectionString.ShouldBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    public void DatabaseConfig_ShouldAllowAnyTimeoutValue(int timeout)
    {
        // Arrange
        var config = new DatabaseConfig();

        // Act
        config.CommandTimeoutSeconds = timeout;

        // Assert
        config.CommandTimeoutSeconds.ShouldBe(timeout);
    }
}