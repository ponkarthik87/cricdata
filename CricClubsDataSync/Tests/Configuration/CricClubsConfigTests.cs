using CricClubsDataSync.Configuration;
using Shouldly;
using Xunit;

namespace CricClubsDataSync.Tests.Configuration;

public class CricClubsConfigTests
{
    [Fact]
    public void CricClubsConfig_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var config = new CricClubsConfig();

        // Assert
        config.ApiBaseUrl.ShouldBe(string.Empty);
        config.Username.ShouldBe(string.Empty);
        config.Password.ShouldBe(string.Empty);
        config.ApiKey.ShouldBe(string.Empty);
        config.RequestTimeoutSeconds.ShouldBe(30);
        config.MaxRetryAttempts.ShouldBe(3);
        config.RateLimitDelayMs.ShouldBe(1000);
        config.MaxConcurrentRequests.ShouldBe(5);
    }

    [Fact]
    public void CricClubsConfig_ShouldAllowPropertyAssignment()
    {
        // Arrange
        var config = new CricClubsConfig();
        const string expectedApiBaseUrl = "https://api.cricclubs.com";
        const string expectedUsername = "testuser";
        const string expectedPassword = "testpass";
        const string expectedApiKey = "test-api-key";
        const int expectedTimeout = 60;
        const int expectedRetries = 5;
        const int expectedDelay = 2000;
        const int expectedConcurrent = 10;

        // Act
        config.ApiBaseUrl = expectedApiBaseUrl;
        config.Username = expectedUsername;
        config.Password = expectedPassword;
        config.ApiKey = expectedApiKey;
        config.RequestTimeoutSeconds = expectedTimeout;
        config.MaxRetryAttempts = expectedRetries;
        config.RateLimitDelayMs = expectedDelay;
        config.MaxConcurrentRequests = expectedConcurrent;

        // Assert
        config.ApiBaseUrl.ShouldBe(expectedApiBaseUrl);
        config.Username.ShouldBe(expectedUsername);
        config.Password.ShouldBe(expectedPassword);
        config.ApiKey.ShouldBe(expectedApiKey);
        config.RequestTimeoutSeconds.ShouldBe(expectedTimeout);
        config.MaxRetryAttempts.ShouldBe(expectedRetries);
        config.RateLimitDelayMs.ShouldBe(expectedDelay);
        config.MaxConcurrentRequests.ShouldBe(expectedConcurrent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void CricClubsConfig_ShouldAllowInvalidTimeoutValues(int timeout)
    {
        // Arrange
        var config = new CricClubsConfig();

        // Act
        config.RequestTimeoutSeconds = timeout;

        // Assert
        config.RequestTimeoutSeconds.ShouldBe(timeout);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void CricClubsConfig_ShouldAllowInvalidRetryValues(int retries)
    {
        // Arrange
        var config = new CricClubsConfig();

        // Act
        config.MaxRetryAttempts = retries;

        // Assert
        config.MaxRetryAttempts.ShouldBe(retries);
    }
}