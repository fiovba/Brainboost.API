using BrainBoost.API.DTOs.Questions;
using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet("quizzes/{quizId}/questions")]
    public async Task<IActionResult> GetByQuizId(int quizId)
    {
        var result = await _questionService.GetByQuizIdAsync(quizId);

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost("quizzes/{quizId}/questions")]
    public async Task<IActionResult> Create(
        int quizId,
        CreateQuestionDto dto
    )
    {
        var result = await _questionService.CreateAsync(
            quizId,
            dto
        );

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("questions/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _questionService.DeleteAsync(id);

        return NoContent();
    }
}