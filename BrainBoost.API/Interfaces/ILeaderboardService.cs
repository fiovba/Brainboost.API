using BrainBoost.API.DTOs.Leaderboard;

namespace BrainBoost.API.Interfaces;

public interface ILeaderboardService
{
    Task<List<LeaderboardItemDto>> GetGlobalAsync();

    Task<List<LeaderboardItemDto>> GetWeeklyAsync();

    Task<List<LeaderboardItemDto>> GetMonthlyAsync();
}