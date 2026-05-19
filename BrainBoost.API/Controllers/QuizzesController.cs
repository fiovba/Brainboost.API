using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Quizzes;
using BrainBoost.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly AppDbContext _context;

    public QuizzesController(
        IQuizService quizService,
        AppDbContext context
    )
    {
        _quizService = quizService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QuizFilterDto filter)
    {
        var result = await _quizService.GetAllAsync(filter);

        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await _quizService.GetBySlugAsync(slug);

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateQuizDto dto)
    {
        var userId = int.Parse(
            User.FindFirstValue("UserId")!
        );

        var result = await _quizService.CreateAsync(dto, userId);

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateQuizDto dto)
    {
        var result = await _quizService.UpdateAsync(id, dto);

        return Ok(result);
    }
    [HttpGet("by-id/{id:int}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(int id)
    {
        var quiz = await _context.Quizzes
            .Include(x => x.Category)
            .Include(x => x.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (quiz == null)
            return NotFound("Quiz tapılmadı.");

        return Ok(new
        {
            id = quiz.Id,
            title = quiz.Title,
            slug = quiz.Slug,
            description = quiz.Description,
            categoryId = quiz.CategoryId,
            category = quiz.Category.Name,
            difficulty = quiz.Difficulty,
            timeLimitMinutes = quiz.TimeLimitMinutes,
            passingScore = quiz.PassingScore,
            isPublished = quiz.IsPublished,
            questions = quiz.Questions.Select(q => new
            {
                id = q.Id,
                text = q.QuestionText,
                questionType = q.QuestionType,
                explanation = q.Explanation,
                points = q.Points,
                options = q.Options.Select(o => new
                {
                    id = o.Id,
                    text = o.OptionText,
                    isCorrect = o.IsCorrect
                }).ToList()
            }).ToList()
        });
    }
    [HttpGet("my")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetMyQuizzes()
    {
        var userId = int.Parse(User.FindFirstValue("UserId")!);

        var quizzes = await _context.Quizzes
            .Include(x => x.Category)
            .Include(x => x.Questions)
            .Where(x => x.CreatedByUserId == userId)
            .Select(x => new
            {
                id = x.Id,
                title = x.Title,
                slug = x.Slug,
                description = x.Description,
              
                difficulty = x.Difficulty,
                timeLimitMinutes = x.TimeLimitMinutes,
                passingScore = x.PassingScore,
                category = x.Category.Name,
                questionCount = x.Questions.Count,
                createdAt = x.CreatedAt,
                isPublished = x.IsPublished
            })
            .OrderByDescending(x => x.createdAt)
            .ToListAsync();

        return Ok(quizzes);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _quizService.DeleteAsync(id);

        return NoContent();
    }
}