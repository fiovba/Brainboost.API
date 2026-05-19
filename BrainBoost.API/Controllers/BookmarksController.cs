using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookmarksController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarksController(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    [HttpPost("quiz/{quizId}")]
    public async Task<IActionResult> AddQuizBookmark(int quizId)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        await _bookmarkService.AddQuizBookmarkAsync(quizId, userId);

        return Ok(new { message = "Quiz bookmark edildi." });
    }

    [HttpPost("flashcard/{flashcardId}")]
    public async Task<IActionResult> AddFlashcardBookmark(int flashcardId)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        await _bookmarkService.AddFlashcardBookmarkAsync(flashcardId, userId);

        return Ok(new { message = "Flashcard bookmark edildi." });
    }

    [HttpGet("my")]
    public async Task<IActionResult> MyBookmarks()
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        var result = await _bookmarkService.GetMyBookmarksAsync(userId);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        await _bookmarkService.DeleteAsync(id, userId);

        return NoContent();
    }
}