namespace CricClubsDataSync.Configuration;

public class SyncOptions
{
    public List<int> SeasonIds { get; set; } = new List<int>();
    public List<int> CompetitionIds { get; set; } = new List<int>();
    public List<int> TeamIds { get; set; } = new List<int>();
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool IncludePlayerStats { get; set; } = true;
    public bool IncludeMatchDetails { get; set; } = true;
    public bool CalculateAnalytics { get; set; } = true;
    public bool SyncHistoricalData { get; set; } = false;
    public int BatchSize { get; set; } = 50;
    public List<string> MatchFormats { get; set; } = new List<string>();
}