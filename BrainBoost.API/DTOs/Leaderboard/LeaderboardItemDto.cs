namespace BrainBoost.API.DTOs.Leaderboard;

public class LeaderboardItemDto
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? ProfileImageUrl { get; set; }

    public int TotalScore { get; set; }

    public int CompletedQuizzes { get; set; }

    public double AveragePercentage { get; set; }
}