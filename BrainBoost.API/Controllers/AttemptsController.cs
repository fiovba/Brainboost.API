using BrainBoost.API.DTOs.Attempts;
using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttemptsController : ControllerBase
{
    private readonly IAttemptService _attemptService;

    public AttemptsController(IAttemptService attemptService)
    {
        _attemptService = attemptService;
    }

    [HttpPost("start/{quizId}")]
    public async Task<IActionResult> Start(int quizId)
    {
        var userId = int.Parse(
            User.FindFirstValue("UserId")!
        );

        var result = await _attemptService.StartAsync(
            quizId,
            userId
        );

        return Ok(result);
    }

    [HttpPost("{attemptId}/submit")]
    public async Task<IActionResult> Submit(
        int attemptId,
        SubmitAttemptDto dto
    )
    {
        var userId = int.Parse(
            User.FindFirstValue("UserId")!
        );

        var result = await _attemptService.SubmitAsync(
            attemptId,
            userId,
            dto
        );

        return Ok(result);
    }
    [HttpGet("{attemptId}/review")]
    public async Task<IActionResult> Review(int attemptId)
    {
        var userId = int.Parse(User.FindFirst("Userid")!.Value);

        var result = await _attemptService
            .GetReviewAsync(attemptId, userId);

        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> MyAttempts()
    {
        var userId = int.Parse(
            User.FindFirstValue("UserId")!
        );

        var result = await _attemptService
            .GetMyAttemptsAsync(userId);

        return Ok(result);
    }
}