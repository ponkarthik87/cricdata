namespace CricClubsDataSync.Configuration;

public class CricClubsConfig
{
    public string ApiBaseUrl { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int RequestTimeoutSeconds { get; set; } = 30;
    public int MaxRetryAttempts { get; set; } = 3;
    public int RateLimitDelayMs { get; set; } = 1000;
    public int MaxConcurrentRequests { get; set; } = 5;
}