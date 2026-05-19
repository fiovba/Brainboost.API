using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Leaderboard;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly AppDbContext _context;

    public LeaderboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaderboardItemDto>> GetGlobalAsync()
    {
        return await BuildLeaderboardAsync(null);
    }

    public async Task<List<LeaderboardItemDto>> GetWeeklyAsync()
    {
        var startDate = DateTime.UtcNow.AddDays(-7);

        return await BuildLeaderboardAsync(startDate);
    }

    public async Task<List<LeaderboardItemDto>> GetMonthlyAsync()
    {
        var startDate = DateTime.UtcNow.AddDays(-30);

        return await BuildLeaderboardAsync(startDate);
    }

    private async Task<List<LeaderboardItemDto>>
        BuildLeaderboardAsync(DateTime? startDate)
    {
        var attemptsQuery = _context.QuizAttempts
            .Include(x => x.User)
            .Where(x => x.Status == "Completed");

        if (startDate.HasValue)
        {
            attemptsQuery = attemptsQuery
                .Where(x =>
                    x.SubmittedAt != null &&
                    x.SubmittedAt >= startDate.Value);
        }

        var leaderboard = await attemptsQuery
            .GroupBy(x => new
            {
                x.UserId,
                x.User.FullName,
                x.User.ProfileImageUrl
            })
            .Select(g => new LeaderboardItemDto
            {
                UserId = g.Key.UserId,

                FullName = g.Key.FullName,

                ProfileImageUrl =
                    g.Key.ProfileImageUrl,

                TotalScore = g.Sum(x => x.Score),

                CompletedQuizzes = g.Count(),

                AveragePercentage =
                    Math.Round(
                        g.Average(x =>
                            x.TotalPoints == 0
                                ? 0
                                : ((double)x.Score /
                                   x.TotalPoints) * 100
                        ),
                        2
                    )
            })
            .OrderByDescending(x => x.TotalScore)
            .ThenByDescending(x => x.AveragePercentage)
            .Take(20)
            .ToListAsync();

        return leaderboard;
    }
}