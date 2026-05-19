using BrainBoost.API.DTOs.FlashcardSets;
using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FlashcardSetsController : ControllerBase
{
    private readonly IFlashcardSetService _flashcardSetService;

    public FlashcardSetsController(IFlashcardSetService flashcardSetService)
    {
        _flashcardSetService = flashcardSetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _flashcardSetService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _flashcardSetService.GetByIdAsync(id);

        if (result == null)
            return NotFound("Flashcard set tapılmadı.");

        return Ok(result);
    }

    [Authorize(Roles = "Student,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateFlashcardSetDto dto)
    {
        try
        {
            var userIdClaim =
                User.FindFirstValue("UserId") ??
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId token içində tapılmadı.");

            var userId = int.Parse(userIdClaim);

            var result = await _flashcardSetService.CreateAsync(dto, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.InnerException?.Message ?? ex.Message
            });
        }
    }

    [Authorize(Roles = "Student,Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateFlashcardSetDto dto)
    {
        try
        {
            var userIdClaim =
                User.FindFirstValue("UserId") ??
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId token içində tapılmadı.");

            var userId = int.Parse(userIdClaim);

            var result = await _flashcardSetService.UpdateAsync(id, dto, userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.InnerException?.Message ?? ex.Message
            });
        }
    }

    [Authorize(Roles = "Student,Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userIdClaim =
                User.FindFirstValue("UserId") ??
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId token içində tapılmadı.");

            var userId = int.Parse(userIdClaim);

            await _flashcardSetService.DeleteAsync(id, userId);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.InnerException?.Message ?? ex.Message
            });
        }
    }
}