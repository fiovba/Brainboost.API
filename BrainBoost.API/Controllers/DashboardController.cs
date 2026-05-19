using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(
        IDashboardService dashboardService
    )
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("student")]
    public async Task<IActionResult> StudentDashboard()
    {
        var userId = int.Parse(
            User.FindFirstValue("UserId")!
        );

        var result = await _dashboardService
            .GetStudentDashboardAsync(userId);

        return Ok(result);
    }
    [Authorize(Roles = "Admin,Teacher")]
    [HttpGet("teacher")]
    public async Task<IActionResult> TeacherDashboard()
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        var result = await _dashboardService
            .GetTeacherDashboardAsync(userId);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<IActionResult> AdminDashboard()
    {
        var result = await _dashboardService
            .GetAdminDashboardAsync();

        return Ok(result);
    }
}