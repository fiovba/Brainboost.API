using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(
        ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpGet("global")]
    public async Task<IActionResult> Global()
    {
        var result = await _leaderboardService
            .GetGlobalAsync();

        return Ok(result);
    }

    [HttpGet("weekly")]
    public async Task<IActionResult> Weekly()
    {
        var result = await _leaderboardService
            .GetWeeklyAsync();

        return Ok(result);
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> Monthly()
    {
        var result = await _leaderboardService
            .GetMonthlyAsync();

        return Ok(result);
    }
}