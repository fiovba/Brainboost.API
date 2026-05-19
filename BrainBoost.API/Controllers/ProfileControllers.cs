using BrainBoost.API.DTOs.Profile;
using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        var result = await _profileService.GetMeAsync(userId);

        return Ok(result);
    }
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe()
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        await _profileService.DeleteAccountAsync(userId);

        return NoContent();
    }
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(UpdateProfileDto dto)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        var result = await _profileService.UpdateAsync(userId, dto);

        return Ok(result);
    }
}