using CricClubsDataSync.Configuration;
using Shouldly;
using Xunit;

namespace CricClubsDataSync.Tests.Configuration;

public class SyncOptionsTests
{
    [Fact]
    public void SyncOptions_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var options = new SyncOptions();

        // Assert
        options.SeasonIds.ShouldNotBeNull();
        options.SeasonIds.ShouldBeEmpty();
        options.CompetitionIds.ShouldNotBeNull();
        options.CompetitionIds.ShouldBeEmpty();
        options.TeamIds.ShouldNotBeNull();
        options.TeamIds.ShouldBeEmpty();
        options.FromDate.ShouldBeNull();
        options.ToDate.ShouldBeNull();
        options.IncludePlayerStats.ShouldBeTrue();
        options.IncludeMatchDetails.ShouldBeTrue();
        options.CalculateAnalytics.ShouldBeTrue();
        options.SyncHistoricalData.ShouldBeFalse();
        options.BatchSize.ShouldBe(50);
        options.MatchFormats.ShouldNotBeNull();
        options.MatchFormats.ShouldBeEmpty();
    }

    [Fact]
    public void SyncOptions_ShouldAllowPropertyAssignment()
    {
        // Arrange
        var options = new SyncOptions();
        var expectedSeasonIds = new List<int> { 1, 2, 3 };
        var expectedCompetitionIds = new List<int> { 10, 20 };
        var expectedTeamIds = new List<int> { 100, 200, 300 };
        var expectedFromDate = new DateTime(2023, 1, 1);
        var expectedToDate = new DateTime(2023, 12, 31);
        var expectedMatchFormats = new List<string> { "T20", "ODI" };
        const bool expectedPlayerStats = false;
        const bool expectedMatchDetails = false;
        const bool expectedAnalytics = false;
        const bool expectedHistorical = true;
        const int expectedBatchSize = 100;

        // Act
        options.SeasonIds = expectedSeasonIds;
        options.CompetitionIds = expectedCompetitionIds;
        options.TeamIds = expectedTeamIds;
        options.FromDate = expectedFromDate;
        options.ToDate = expectedToDate;
        options.IncludePlayerStats = expectedPlayerStats;
        options.IncludeMatchDetails = expectedMatchDetails;
        options.CalculateAnalytics = expectedAnalytics;
        options.SyncHistoricalData = expectedHistorical;
        options.BatchSize = expectedBatchSize;
        options.MatchFormats = expectedMatchFormats;

        // Assert
        options.SeasonIds.ShouldBe(expectedSeasonIds);
        options.CompetitionIds.ShouldBe(expectedCompetitionIds);
        options.TeamIds.ShouldBe(expectedTeamIds);
        options.FromDate.ShouldBe(expectedFromDate);
        options.ToDate.ShouldBe(expectedToDate);
        options.IncludePlayerStats.ShouldBe(expectedPlayerStats);
        options.IncludeMatchDetails.ShouldBe(expectedMatchDetails);
        options.CalculateAnalytics.ShouldBe(expectedAnalytics);
        options.SyncHistoricalData.ShouldBe(expectedHistorical);
        options.BatchSize.ShouldBe(expectedBatchSize);
        options.MatchFormats.ShouldBe(expectedMatchFormats);
    }

    [Fact]
    public void SyncOptions_ShouldAllowEmptyLists()
    {
        // Arrange
        var options = new SyncOptions();
        var emptyList = new List<int>();
        var emptyStringList = new List<string>();

        // Act
        options.SeasonIds = emptyList;
        options.CompetitionIds = emptyList;
        options.TeamIds = emptyList;
        options.MatchFormats = emptyStringList;

        // Assert
        options.SeasonIds.ShouldBeEmpty();
        options.CompetitionIds.ShouldBeEmpty();
        options.TeamIds.ShouldBeEmpty();
        options.MatchFormats.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(1000)]
    public void SyncOptions_ShouldAllowAnyBatchSize(int batchSize)
    {
        // Arrange
        var options = new SyncOptions();

        // Act
        options.BatchSize = batchSize;

        // Assert
        options.BatchSize.ShouldBe(batchSize);
    }

    [Fact]
    public void SyncOptions_ShouldAllowDateRanges()
    {
        // Arrange
        var options = new SyncOptions();
        var fromDate = new DateTime(2020, 1, 1);
        var toDate = new DateTime(2025, 12, 31);

        // Act
        options.FromDate = fromDate;
        options.ToDate = toDate;

        // Assert
        options.FromDate.ShouldBe(fromDate);
        options.ToDate.ShouldBe(toDate);
    }

    [Fact]
    public void SyncOptions_ShouldAllowNullDates()
    {
        // Arrange
        var options = new SyncOptions();

        // Act
        options.FromDate = null;
        options.ToDate = null;

        // Assert
        options.FromDate.ShouldBeNull();
        options.ToDate.ShouldBeNull();
    }
}