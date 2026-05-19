using BrainBoost.API.DTOs.Flashcards;
using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;

    public FlashcardsController(IFlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FlashcardFilterDto filter)
    {
        var result = await _flashcardService.GetAllAsync(filter);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _flashcardService.GetByIdAsync(id);

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateFlashcardDto dto)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        var result = await _flashcardService.CreateAsync(dto, userId);

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFlashcardDto dto)
    {
        var result = await _flashcardService.UpdateAsync(id, dto);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _flashcardService.DeleteAsync(id);

        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/mark-known")]
    public async Task<IActionResult> MarkKnown(int id)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        await _flashcardService.MarkKnownAsync(id, userId);

        return Ok(new { message = "Flashcard known olaraq qeyd edildi." });
    }

    [Authorize]
    [HttpPost("{id}/mark-unknown")]
    public async Task<IActionResult> MarkUnknown(int id)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        await _flashcardService.MarkUnknownAsync(id, userId);

        return Ok(new { message = "Flashcard unknown olaraq qeyd edildi." });
    }
}